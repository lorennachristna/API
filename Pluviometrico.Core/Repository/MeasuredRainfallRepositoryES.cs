using Nest;
using Pluviometrico.Core.DTOs;
using Pluviometrico.Core.Repository.Interface;
using Pluviometrico.Data;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace Pluviometrico.Core.Repository
{
    public class MeasuredRainfallRepositoryES : IMeasuredRainfallRepository
    {
        private readonly IElasticClient _elasticClient;
        private readonly string _distanceCalculationString = "6371 * Math.acos(Math.cos(-22.913924*Math.PI/180) * Math.cos(doc['latitude'].value*Math.PI/180) * Math.cos(-43.084737*Math.PI/180 - (doc['longitude'].value*Math.PI/180)) + Math.sin(-22.913924*Math.PI/180) * Math.sin(doc['latitude'].value*Math.PI/180))";

        public MeasuredRainfallRepositoryES(IElasticClient elasticClient)
        {
            _elasticClient = elasticClient;
        }
        public async Task<List<MeasuredRainfallDTO>> FilterByYear(int year)
        {
            var response = await _elasticClient.SearchAsync<MeasuredRainfall>(s =>
                s.Query(q =>
                    q.Bool(b => 
                        b.Must(m =>
                            m.Term(t => t.Field(f => f.Ano).Value(year))
                        )
                    )
                )
            );

            return response?.Documents?.Select(d => new MeasuredRainfallDTO
            {
                Source = "CEMADEN",
                City = d.Municipio,
                UF = d.UF,
                Day = d.Dia,
                Month = d.Mes,
                Year = d.Ano,
                Hour = d.Hora,
                StationCode = d.CodEstacaoOriginal,
                StationName = d.NomeEstacaoOriginal,
                RainfallIndex = d.ValorMedida
            }).ToList();
        }

        public async Task<List<MeasuredRainfallDTO>> FilterByRainfallIndex(double index)
        {
            var response = await _elasticClient.SearchAsync<MeasuredRainfall>(s =>
                s.Query(q =>
                    q.Bool(b =>
                        b.Must(m =>
                            m.Range(r => r.Field(f => f.ValorMedida).GreaterThan(index))
                        )
                    )
                )
            );

            return response?.Documents?.Select(d => new MeasuredRainfallDTO
            {
                Source = "CEMADEN",
                City = d.Municipio,
                UF = d.UF,
                Day = d.Dia,
                Month = d.Mes,
                Year = d.Ano,
                Hour = d.Hora,
                StationCode = d.CodEstacaoOriginal,
                StationName = d.NomeEstacaoOriginal,
                RainfallIndex = d.ValorMedida
            }).ToList();
        }

        public async Task<List<MeasuredRainfallDTO>> FilterByDistance(double distance)
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
            var filteredResponse = new List<MeasuredRainfallDTO> ();

            foreach (var hit in response?.Hits)
            {
                filteredResponse.Add(new MeasuredRainfallDTO
                {
                    Source = "CEMADEN",
                    City = hit.Source.Municipio,
                    UF = hit.Source.UF,
                    Day = hit.Source.Dia,
                    Month = hit.Source.Mes,
                    Year = hit.Source.Ano,
                    Hour = hit.Source.Hora,
                    StationCode = hit.Source.CodEstacaoOriginal,
                    StationName = hit.Source.NomeEstacaoOriginal,
                    RainfallIndex = hit.Source.ValorMedida,
                    Distance = hit.Fields.Value<double>("distancia")
                });
            }

            return filteredResponse;
        }

        public async Task<List<MeasuredRainfallDTO>> FilterByDistanceAndRainfallIndex(double distance, double index)
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
                                    .Source($"double distancia = {_distanceCalculationString}; return distancia < {distance};"))))
                        .Must(m => m.Range(r => r.Field(f => f.ValorMedida).GreaterThan(index)))
                        ))
            );
            var filteredResponse = new List<MeasuredRainfallDTO>();

            foreach (var hit in response?.Hits)
            {
                filteredResponse.Add(new MeasuredRainfallDTO
                {
                    Source = "CEMADEN",
                    City = hit.Source.Municipio,
                    UF = hit.Source.UF,
                    Day = hit.Source.Dia,
                    Month = hit.Source.Mes,
                    Year = hit.Source.Ano,
                    Hour = hit.Source.Hora,
                    StationCode = hit.Source.CodEstacaoOriginal,
                    StationName = hit.Source.NomeEstacaoOriginal,
                    RainfallIndex = hit.Source.ValorMedida,
                    Distance = hit.Fields.Value<double>("distancia")
                });
            }

            return filteredResponse;
        }

        public async Task<List<MeasuredRainfallDTO>> FilterByDistanceAndDate(double distance, int year, int month, int day)
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
                                    .Source($"double distancia = {_distanceCalculationString}; return distancia < {distance};"))))
                        .Must(m => 
                            m.Term(t => t.Field(f => f.Ano).Value(year)) &&
                            m.Term(t => t.Field(f => f.Mes).Value(month)) &&
                            m.Term(t => t.Field(f => f.Dia).Value(day))
                        )))
            );
            var filteredResponse = new List<MeasuredRainfallDTO>();

            foreach (var hit in response?.Hits)
            {
                filteredResponse.Add(new MeasuredRainfallDTO
                {
                    Source = "CEMADEN",
                    City = hit.Source.Municipio,
                    UF = hit.Source.UF,
                    Day = hit.Source.Dia,
                    Month = hit.Source.Mes,
                    Year = hit.Source.Ano,
                    Hour = hit.Source.Hora,
                    StationCode = hit.Source.CodEstacaoOriginal,
                    StationName = hit.Source.NomeEstacaoOriginal,
                    RainfallIndex = hit.Source.ValorMedida,
                    Distance = hit.Fields.Value<double>("distancia")
                });
            }

            return filteredResponse;
        }

        public async Task<List<MeasuredRainfallDTO>> FilterByDistanceAndDateRange(DateTime firstDate, DateTime secondDate, double distance)
        {
            var dates = Utils.MaxMinDate(firstDate, secondDate);

            var response = await _elasticClient.SearchAsync<MeasuredRainfall>(s => s
                .Source(true)
                .ScriptFields(sf => sf
                    .ScriptField("distancia", script => script
                        .Source(_distanceCalculationString)))
                .Query(q => q.Bool(b => b
                    .Filter(f => f.Script(s => s.Script(s => s.Source($"double distancia = {_distanceCalculationString} ; return distancia < {distance};"))))
                    .Must(m => m.DateRange(r => r.Field(f => f.DataHora).GreaterThanOrEquals(DateMath.Anchored(dates.lesserDate)).LessThanOrEquals(DateMath.Anchored(dates.greaterDate)))))
            ));

            var filteredResponse = new List<MeasuredRainfallDTO>();

            foreach (var hit in response?.Hits)
            {
                filteredResponse.Add(new MeasuredRainfallDTO
                {
                    Source = "CEMADEN",
                    City = hit.Source.Municipio,
                    UF = hit.Source.UF,
                    Day = hit.Source.Dia,
                    Month = hit.Source.Mes,
                    Year = hit.Source.Ano,
                    Hour = hit.Source.Hora,
                    StationCode = hit.Source.CodEstacaoOriginal,
                    StationName = hit.Source.NomeEstacaoOriginal,
                    RainfallIndex = hit.Source.ValorMedida,
                    Distance = hit.Fields.Value<double>("distancia")
                });
            }
            return filteredResponse;
        }

        public async Task<List<MeasuredRainfallDTO>> FilterByDistanceAndCity(double distance, string city, int limit)
        {
            var response = await _elasticClient.SearchAsync<MeasuredRainfall>(s => s
                .Source(true)
                .ScriptFields(sf =>
                    sf.ScriptField("distancia", script => script
                        .Source(_distanceCalculationString)))
                .Query(q =>
                    q.Bool(b => b
                        .Filter(f => f.Script(s => s.Script(s => s.Source($"double distancia = {_distanceCalculationString} ; return distancia > {distance};"))))
                        .Must(m => m.Match(t => t.Field(f => f.Municipio).Query(city))))));

            var filteredResponse = new HashSet<MeasuredRainfallDTO>();

            foreach (var hit in response?.Hits)
            {
                filteredResponse.Add(new MeasuredRainfallDTO {
                    Source = "CEMADEN",
                    City = hit.Source.Municipio,
                    UF = hit.Source.UF,
                    StationCode = hit.Source.CodEstacaoOriginal,
                    StationName = hit.Source.NomeEstacaoOriginal,
                    RainfallIndex = hit.Source.ValorMedida,
                    Distance = hit.Fields.Value<double>("distancia")
                });
            }
            return filteredResponse.Take(limit).ToList();
        }

        public async Task<List<MeasuredRainfallDTO>> GetAverageRainfallIndexByCity(string city, int limit)
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
                                            .Terms("UF", t => t
                                                .Field(f => f.UF.Suffix("keyword"))
                                                .Aggregations(a => a
                                                    .Terms("distancia", t => t
                                                        .Field("distancia")
                                                        .Aggregations(a => a
                                                            .Average("media", s => s
                                                                .Field(f => f.ValorMedida)))))))))))))
                .Query(q => q.Bool(b => b.Must(m =>
                    m.Match(t => t
                        .Field(f => f.Municipio).Query(city))
                )))
            );

            var filteredResponse = new List<MeasuredRainfallDTO>();

            var stationCodeBuckets = response.Aggregations.Terms("codigoEstacao").Buckets;
            foreach (var stationCodeBucket in stationCodeBuckets)
            {
                var stationCode = stationCodeBucket.Key;
                var stationBuckets = stationCodeBucket.Terms("estacao").Buckets;
                foreach (var stationBucket in stationBuckets)
                {
                    var station = stationBucket.Key;
                    var cityBuckets = stationBucket.Terms("municipio").Buckets;
                    foreach (var cityBucket in cityBuckets)
                    {
                        var responseCity = cityBucket.Key;
                        var UFBuckets = cityBucket.Terms("UF").Buckets;
                        foreach (var UFBucket in UFBuckets)
                        {
                            var uF = UFBucket.Key;
                            var distanceBuckets = UFBucket.Terms("distancia").Buckets;
                            foreach (var distanceBucket in distanceBuckets)
                            {
                                var responseDistance = double.Parse(distanceBucket.Key);
                                var average = distanceBucket.Average("media").Value;
                                filteredResponse.Add(new MeasuredRainfallDTO
                                {
                                    Source = "CEMADEN",
                                    City = responseCity,
                                    UF = uF,
                                    StationCode = stationCode,
                                    StationName = station,
                                    Distance = responseDistance,
                                    AverageRainfallIndex = average
                                });
                            }
                        }
                    }
                }
            }

            return filteredResponse.Take(limit).Distinct().ToList();
        }

        public async Task<List<MeasuredRainfallDTO>> FilterByGeolocationAndCity(string city,
            double minLatitude, double maxLatitude,
            double minLongitude, double maxLongitude)
        {
            var response = await _elasticClient.SearchAsync<MeasuredRainfall>(s => s
                .Source(true)
                .ScriptFields(sf =>
                    sf.ScriptField("distancia", script => script
                        .Source(_distanceCalculationString)))
                .Query(q =>
                    q.Bool(b => 
                        b.Must(m =>
                            m.Match(m => m.Field(f => f.Municipio).Query(city)) &&
                            m.Range(t => t.Field(f => f.Latitude).GreaterThan(minLatitude)) &&
                            m.Range(t => t.Field(f => f.Latitude).LessThan(maxLatitude)) &&
                            m.Range(t => t.Field(f => f.Longitude).GreaterThan(minLongitude)) &&
                            m.Range(t => t.Field(f => f.Longitude).LessThan(maxLongitude))))));

            var filteredResponse = new List<MeasuredRainfallDTO>();

            foreach (var hit in response?.Hits)
            {
                filteredResponse.Add(new MeasuredRainfallDTO
                {
                    Source = "CEMADEN",
                    City = hit.Source.Municipio,
                    UF = hit.Source.UF,
                    Year = hit.Source.Ano,
                    Month = hit.Source.Mes,
                    Day = hit.Source.Dia,
                    Hour = hit.Source.Hora,
                    StationCode = hit.Source.CodEstacaoOriginal,
                    StationName = hit.Source.NomeEstacaoOriginal,
                    RainfallIndex = hit.Source.ValorMedida,
                    Distance = hit.Fields.Value<double>("distancia")
                });
            }
            return filteredResponse;
        }

        public async Task<List<MeasuredRainfallDTO>> FilterByGeolocationAndDateRange(DateTime firstDate, DateTime secondDate,
            double minLatitude, double maxLatitude,
            double minLongitude, double maxLongitude)
        {

            var dates = Utils.MaxMinDate(firstDate, secondDate);

            var response = await _elasticClient.SearchAsync<MeasuredRainfall>(s => s
                .Source(true)
                .ScriptFields(sf =>
                    sf.ScriptField("distancia", script => script
                        .Source(_distanceCalculationString)))
                .Query(q =>
                    q.Bool(b =>
                        b.Must(m =>
                            m.DateRange(r => r.Field(f => f.DataHora).GreaterThanOrEquals(dates.lesserDate).LessThanOrEquals(dates.greaterDate)) &&
                            m.Range(t => t.Field(f => f.Latitude).GreaterThan(minLatitude)) &&
                            m.Range(t => t.Field(f => f.Latitude).LessThan(maxLatitude)) &&
                            m.Range(t => t.Field(f => f.Longitude).GreaterThan(minLongitude)) &&
                            m.Range(t => t.Field(f => f.Longitude).LessThan(maxLongitude))))));

            var filteredResponse = new List<MeasuredRainfallDTO>();

            foreach (var hit in response?.Hits)
            {
                filteredResponse.Add(new MeasuredRainfallDTO
                {
                    Source = "CEMADEN",
                    City = hit.Source.Municipio,
                    UF = hit.Source.UF,
                    Year = hit.Source.Ano,
                    Month = hit.Source.Mes,
                    Day = hit.Source.Dia,
                    Hour = hit.Source.Hora,
                    StationCode = hit.Source.CodEstacaoOriginal,
                    StationName = hit.Source.NomeEstacaoOriginal,
                    RainfallIndex = hit.Source.ValorMedida,
                    Distance = hit.Fields.Value<double>("distancia")
                });
            }
            return filteredResponse;
        }

        public async Task<List<MeasuredRainfallDTO>> FilterByGeolocationAndRainfallIndex(double index,
            double minLatitude, double maxLatitude,
            double minLongitude, double maxLongitude)
        {
            var response = await _elasticClient.SearchAsync<MeasuredRainfall>(s => s
                .Source(true)
                .ScriptFields(sf =>
                    sf.ScriptField("distancia", script => script
                        .Source(_distanceCalculationString)))
                .Query(q =>
                    q.Bool(b =>
                        b.Must(m =>
                            m.Range(r => r.Field(f => f.ValorMedida).GreaterThanOrEquals(index)) &&
                            m.Range(t => t.Field(f => f.Latitude).GreaterThan(minLatitude)) &&
                            m.Range(t => t.Field(f => f.Latitude).LessThan(maxLatitude)) &&
                            m.Range(t => t.Field(f => f.Longitude).GreaterThan(minLongitude)) &&
                            m.Range(t => t.Field(f => f.Longitude).LessThan(maxLongitude))))));

            var filteredResponse = new List<MeasuredRainfallDTO>();

            foreach (var hit in response?.Hits)
            {
                filteredResponse.Add(new MeasuredRainfallDTO
                {
                    Source = "CEMADEN",
                    City = hit.Source.Municipio,
                    UF = hit.Source.UF,
                    Year = hit.Source.Ano,
                    Month = hit.Source.Mes,
                    Day = hit.Source.Dia,
                    Hour = hit.Source.Hora,
                    StationCode = hit.Source.CodEstacaoOriginal,
                    StationName = hit.Source.NomeEstacaoOriginal,
                    RainfallIndex = hit.Source.ValorMedida,
                    Distance = hit.Fields.Value<double>("distancia")
                });
            }
            return filteredResponse;
        }
    }
}
