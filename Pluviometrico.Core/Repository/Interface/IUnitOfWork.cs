using Pluviometrico.Core.DTOs;
using System;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace Pluviometrico.Core.Repository.Interface
{
    public interface IUnitOfWork
    {
        Task<List<MeasuredRainfallDTO>> FilterByYear(int year);

        Task<List<MeasuredRainfallDTO>> FilterByRainfallIndex(double index);

        Task<List<MeasuredRainfallDTO>> FilterByDistance(double distance);

        Task<List<MeasuredRainfallDTO>> FilterByDistanceAndRainfallIndex(double distance, double index);

        Task<List<MeasuredRainfallDTO>> FilterByDistanceAndDate(double distance, int year, int month, int day);

        Task<List<MeasuredRainfallDTO>> FilterByDistanceAndDateRange(DateTime firstDate, DateTime secondDate, double distance);

        Task<List<MeasuredRainfallDTO>> FilterByDistanceAndCity(double distance, string city, int limit);

        Task<List<MeasuredRainfallDTO>> GetAverageRainfallIndexByCity(string city, int limit);

        Task<List<MeasuredRainfallDTO>> FilterByGeolocationAndCity(string city,
            double minLatitude, double maxLatitude,
            double minLongitude, double maxLongitude);

        Task<List<MeasuredRainfallDTO>> FilterByGeolocationAndDateRange(DateTime firstDate, DateTime secondDate,
            double minLatitude, double maxLatitude,
            double minLongitude, double maxLongitude);

        Task<List<MeasuredRainfallDTO>> FilterByGeolocationAndRainfallIndex(double index,
            double minLatitude, double maxLatitude,
            double minLongitude, double maxLongitude);
    }
}
