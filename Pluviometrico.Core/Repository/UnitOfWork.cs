using Nest;
using Pluviometrico.Core.Repository.Interface;
using Pluviometrico.Data.DatabaseContext;

namespace Pluviometrico.Core.Repository
{
    public class UnitOfWork : IUnitOfWork
    {
        private IElasticClient _elasticClient;
        private PostgreSQLContext _context;
        private DWContext _dwContext;
        private IMeasuredRainfallRepository _measuredRainfallRepository;
        //public IMeasuredRainfallRepository MeasuredRainfallList => _measuredRainfallRepository ??= new MeasuredRainfallRepository(_elasticClient);
        //public IMeasuredRainfallRepository MeasuredRainfallList => _measuredRainfallRepository ??= new MeasuredRainfallRepositoryPostgreSQL(_context);
        public IMeasuredRainfallRepository MeasuredRainfallList => _measuredRainfallRepository ??= new MeasuredRainfallRepositoryDW(_dwContext);

        public UnitOfWork(IElasticClient elasticClient, PostgreSQLContext context, DWContext dWContext)
        {
            _elasticClient = elasticClient;
            _context = context;
            _dwContext = dWContext;
        }
    }
}
