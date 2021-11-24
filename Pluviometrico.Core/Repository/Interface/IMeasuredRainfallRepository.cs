using Nest;
using Pluviometrico.Data;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace Pluviometrico.Core.Repository.Interface
{
    public interface IMeasuredRainfallRepository
    {
        Task<List<MeasuredRainfall>> GetListByMonthAndYear(int month, int year);
        Task<List<object>> GetByDistanceAndYearRange(int greaterThanYear, int lessThanYear, double distance);
        Task<List<object>> GetByDistanceAndYear(int year, double distance);
        Task<List<object>> GetValueAggregationsByDate(int year);
        Task<List<object>> GetValueAggregationsByDistance(int year, double distance);
        Task<List<object>> GetValueAggregationsByDistanceGroupByStation(int year, double distance);
        Task<List<MeasuredRainfall>> GetAll();
        Task<List<object>> FilterByDistance(double distance);
        Task<List<object>> GetAllWithDistance();
        Task<List<object>> GetValueByCityAndYear();
        Task<List<object>> GetValueByStationAndDistance(double distance);
        Task<List<object>> GetValueByDistance(double distance);
    }
}
