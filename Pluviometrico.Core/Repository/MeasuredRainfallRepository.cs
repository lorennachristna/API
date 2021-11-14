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
    }
}
