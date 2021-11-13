using Nest;
using Pluviometrico.Core.Repository.Interface;

namespace Pluviometrico.Core.Repository
{
    public class UnitOfWork : IUnitOfWork
    {
        private IElasticClient _elasticClient;
        private IMeasuredRainfallRepository _measuredRainfallRepository;
        public IMeasuredRainfallRepository MeasuredRainfallList => _measuredRainfallRepository ??= new MeasuredRainfallRepository(_elasticClient);

        public UnitOfWork(IElasticClient elasticClient)
        {
            _elasticClient = elasticClient;
        }
    }
}
