using Nest;
using Pluviometrico.Core.Repository.Interface;
using Pluviometrico.Data;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace Pluviometrico.Core.Repository
{
    public class MeasuredRainfallRepository : IMeasuredRainfallRepository
    {
        private readonly IElasticClient _elasticClient;

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
                            m.Match(m => m.Field(f => f.Mes == month)) &&
                            m.Match(m => m.Field(f => f.Ano == year))
                        )
                    )
                )
            );

            return response?.Documents?.ToList();
        }

        //TODO: Check if adding "distancia" field significantly slows response time"?
        public async Task<List<ElasticSearchHit>> GetByDistance(int greaterThanYear, int lessThanYear, double distance)
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
    }
}
