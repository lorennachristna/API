using Nest;
using Pluviometrico.Core.Repository.Interface;
using Pluviometrico.Data;
using Pluviometrico.Data.DatabaseContext;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace Pluviometrico.Core.Repository
{
    public class MeasuredRainfallRepository : IMeasuredRainfallRepository
    {
        private readonly IElasticClient _elasticClient;
        private readonly string _distanceCalculationString = "6371 * Math.acos(Math.cos(-22.9060000000000*Math.PI/180) * Math.cos(doc['latitude'].value*Math.PI/180) * Math.cos(-43.0530000000000*Math.PI/180 - (doc['longitude'].value*Math.PI/180)) + Math.sin(-22.9060000000000*Math.PI/180) * Math.sin(doc['latitude'].value*Math.PI/180))";

        public MeasuredRainfallRepository(IElasticClient elasticClient)
        {
            _elasticClient = elasticClient;
        }

        //TODO: Google difference between match and term, which one is better for texts, ints, etc.
        //TODO: Describe each ES type (query, bool, must, etc)
        public async Task<List<MeasuredRainfall>> GetListByMonthAndYear(int month, int year)
        {
            var response = await _elasticClient.SearchAsync<MeasuredRainfall>(s =>
                s.Query(q =>
                    q.Bool(b => 
                        b.Must(m =>
                            m.Term(t => t.Field(f => f.Mes).Value(month)) &&
                            m.Term(t => t.Field(f => f.Ano).Value(year))
                        )
                    )
                )
            );

            return response?.Documents?.ToList();
        }

        //TODO: Check if adding "distancia" field significantly slows response time"?
        //TODO: Remove local variable distanceCalculationString (replace by global _distanceCalculationString)
        public async Task<List<ElasticSearchHit>> GetByDistanceAndYearRange(int greaterThanYear, int lessThanYear, double distance)
        {
            var distanceCalculationString = "6371 * Math.acos(Math.cos(-22.9060000000000*Math.PI/180) * Math.cos(doc['latitude'].value*Math.PI/180) * Math.cos(-43.0530000000000*Math.PI/180 - (doc['longitude'].value*Math.PI/180)) + Math.sin(-22.9060000000000*Math.PI/180) * Math.sin(doc['latitude'].value*Math.PI/180))";

            var response = await _elasticClient.SearchAsync<MeasuredRainfall>(s => s
                .Source(true)
                .ScriptFields(sf =>
                    sf.ScriptField("distancia", script => script
                        .Source(distanceCalculationString)
                    )
                )
                .Query(q =>
                    q.Bool(b => b
                        .Filter(f => f.Script(s => s.Script(s => s.Source($"double distancia = {distanceCalculationString} ; return distancia < {distance};"))))
                        .Must(m => m.Range(r => r.Field(f => f.Ano).GreaterThanOrEquals(greaterThanYear).LessThanOrEquals(lessThanYear)))
                    )
                )
            );
            //TODO: mudar para lista de objetos - fields como distancia (assim como no método FilterByDistance)
            var hits = response?.Hits?.Select(h => {
                return new ElasticSearchHit
                {
                    Source = h.Source,
                    //Created class "Fields" has to be declared as "Data.Fields" (Data is the folder) to avoid ambiguity
                    Fields = new Data.Fields { Distancia = h.Fields.Value<double>("distancia") }
                };
             });

            return hits.ToList();
        }

        public async Task<List<ElasticSearchHit>> GetByDistanceAndYear(int year, double distance)
        {
            var distanceCalculationString = "6371 * Math.acos(Math.cos(-22.9060000000000*Math.PI/180) * Math.cos(doc['latitude'].value*Math.PI/180) * Math.cos(-43.0530000000000*Math.PI/180 - (doc['longitude'].value*Math.PI/180)) + Math.sin(-22.9060000000000*Math.PI/180) * Math.sin(doc['latitude'].value*Math.PI/180))";

            var response = await _elasticClient.SearchAsync<MeasuredRainfall>(s => s
                .Source(true)
                .ScriptFields(sf =>
                    sf.ScriptField("distancia", script => script
                        .Source(distanceCalculationString)
                    )
                )
                .Query(q =>
                    q.Bool(b => b
                        .Filter(f => f.Script(s => s.Script(s => s.Source($"double distancia = {distanceCalculationString} ; return distancia < {distance};"))))
                        .Must(m => m.Term(t => t.Field(f => f.Ano).Value(year)))
                    )
                )
            );

            var hits = response?.Hits?.Select(h => {
                return new ElasticSearchHit
                {
                    Source = h.Source,
                    //Created class "Fields" has to be declared as "Data.Fields" (Data is the folder) to avoid ambiguity
                    Fields = new Data.Fields { Distancia = h.Fields.Value<double>("distancia") }
                };
            });

            return hits.ToList();
        }

        //TODO: check if .keyword makes any difference (yes it does, if i don't use it, it returns an error)
        //TODO: remove "year" aggregation: year is already determined by query
        public async Task<List<object>> GetValueAggregationsByDate(int year)
        {
            var response = await _elasticClient.SearchAsync<MeasuredRainfall>(s => s
                .Size(0)
                .Aggregations(a => a.Terms("municipio", t => t
                    .Field(f => f.Municipio.Suffix("keyword"))
                    //aggregations inside "municipio"
                    .Aggregations(a => a.Terms("mes", t => t
                        .Field(f => f.Mes)
                        .Aggregations(a => a.Terms("ano", t => t
                            .Field(f => f.Ano)
                            .Aggregations(a => a.Sum("soma", s => s
                                .Field(f => f.ValorMedida)
                            ))
                        ))
                    ))
                ))
                .Query(q => q.Bool(b => b.Must(m => m.Term(t => t.Field(f => f.Ano).Value(year))))
                )
            );

            var filteredResponse = new List<object>();

            var cityBuckets = response.Aggregations.Terms("municipio").Buckets;
            foreach (var cityBucket in cityBuckets)
            {
                var city = cityBucket.Key;
                var monthBuckets = cityBucket.Terms("mes").Buckets;
                foreach(var monthBucket in monthBuckets)
                {
                    var month = monthBucket.Key; 
                    var yearBuckets = monthBucket.Terms<double>("ano").Buckets;
                    foreach(var yearBucket in yearBuckets)
                    {
                        var responseYear = yearBucket.Key;
                        var sumValueBuckets = yearBucket.Sum("soma").Value;
                        filteredResponse.Add(new {
                            cidade = city,
                            mes = month,
                            year = responseYear,
                            soma_valormedida = sumValueBuckets
                        });
                    }
                }
            }
            return filteredResponse;
        }

        //TODO: remove "year" aggregation: year is already determined by query
        public async Task<List<object>> GetValueAggregationsByDistance(int year, double distance)
        {
            var response = await _elasticClient.SearchAsync<MeasuredRainfall>(s => s
                .Size(0)
                .RuntimeFields<MeasuredRainfallRuntimeFields>(r => r
                    .RuntimeField(f => f.Distancia, FieldType.Double, f => f
                        .Script($"double distancia = {_distanceCalculationString}; emit(distancia);"))
                )
                .Aggregations(a => a
                    .Terms("municipio", t => t
                        .Field(f => f.Municipio.Suffix("keyword"))
                        .Aggregations(a => a
                            .Terms("mes", t => t
                                .Field(f => f.Mes)
                                .Aggregations(a => a
                                   .Terms("ano", t => t
                                       .Field(f => f.Ano)
                                       .Aggregations(a => a
                                            .Terms("distancia", t => t
                                                .Field("distancia")
                                                .Aggregations(a => a
                                                    .Sum("soma", s => s.Field(f => f.ValorMedida)))
                                            )
                                        )
                                    )
                                )
                            )
                        )
                    )
                )
                .Query(q => q.Bool(b => b
                    .Must(m =>
                        m.Term(t => t.
                            Field(f => f.Ano)
                            .Value(year)
                         ) &&
                         m.Range(r => r
                            .Field("distancia")
                            .LessThan(distance)
                         )
                    )
                ))
            );

            var filteredResponse = new List<object>();

            var cityBuckets = response.Aggregations.Terms("municipio").Buckets;
            foreach (var cityBucket in cityBuckets)
            {
                var city = cityBucket.Key;
                var monthBuckets = cityBucket.Terms("mes").Buckets;
                foreach(var monthBucket in monthBuckets)
                {
                    var month = monthBucket.Key;
                    var yearBuckets = monthBucket.Terms("ano").Buckets;
                    foreach (var yearBucket in yearBuckets)
                    {
                        var responseYear = yearBucket.Key;
                        var distanceBuckets = yearBucket.Terms("distancia").Buckets;
                        foreach (var distanceBucket in distanceBuckets)
                        {
                            var responseDistance = distanceBucket.Key;
                            var sum = distanceBucket.Sum("soma").Value;
                            filteredResponse.Add(new
                            {
                                cidade = city,
                                mes = month,
                                ano = responseYear,
                                distancia = responseDistance,
                                soma = sum
                            });
                        }
                    }
                }
            }

            return filteredResponse;
        }

        public async Task<List<object>> GetValueAggregationsByDistanceGroupByStation(int year, double distance)
        {
            var response = await _elasticClient.SearchAsync<MeasuredRainfall>(s => s
                .RuntimeFields<MeasuredRainfallRuntimeFields>(r => r
                    .RuntimeField(r => r.Distancia, FieldType.Double, r => r
                        .Script($"double distancia = {_distanceCalculationString}; emit(distancia);")))
                .Aggregations(a => a
                    .Terms("codigoEstacao", t => t
                        .Field(f => f.CodEstacaoOriginal.Suffix("keyword"))
                        .Aggregations(a => a
                            .Terms("estacao", t => t
                                .Field(f => f.NomeEstacaoOriginal.Suffix("keyword"))
                                .Aggregations(a => a
                                    .Terms("municipio", t => t
                                        .Field(f => f.Municipio.Suffix("keyword"))
                                        .Aggregations(a => a
                                            .Terms("mes", t => t
                                                .Field(f => f.Mes)
                                                .Aggregations(a => a
                                                    .Terms("ano", t => t
                                                        .Field(f => f.Ano)
                                                        .Aggregations(a => a
                                                            .Terms("distancia", t => t
                                                                .Field("distancia")
                                                                .Aggregations(a => a
                                                                    .Average("media", s => s
                                                                        .Field(f => f.ValorMedida)))))))))))))))
                .Query(q => q.Bool(b => b.Must(m =>
                    m.Term(t => t
                        .Field(f => f.Ano)
                        .Value(year)) &&
                    m.Range(r => r
                        .Field("distancia")
                        .LessThan(distance))
                )))
            );

            var filteredResponse = new List<object>();

            var stationCodeBuckets = response.Aggregations.Terms("codigoEstacao").Buckets;
            foreach(var stationCodeBucket in stationCodeBuckets)
            {
                var stationCode = stationCodeBucket.Key;
                var stationBuckets = stationCodeBucket.Terms("estacao").Buckets;
                foreach (var stationBucket in stationBuckets)
                {
                    var station = stationBucket.Key;
                    var cityBuckets = stationBucket.Terms("municipio").Buckets;
                    foreach (var cityBucket in cityBuckets)
                    {
                        var city = cityBucket.Key;
                        var monthBuckets = cityBucket.Terms("mes").Buckets;
                        foreach (var monthBucket in monthBuckets)
                        {
                            var month = monthBucket.Key;
                            var yearBuckets = monthBucket.Terms("ano").Buckets;
                            foreach (var yearBucket in yearBuckets)
                            {
                                var responseYear = yearBucket.Key;
                                var distanceBuckets = yearBucket.Terms("distancia").Buckets;
                                foreach (var distanceBucket in distanceBuckets)
                                {
                                    var responseDistance = distanceBucket.Key;
                                    var average = distanceBucket.Average("media").Value;
                                    filteredResponse.Add(new
                                    {
                                        codigoEstacao = stationCode,
                                        estacao = station,
                                        municipio = city,
                                        mes = month,
                                        ano = responseYear,
                                        distancia = responseDistance,
                                        media = average
                                    });
                                }
                            }

                        }

                        
                    }
                }
            }

            return filteredResponse;
        }

        public async Task<List<MeasuredRainfall>> GetAll()
        {
            var response = await _elasticClient.SearchAsync<MeasuredRainfall>(s => s.Source(true));
            return response.Documents.ToList();
        }

        public async Task<List<object>> FilterByDistance(double distance)
        {
            var response = await _elasticClient.SearchAsync<MeasuredRainfall>(s => s
                .Source(true)
                .ScriptFields(s => s.ScriptField("distancia", script => script
                    .Source(_distanceCalculationString)
                ))
                .Query(q => q
                    .Bool(b => b
                        .Filter(f => f
                            .Script(s => s
                                .Script(s => s
                                    .Source($"double distancia = {_distanceCalculationString}; return distancia < {distance};"))))))
            );
            var filteredResponse = new List<object>();

            foreach (var hit in response?.Hits)
            {
                filteredResponse.Add(new
                {
                    source = hit.Source,
                    distancia = hit.Fields.Value<double>("distancia")
                });
            }

            return filteredResponse;
        }

        public async Task<List<object>> GetAllWithDistance()
        {
            var response = await _elasticClient.SearchAsync<MeasuredRainfall>(s => s
                .Source(true)
                .ScriptFields(s => s
                    .ScriptField("distancia", script => script
                        .Source(_distanceCalculationString)
                    )
                )
            );

            var filteredResponse = new List<object>();

            foreach (var hit in response?.Hits)
            {
                filteredResponse.Add(new
                {
                    source = hit.Source,
                    distancia = hit.Fields.Value<double>("distancia")
                });
            }

            return filteredResponse;
        }
    }
}
