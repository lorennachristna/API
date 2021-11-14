using Pluviometrico.Data;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace Pluviometrico.Core.Repository.Interface
{
    public interface IMeasuredRainfallRepository
    {
        Task<List<MeasuredRainfall>> GetListByMonthAndYear(int month, int year);
    }
}
