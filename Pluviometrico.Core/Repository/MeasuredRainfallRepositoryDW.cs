﻿using Microsoft.EntityFrameworkCore;
using Pluviometrico.Core.DTOs;
using Pluviometrico.Core.Repository.Interface;
using Pluviometrico.Data;
using Pluviometrico.Data.DatabaseContext;
using Pluviometrico.Data.DWModels;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace Pluviometrico.Core.Repository
{
    public class MeasuredRainfallRepositoryDW : IMeasuredRainfallRepository
    {
        private readonly DWContext _context;

        public MeasuredRainfallRepositoryDW(DWContext context)
        {
            _context = context;
        }

        public async Task<List<MeasuredRainfallDTO>> FilterByYear(int year)
        {
            var response = await _context.FactRainList
                .Include(f => f.Time)
                .Include(f => f.Location)
                .Include(f => f.Source)
                .Include(f => f.Station)
                .Where(f => f.Time.Year == year)
                .Select(f => 
                    new MeasuredRainfallDTO 
                    {
                        Source = f.Source.Source,
                        City = f.Location.City,
                        UF = f.Location.UF,
                        Day = f.Time.Day,
                        Month = f.Time.Month,
                        Year = f.Time.Year,
                        Hour = f.Time.Hour,
                        StationCode = f.Station.StationCode,
                        StationName = f.Station.StationName,
                        RainfallIndex = f.RainfallIndex
                    }).ToListAsync();

            return response.Take(10).ToList();
        }

        public async Task<List<MeasuredRainfallDTO>> FilterByRainfallIndex(double index)
        {
            var response = await _context.FactRainList
                .Include(f => f.Time)
                .Include(f => f.Location)
                .Include(f => f.Source)
                .Include(f => f.Station)
                .Where(f => f.RainfallIndex > index)
                .Select(f =>
                    new MeasuredRainfallDTO
                    {
                        Source = f.Source.Source,
                        City = f.Location.City,
                        UF = f.Location.UF,
                        Day = f.Time.Day,
                        Month = f.Time.Month,
                        Year = f.Time.Year,
                        Hour = f.Time.Hour,
                        StationCode = f.Station.StationCode,
                        StationName = f.Station.StationName,
                        RainfallIndex = f.RainfallIndex
                    }).ToListAsync();

            return response.Take(10).ToList();
        }

        public async Task<List<MeasuredRainfallDTO>> FilterByDistance(double distance)
        {
            var response = await _context.FactRainList
                .Include(f => f.Time)
                .Include(f => f.Location)
                .Include(f => f.Source)
                .Include(f => f.Station)
                .Select(f =>
                    new MeasuredRainfallDTO
                    {
                        Source = f.Source.Source,
                        City = f.Location.City,
                        UF = f.Location.UF,
                        Day = f.Time.Day,
                        Month = f.Time.Month,
                        Year = f.Time.Year,
                        Hour = f.Time.Hour,
                        StationCode = f.Station.StationCode,
                        StationName = f.Station.StationName,
                        RainfallIndex = f.RainfallIndex,
                        Distance = 6371 *
                                Math.Acos(
                                    Math.Cos((Math.PI / 180) * (-22.913924)) * Math.Cos((Math.PI / 180) * (f.Location.Latitude)) *
                                    Math.Cos((Math.PI / 180) * (-43.084737) - (Math.PI / 180) * (f.Location.Longitude)) +
                                    Math.Sin((Math.PI / 180) * (-22.913924)) *
                                    Math.Sin((Math.PI / 180) * (f.Location.Latitude))
                                )
                    })
                .Where(s => s.Distance < distance).ToListAsync();

            return response.Take(10).ToList();

        }

        public async Task<List<MeasuredRainfallDTO>> FilterByDistanceAndRainfallIndex(double distance, double index)
        {
            var response = await _context.FactRainList
                .Include(f => f.Time)
                .Include(f => f.Location)
                .Include(f => f.Source)
                .Include(f => f.Station)
                .Where(f => f.RainfallIndex > index)
                .Select(f =>
                    new MeasuredRainfallDTO
                    {
                        Source = f.Source.Source,
                        City = f.Location.City,
                        UF = f.Location.UF,
                        Day = f.Time.Day,
                        Month = f.Time.Month,
                        Year = f.Time.Year,
                        Hour = f.Time.Hour,
                        StationCode = f.Station.StationCode,
                        StationName = f.Station.StationName,
                        RainfallIndex = f.RainfallIndex,
                        Distance = 6371 *
                                Math.Acos(
                                    Math.Cos((Math.PI / 180) * (-22.913924)) * Math.Cos((Math.PI / 180) * (f.Location.Latitude)) *
                                    Math.Cos((Math.PI / 180) * (-43.084737) - (Math.PI / 180) * (f.Location.Longitude)) +
                                    Math.Sin((Math.PI / 180) * (-22.913924)) *
                                    Math.Sin((Math.PI / 180) * (f.Location.Latitude))
                                )
                    })
                .Where(s => s.Distance < distance)
                .ToListAsync();

            return response.Take(10).ToList();

        }

        public async Task<List<MeasuredRainfallDTO>> FilterByDistanceAndDate(double distance, int year, int month, int day)
        {
            var response = await _context.FactRainList
                .Include(f => f.Time)
                .Include(f => f.Location)
                .Include(f => f.Source)
                .Include(f => f.Station)
                .Where(f => 
                    f.Time.Year == year &&
                    f.Time.Month == month &&
                    f.Time.Day == day)
                .Select(f =>
                    new MeasuredRainfallDTO
                    {
                        Source = f.Source.Source,
                        City = f.Location.City,
                        UF = f.Location.UF,
                        Day = f.Time.Day,
                        Month = f.Time.Month,
                        Year = f.Time.Year,
                        Hour = f.Time.Hour,
                        StationCode = f.Station.StationCode,
                        StationName = f.Station.StationName,
                        RainfallIndex = f.RainfallIndex,
                        Distance = 6371 *
                                Math.Acos(
                                    Math.Cos((Math.PI / 180) * (-22.913924)) * Math.Cos((Math.PI / 180) * (f.Location.Latitude)) *
                                    Math.Cos((Math.PI / 180) * (-43.084737) - (Math.PI / 180) * (f.Location.Longitude)) +
                                    Math.Sin((Math.PI / 180) * (-22.913924)) *
                                    Math.Sin((Math.PI / 180) * (f.Location.Latitude))
                                )
                    })
                .Where(s => s.Distance < distance).ToListAsync();

            return response.Take(10).ToList();

        }

        public async Task<List<MeasuredRainfallDTO>> FilterByDistanceAndDateRange(DateTime firstDate, DateTime secondDate, double distance)
        {
            var dates = Utils.MaxMinDate(firstDate, secondDate);

            var response = await _context.FactRainList
                .Include(f => f.Location)
                .Include(f => f.Source)
                .Include(f => f.Station)
                .Include(f => f.Time)
                .Select(f =>
                    new MeasuredRainfallDTO
                    {
                        Source = f.Source.Source,
                        City = f.Location.City,
                        UF = f.Location.UF,
                        Day = f.Time.Day,
                        Month = f.Time.Month,
                        Year = f.Time.Year,
                        Hour = f.Time.Hour,
                        StationCode = f.Station.StationCode,
                        StationName = f.Station.StationName,
                        RainfallIndex = f.RainfallIndex,
                        Distance = 6371 *
                                Math.Acos(
                                    Math.Cos((Math.PI / 180) * (-22.913924)) * Math.Cos((Math.PI / 180) * (f.Location.Latitude)) *
                                    Math.Cos((Math.PI / 180) * (-43.084737) - (Math.PI / 180) * (f.Location.Longitude)) +
                                    Math.Sin((Math.PI / 180) * (-22.913924)) *
                                    Math.Sin((Math.PI / 180) * (f.Location.Latitude))
                                ),
                        Date = new DateTime(f.Time.Year, f.Time.Month, f.Time.Day)
                    })
                .Where(s =>
                    s.Distance < distance &&
                    s.Date <= dates.greaterDate &&
                    s.Date >= dates.lesserDate
                ).ToListAsync();

            return response.Take(10).ToList();
        }

        public async Task<List<MeasuredRainfallDTO>> FilterByDistanceAndCity(double distance, string city, int limit)
        {
            var response = await _context.FactRainList
                .Include(f => f.Location)
                .Include(f => f.Source)
                .Include(f => f.Station)
                .Select(f =>
                    new MeasuredRainfallDTO
                    {
                        Source = f.Source.Source,
                        City = f.Location.City,
                        UF = f.Location.UF,
                        StationCode = f.Station.StationCode,
                        StationName = f.Station.StationName,
                        Distance = 6371 *
                        Math.Acos(
                            Math.Cos((Math.PI / 180) * (-22.913924)) * Math.Cos((Math.PI / 180) * (f.Location.Latitude)) *
                            Math.Cos((Math.PI / 180) * (-43.084737) - (Math.PI / 180) * (f.Location.Longitude)) +
                            Math.Sin((Math.PI / 180) * (-22.913924)) *
                            Math.Sin((Math.PI / 180) * (f.Location.Latitude)))
                    })
                .Where(s =>
                    s.Distance > distance &&
                    s.City == city
                ).Distinct().Take(limit).ToListAsync();

            return response.ToList();
        }

        public Task<List<MeasuredRainfallDTO>> GetAverageRainfallIndexByCity(string city, int limit)
        {
            var response = _context.FactRainList
                .Include(f => f.Location)
                .Include(f => f.Time)
                .Include(f => f.Station)
                .Where(f => f.Location.City == city)
                .GroupBy(f => new
                {
                    f.Source.Source,
                    f.Location.City,
                    f.Location.UF,
                    f.Station.StationCode,
                    f.Station.StationName,
                    Distance = 6371 *
                            Math.Acos(
                                Math.Cos((Math.PI / 180) * (-22.913924)) * Math.Cos((Math.PI / 180) * (f.Location.Latitude)) *
                                Math.Cos((Math.PI / 180) * (-43.084737) - (Math.PI / 180) * (f.Location.Longitude)) +
                                Math.Sin((Math.PI / 180) * (-22.913924)) *
                                Math.Sin((Math.PI / 180) * (f.Location.Latitude))
                            )
                })
                .Select(g => new MeasuredRainfallDTO
                {
                    Source = g.Key.Source,
                    City = g.Key.City,
                    UF = g.Key.UF,
                    StationCode = g.Key.StationCode,
                    StationName = g.Key.StationName,
                    Distance = g.Key.Distance,
                    AverageRainfallIndex = g.Average(f => f.RainfallIndex)
                });

            return response.Distinct().Take(limit).ToListAsync();
        }

        public async Task<List<MeasuredRainfallDTO>> FilterByGeolocationAndCity(string city,
            double minLatitude, double maxLatitude,
            double minLongitude, double maxLongitude)
        {
            var response = await _context.FactRainList
                .Include(f => f.Time)
                .Include(f => f.Location)
                .Include(f => f.Source)
                .Include(f => f.Station)
                .Where(f => 
                    f.Location.Latitude > minLatitude &&
                    f.Location.Latitude < maxLatitude &&
                    f.Location.Longitude > minLongitude &&
                    f.Location.Longitude < maxLongitude
                )
                .Select(f =>
                    new MeasuredRainfallDTO
                    {
                        Source = f.Source.Source,
                        City = f.Location.City,
                        UF = f.Location.UF,
                        Day = f.Time.Day,
                        Month = f.Time.Month,
                        Year = f.Time.Year,
                        Hour = f.Time.Hour,
                        StationCode = f.Station.StationCode,
                        StationName = f.Station.StationName,
                        RainfallIndex = f.RainfallIndex,
                        Distance = 6371 *
                                Math.Acos(
                                    Math.Cos((Math.PI / 180) * (-22.913924)) * Math.Cos((Math.PI / 180) * (f.Location.Latitude)) *
                                    Math.Cos((Math.PI / 180) * (-43.084737) - (Math.PI / 180) * (f.Location.Longitude)) +
                                    Math.Sin((Math.PI / 180) * (-22.913924)) *
                                    Math.Sin((Math.PI / 180) * (f.Location.Latitude))
                                )
                    })
                .Where(s => s.City == city).ToListAsync();

            return response.Take(10).ToList();
        }

        public async Task<List<MeasuredRainfallDTO>> FilterByGeolocationAndDateRange(DateTime firstDate, DateTime secondDate,
            double minLatitude, double maxLatitude,
            double minLongitude, double maxLongitude)
        {
            var dates = Utils.MaxMinDate(firstDate, secondDate);

            var response = await _context.FactRainList
                .Include(f => f.Time)
                .Include(f => f.Location)
                .Include(f => f.Source)
                .Include(f => f.Station)
                .Where(f =>
                    f.Location.Latitude > minLatitude &&
                    f.Location.Latitude < maxLatitude &&
                    f.Location.Longitude > minLongitude &&
                    f.Location.Longitude < maxLongitude
                )
                .Select(f =>
                    new MeasuredRainfallDTO
                    {
                        Source = f.Source.Source,
                        City = f.Location.City,
                        UF = f.Location.UF,
                        Day = f.Time.Day,
                        Month = f.Time.Month,
                        Year = f.Time.Year,
                        Hour = f.Time.Hour,
                        StationCode = f.Station.StationCode,
                        StationName = f.Station.StationName,
                        RainfallIndex = f.RainfallIndex,
                        Distance = 6371 *
                                Math.Acos(
                                    Math.Cos((Math.PI / 180) * (-22.913924)) * Math.Cos((Math.PI / 180) * (f.Location.Latitude)) *
                                    Math.Cos((Math.PI / 180) * (-43.084737) - (Math.PI / 180) * (f.Location.Longitude)) +
                                    Math.Sin((Math.PI / 180) * (-22.913924)) *
                                    Math.Sin((Math.PI / 180) * (f.Location.Latitude))
                                ),
                        Date = new DateTime(f.Time.Year, f.Time.Month, f.Time.Day)
                    })
                .Where(s => 
                    s.Date <= dates.greaterDate &&
                    s.Date >= dates.lesserDate
                ).ToListAsync();

            return response.Take(10).ToList();
        }

        public async Task<List<MeasuredRainfallDTO>> FilterByGeolocationAndRainfallIndex(double index,
            double minLatitude, double maxLatitude,
            double minLongitude, double maxLongitude)
        {
            var response = await _context.FactRainList
                .Include(f => f.Time)
                .Include(f => f.Location)
                .Include(f => f.Source)
                .Include(f => f.Station)
                .Where(f =>
                    f.Location.Latitude > minLatitude &&
                    f.Location.Latitude < maxLatitude &&
                    f.Location.Longitude > minLongitude &&
                    f.Location.Longitude < maxLongitude
                )
                .Select(f =>
                    new MeasuredRainfallDTO
                    {
                        Source = f.Source.Source,
                        City = f.Location.City,
                        UF = f.Location.UF,
                        Day = f.Time.Day,
                        Month = f.Time.Month,
                        Year = f.Time.Year,
                        Hour = f.Time.Hour,
                        StationCode = f.Station.StationCode,
                        StationName = f.Station.StationName,
                        RainfallIndex = f.RainfallIndex,
                        Distance = 6371 *
                                Math.Acos(
                                    Math.Cos((Math.PI / 180) * (-22.913924)) * Math.Cos((Math.PI / 180) * (f.Location.Latitude)) *
                                    Math.Cos((Math.PI / 180) * (-43.084737) - (Math.PI / 180) * (f.Location.Longitude)) +
                                    Math.Sin((Math.PI / 180) * (-22.913924)) *
                                    Math.Sin((Math.PI / 180) * (f.Location.Latitude))
                                )
                    })
                .Where(s => s.RainfallIndex >= index).ToListAsync();

            return response.Take(10).ToList();
        }







        public Task<List<object>> FilterByDistanceAndYearRange(int greaterThanYear, int lessThanYear, double distance)
        {
            var yearRange = new List<int>();

            for (int i = greaterThanYear; i <= lessThanYear; i++)
            {
                yearRange.Add(i);
            }

            var response = _context.FactRainList
                .Include(f => f.Location)
                .Include(f => f.Time)
                .Where(f =>
                    yearRange.Contains(f.Time.Year)
                 )
                .Select(f => new
                {
                    Source = f,
                    Distancia = 6371 *
                            Math.Acos(
                                Math.Cos((Math.PI / 180) * (-22.913924)) * Math.Cos((Math.PI / 180) * (f.Location.Latitude)) *
                                Math.Cos((Math.PI / 180) * (-43.084737) - (Math.PI / 180) * (f.Location.Longitude)) +
                                Math.Sin((Math.PI / 180) * (-22.913924)) *
                                Math.Sin((Math.PI / 180) * (f.Location.Latitude))
                            )
                })
                .Where(s => s.Distancia < distance)
                .Select(w => (object) w);

            return response.Take(10).ToListAsync();
        }

        public Task<List<object>> FilterByDistanceAndYear(int year, double distance)
        {
            var response = _context.FactRainList
                .Include(f => f.Location)
                .Include(f => f.Time)
                .Where(f => f.Time.Year == year)
                .Select(f => new
                {
                    Source = f,
                    Distance = 6371 *
                            Math.Acos(
                                Math.Cos((Math.PI / 180) * (-22.913924)) * Math.Cos((Math.PI / 180) * (f.Location.Latitude)) *
                                Math.Cos((Math.PI / 180) * (-43.084737) - (Math.PI / 180) * (f.Location.Longitude)) +
                                Math.Sin((Math.PI / 180) * (-22.913924)) *
                                Math.Sin((Math.PI / 180) * (f.Location.Latitude))
                            )
                })
                .Where(s => s.Distance < distance)
                //.OrderBy(s => s.Source.Id)
                .Select(w => (object) w);


            return response.Take(10).ToListAsync();
        }

        //TODO: Verificar se ano e mês devem ser string ou int
        public Task<List<object>> GetMeasureByCityFilterByDate(int year)
        {
            var response = _context.FactRainList
                .Include(f => f.Location)
                .Include(f => f.Time)
                .Where(f => f.Time.Year == year)
                .GroupBy(f => new { f.Location.City, f.Time.Month, f.Time.Year})
                .Select(g => (object) new { 
                    g.Key.City, 
                    g.Key.Month, 
                    g.Key.Year, 
                    soma = g.Sum(f => f.RainfallIndex) }
                );

            return response.Take(10).ToListAsync();
        }

        public Task<List<object>> GetMeasureByCityFilterByYearAndDistance(int year, double distance)
        {
            var response = _context.FactRainList
                .Include(f => f.Location)
                .Include(f => f.Time)
                .Where(f => f.Time.Year == year)
                .GroupBy(f => new
                {
                    f.Location.City,
                    f.Time.Month,
                    Distance = 6371 *
                            Math.Acos(
                                Math.Cos((Math.PI / 180) * (-22.913924)) * Math.Cos((Math.PI / 180) * (f.Location.Latitude)) *
                                Math.Cos((Math.PI / 180) * (-43.084737) - (Math.PI / 180) * (f.Location.Longitude)) +
                                Math.Sin((Math.PI / 180) * (-22.913924)) *
                                Math.Sin((Math.PI / 180) * (f.Location.Latitude))
                            )
                })
                .Where(g => g.Key.Distance < distance)
                .Select(g => (object) new { 
                    g.Key.City,
                    g.Key.Month,
                    g.Key.Distance
                })
                ;

            return response.Take(10).ToListAsync();
        }

        public Task<List<object>> GetAverageMeasureByCityAndStationFilterByDateAndDistance(int year, double distance, int month)
        {
            var response = _context.FactRainList
                .Include(f => f.Location)
                .Include(f => f.Time)
                .Include(f => f.Station)
                .Where(f => f.Time.Year == year && f.Time.Month == month)
                .GroupBy(f => new
                {
                    f.Station.StationCode,
                    f.Station.StationName,
                    f.Location.City,
                    f.Time.Month,
                    f.Time.Year,
                    Distance = 6371 *
                            Math.Acos(
                                Math.Cos((Math.PI / 180) * (-22.913924)) * Math.Cos((Math.PI / 180) * (f.Location.Latitude)) *
                                Math.Cos((Math.PI / 180) * (-43.084737) - (Math.PI / 180) * (f.Location.Longitude)) +
                                Math.Sin((Math.PI / 180) * (-22.913924)) *
                                Math.Sin((Math.PI / 180) * (f.Location.Latitude))
                            )
                })
                .Where(g => g.Key.Distance < distance)
                .Select(g => (object) new
                {
                    g.Key.StationCode,
                    g.Key.StationName,
                    g.Key.City,
                    g.Key.Month,
                    g.Key.Year,
                    g.Key.Distance,
                    Average = g.Average(f => f.RainfallIndex)
                });

            return response.ToListAsync();
        }

        public async Task<List<MeasuredRainfall>> GetAll()
        {
            var response = _context.FactRainList
                .Include(f => f.Location)
                .Include(f => f.Time)
                .Include(f => f.Station);

            var facts = await response.Take(10).ToListAsync();

            return ConvertToMeasuredRainfallList(facts);
        }


        public Task<List<object>> GetAllWithDistance()
        {
            var response = _context.FactRainList
                .Include(f => f.Location)
                .Select(f => (object) new
                {
                    Source = f,
                    Distance = 6371 *
                            Math.Acos(
                                Math.Cos((Math.PI / 180) * (-22.913924)) * Math.Cos((Math.PI / 180) * (f.Location.Latitude)) *
                                Math.Cos((Math.PI / 180) * (-43.084737) - (Math.PI / 180) * (f.Location.Longitude)) +
                                Math.Sin((Math.PI / 180) * (-22.913924)) *
                                Math.Sin((Math.PI / 180) * (f.Location.Latitude))
                            )
                });

            return response.Take(10).ToListAsync();
        }

        public Task<List<object>> GetMeasureByCityAndYear()
        {
            var response = _context.FactRainList
                .Include(f => f.Location)
                .Include(f => f.Time)
                .GroupBy(f => new { f.Location.City, f.Time.Year })
                .Select(g => (object) new
                {
                    g.Key.City,
                    g.Key.Year,
                    Sum = g.Sum(f => f.RainfallIndex)
                });

            return response.ToListAsync();
        }

        public Task<List<object>> GetMeasureByCityAndYearFilterByDistance(double distance)
        {
            var response = _context.FactRainList
                .Include(f => f.Station)
                .GroupBy(f => new { 
                    f.Station.StationCode,
                    f.Station.StationName,
                    Distance = 6371 *
                            Math.Acos(
                                Math.Cos((Math.PI / 180) * (-22.913924)) * Math.Cos((Math.PI / 180) * (f.Location.Latitude)) *
                                Math.Cos((Math.PI / 180) * (-43.084737) - (Math.PI / 180) * (f.Location.Longitude)) +
                                Math.Sin((Math.PI / 180) * (-22.913924)) *
                                Math.Sin((Math.PI / 180) * (f.Location.Latitude))
                            )
                })
                .Where(g => g.Key.Distance < distance)
                .Select(g => (object) new
                {
                    g.Key.StationCode,
                    g.Key.StationName,
                    g.Key.Distance,
                    Sum = g.Sum(f => f.RainfallIndex)
                });

            return response.Take(10).ToListAsync();
        }

        public Task<List<object>> GetMeasureByCityAndDateFilterByDistance(double distance)
        {
            var response = _context.FactRainList
                .Include(f => f.Location)
                .Include(f => f.Station)
                .Include(f => f.Time)
                .GroupBy(f => new
                {
                    f.Time.Month,
                    f.Time.Year,
                    f.Location.City,
                    Distance = 6371 *
                            Math.Acos(
                                Math.Cos((Math.PI / 180) * (-22.913924)) * Math.Cos((Math.PI / 180) * (f.Location.Latitude)) *
                                Math.Cos((Math.PI / 180) * (-43.084737) - (Math.PI / 180) * (f.Location.Longitude)) +
                                Math.Sin((Math.PI / 180) * (-22.913924)) *
                                Math.Sin((Math.PI / 180) * (f.Location.Latitude))
                            )
                })
                .Where(g => g.Key.Distance < distance)
                .Select(g => (object) new
                {
                    g.Key.Month,
                    g.Key.Year,
                    g.Key.City,
                    g.Key.Distance,
                    sum = g.Sum(f => f.RainfallIndex)
                });

            return response.ToListAsync();
        }

        private List<MeasuredRainfall> ConvertToMeasuredRainfallList(List<FactRain> facts)
        {
            var result = new List<MeasuredRainfall>();
            foreach (var fact in facts)
            {
                var dateTime = new DateTime(fact.Time.Year, fact.Time.Month, fact.Time.Day);

                result.Add(new MeasuredRainfall(
                        fact.Location.City,
                        fact.Station.StationCode,
                        fact.Location.UF,
                        fact.Station.StationName,
                        fact.Location.Latitude,
                        fact.Location.Longitude,
                        dateTime,
                        fact.RainfallIndex,
                        fact.Time.Hour,
                        fact.Time.Day,
                        fact.Time.Month,
                        fact.Time.Year,
                        dateTime,
                        fact.Location.State,
                        fact.Location.NeighborHood));
            }

            return result;
        }

    }
}
