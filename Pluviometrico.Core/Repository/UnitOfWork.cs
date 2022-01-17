using Nest;
using Pluviometrico.Core.DTOs;
using Pluviometrico.Core.Repository.Interface;
using Pluviometrico.Data.DatabaseContext;
using Pluviometrico.Data.DatabaseContext.Database;
using System;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace Pluviometrico.Core.Repository
{
    public class UnitOfWork : IUnitOfWork
    {
        private IElasticClient _elasticClient;
        private PostgreSQLContext _context;
        private PostgreSQLDWContext _postgreSQLDWContext;

        private MachineLearningRepository _machineLearningRepository;
        private IMeasuredRainfallRepository _measuredRainfallElastic;
        private IMeasuredRainfallRepository _measuredRainfallPostgreSql;
        private IMeasuredRainfallRepository _measuredRainfallPostgreSqlDW;

        public UnitOfWork(IElasticClient elasticClient, PostgreSQLContext context, PostgreSQLDWContext postgreSQLDWContext)
        {
            _elasticClient = elasticClient;
            _context = context;
            _postgreSQLDWContext = postgreSQLDWContext;
            _machineLearningRepository = new MachineLearningRepository(_postgreSQLDWContext);
            _measuredRainfallElastic = new MeasuredRainfallRepositoryES(_elasticClient);
            _measuredRainfallPostgreSql = new MeasuredRainfallRepositoryPostgreSQL(_context);
            _measuredRainfallPostgreSqlDW = new MeasuredRainfallRepositoryDW(_postgreSQLDWContext);
        }

        private DatabaseType GetBestPerformingDatabase(int queryNumber)
        {
            return _machineLearningRepository.GetBestPerformingDatabase(queryNumber);
        }

        //APAGAR
        private DatabaseType GetDatabase()
        {
            return DatabaseType.PostgreSQL;
        }

        private IMeasuredRainfallRepository GetDatabaseRepository(DatabaseType database)
        {
            IMeasuredRainfallRepository measuredRainfall;

            if (database == DatabaseType.ElasticSearch)
            {
                measuredRainfall = _measuredRainfallElastic;
            }
            else if (database == DatabaseType.PostgreSQL)
            {
                measuredRainfall = _measuredRainfallPostgreSqlDW;
            }
            else
            {
                //retornar erro
                return null;
            }
            return measuredRainfall;
        }

        public Task<List<MeasuredRainfallDTO>> FilterByYear(int year)
        {
            var database = GetBestPerformingDatabase(1);
            var measureRainfallRepository = GetDatabaseRepository(database);

            return measureRainfallRepository.FilterByYear(year);
        }


        public Task<List<MeasuredRainfallDTO>> FilterByRainfallIndex(double index)
        {
            var database = GetDatabase();
            var measureRainfallRepository = GetDatabaseRepository(database);

            return measureRainfallRepository.FilterByRainfallIndex(index);
        }

        public Task<List<MeasuredRainfallDTO>> FilterByDistance(double distance)
        {
            var database = GetDatabase();
            var measureRainfallRepository = GetDatabaseRepository(database);

            return measureRainfallRepository.FilterByDistance(distance);
        }

        public Task<List<MeasuredRainfallDTO>> FilterByDistanceAndRainfallIndex(double distance, double index)
        {
            var database = GetDatabase();
            var measureRainfallRepository = GetDatabaseRepository(database);

            return measureRainfallRepository.FilterByDistanceAndRainfallIndex(distance, index);
        }

        public Task<List<MeasuredRainfallDTO>> FilterByDistanceAndDate(double distance, int year, int month, int day)
        {
            var database = GetDatabase();
            var measureRainfallRepository = GetDatabaseRepository(database);

            return measureRainfallRepository.FilterByDistanceAndDate(distance, year, month, day);
        }

        public Task<List<MeasuredRainfallDTO>> FilterByDistanceAndDateRange(DateTime firstDate, DateTime secondDate, double distance)
        {
            var database = GetDatabase();
            var measureRainfallRepository = GetDatabaseRepository(database);

            return measureRainfallRepository.FilterByDistanceAndDateRange(firstDate, secondDate, distance);
        }

        public Task<List<MeasuredRainfallDTO>> FilterByDistanceAndCity(double distance, string city, int limit)
        {
            var database = GetDatabase();
            var measureRainfallRepository = GetDatabaseRepository(database);

            return measureRainfallRepository.FilterByDistanceAndCity(distance, city, limit);
        }

        public Task<List<MeasuredRainfallDTO>> GetAverageRainfallIndexByCity(string city, int limit)
        {
            var database = GetDatabase();
            var measureRainfallRepository = GetDatabaseRepository(database);

            return measureRainfallRepository.GetAverageRainfallIndexByCity(city, limit);
        }

        public Task<List<MeasuredRainfallDTO>> FilterByGeolocationAndCity(string city, double minLatitude, double maxLatitude, double minLongitude, double maxLongitude)
        {
            var database = GetDatabase();
            var measureRainfallRepository = GetDatabaseRepository(database);

            return measureRainfallRepository.FilterByGeolocationAndCity(city, minLatitude, maxLatitude, minLongitude, maxLongitude);
        }

        public Task<List<MeasuredRainfallDTO>> FilterByGeolocationAndDateRange(DateTime firstDate, DateTime secondDate, double minLatitude, double maxLatitude, double minLongitude, double maxLongitude)
        {
            var database = GetDatabase();
            var measureRainfallRepository = GetDatabaseRepository(database);

            return measureRainfallRepository.FilterByGeolocationAndDateRange(firstDate, secondDate, minLatitude, maxLatitude, minLongitude, maxLongitude);
        }

        public Task<List<MeasuredRainfallDTO>> FilterByGeolocationAndRainfallIndex(double index, double minLatitude, double maxLatitude, double minLongitude, double maxLongitude)
        {
            var database = GetDatabase();
            var measureRainfallRepository = GetDatabaseRepository(database);

            return measureRainfallRepository.FilterByGeolocationAndRainfallIndex(index, minLatitude, maxLatitude, minLongitude, maxLongitude);
        }
    }
}
