using Nest;
using Pluviometrico.Data;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace Pluviometrico.Core.Repository.Interface
{
    public interface IMeasuredRainfallRepository
    {
        Task<List<MeasuredRainfall>> GetListByMonthAndYear(int month, int year);
        Task<List<ElasticSearchHit>> GetByDistance(int greaterThanYear, int lessThanYear, double distance);
    }
}
