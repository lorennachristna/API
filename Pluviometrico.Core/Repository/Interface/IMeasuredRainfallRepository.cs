using Pluviometrico.Data;
using System.Threading.Tasks;

namespace Pluviometrico.Core.Repository.Interface
{
    public interface IMeasuredRainfallRepository
    {
        Task<MeasuredRainfall> Get(int id);
    }
}
