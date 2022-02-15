using Microsoft.Extensions.Configuration;
using Nest;
using Pluviometrico.Core.DTOs;
using Pluviometrico.Core.Repository.Interface;
using Pluviometrico.Core.Repository.ODBC;
using Pluviometrico.Data.DatabaseContext.Database;
using System;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace Pluviometrico.Core.Repository
{
    public class UnitOfWork : IUnitOfWork
    {
        private IElasticClient _elasticClient;
        private PostgreSQLDWContext _postgreSQLDWContext;

        private MachineLearningRepository _machineLearningRepository;
        private IMeasuredRainfallRepository _measuredRainfallElastic;
        private IMeasuredRainfallRepository _measuredRainfallPostgreSqlDW;
        private IMeasuredRainfallRepository _measuredRainfallMonetDb;
        private IMeasuredRainfallRepository _measuredRainfallDrill;
        private IMeasuredRainfallRepository _measuredRainfallSpark;

        public UnitOfWork(IElasticClient elasticClient, PostgreSQLDWContext postgreSQLDWContext, IConfiguration configuration)
        {
            _elasticClient = elasticClient;
            _postgreSQLDWContext = postgreSQLDWContext;
            _machineLearningRepository = new MachineLearningRepository(_postgreSQLDWContext);
            _measuredRainfallElastic = new MeasuredRainfallRepositoryES(_elasticClient);
            _measuredRainfallPostgreSqlDW = new MeasuredRainfallRepositoryDW(_postgreSQLDWContext);
            _measuredRainfallMonetDb = new MeasuredRainfallRepositoryODBC(new MonetDBProperties(configuration));
            _measuredRainfallDrill = new MeasuredRainfallRepositoryODBC(new DrillProperties(configuration));
            _measuredRainfallSpark = new MeasuredRainfallRepositorySpark();
        }

        private DatabaseType GetBestPerformingDatabase(int queryNumber)
        {
            return _machineLearningRepository.GetBestPerformingDatabase(queryNumber);
        }

        //APAGAR
        private DatabaseType GetDatabase()
        {
            return DatabaseType.ApacheSpark;
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
            else if (database == DatabaseType.MonetDB)
            {
                measuredRainfall = _measuredRainfallMonetDb;
            }
            else if (database == DatabaseType.ApacheDrill)
            {
                measuredRainfall = _measuredRainfallDrill;
            }
            else if (database == DatabaseType.ApacheSpark)
            {
                measuredRainfall = _measuredRainfallSpark;
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
