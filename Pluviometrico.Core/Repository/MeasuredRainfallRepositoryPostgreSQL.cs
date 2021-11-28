using Microsoft.EntityFrameworkCore;
using Pluviometrico.Core.Repository.Interface;
using Pluviometrico.Data;
using Pluviometrico.Data.DatabaseContext;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace Pluviometrico.Core.Repository
{
    public class MeasuredRainfallRepositoryPostgreSQL : IMeasuredRainfallRepository
    {
        private readonly PostgreSQLContext _context;

        public MeasuredRainfallRepositoryPostgreSQL(PostgreSQLContext context)
        {
            _context = context;
        }

        public Task<List<object>> FilterByDistance(double distance)
        {
            throw new System.NotImplementedException();
        }

        public Task<List<MeasuredRainfall>> GetAll()
        {
            return _context.MeasuredRainfallList.Take(10).ToListAsync();
        }

        public Task<List<object>> GetAllWithDistance()
        {
            throw new System.NotImplementedException();
        }

        public Task<List<object>> FilterByDistanceAndYear(int year, double distance)
        {
            throw new System.NotImplementedException();
        }

        public Task<List<object>> FilterByDistanceAndYearRange(int greaterThanYear, int lessThanYear, double distance)
        {
            throw new System.NotImplementedException();
        }

        public Task<List<MeasuredRainfall>> FilterByMonthAndYear(int month, int year)
        {
            throw new System.NotImplementedException();
        }

        public Task<List<object>> GetMeasureByCityFilterByDate(int year)
        {
            throw new System.NotImplementedException();
        }

        public Task<List<object>> GetMeasureByCityFilterByYearAndDistance(int year, double distance)
        {
            throw new System.NotImplementedException();
        }

        public Task<List<object>> GetAverageMeasureByCityAndStationFilterByDateAndDistance(int year, double distance)
        {
            throw new System.NotImplementedException();
        }

        public Task<List<object>> GetMeasureByCityAndYear()
        {
            throw new System.NotImplementedException();
        }

        public Task<List<object>> GetMeasureByCityAndDateFilterByDistance(double distance)
        {
            throw new System.NotImplementedException();
        }

        public Task<List<object>> GetMeasureByCityAndYearFilterByDistance(double distance)
        {
            throw new System.NotImplementedException();
        }
    }
}
