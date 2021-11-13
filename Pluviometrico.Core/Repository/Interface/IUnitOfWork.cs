using System;

namespace Pluviometrico.Core.Repository.Interface
{
    public interface IUnitOfWork
    {
        IMeasuredRainfallRepository MeasuredRainfallList { get; }
    }
}
