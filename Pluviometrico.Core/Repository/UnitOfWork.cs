using Nest;
using Pluviometrico.Core.Repository.Interface;
using Pluviometrico.Data.DatabaseContext;

namespace Pluviometrico.Core.Repository
{
    public class UnitOfWork : IUnitOfWork
    {
        private IElasticClient _elasticClient;
        private PostgreSQLContext _context;
        private IMeasuredRainfallRepository _measuredRainfallRepository;
        //public IMeasuredRainfallRepository MeasuredRainfallList => _measuredRainfallRepository ??= new MeasuredRainfallRepository(_elasticClient);
        public IMeasuredRainfallRepository MeasuredRainfallList => _measuredRainfallRepository ??= new MeasuredRainfallRepositoryPostgreSQL(_context);

        public UnitOfWork(IElasticClient elasticClient, PostgreSQLContext context)
        {
            _elasticClient = elasticClient;
            _context = context;
        }
    }
}
