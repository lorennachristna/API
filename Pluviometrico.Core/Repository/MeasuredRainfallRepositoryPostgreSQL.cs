using Microsoft.EntityFrameworkCore;
using Pluviometrico.Core.DTOs;
using Pluviometrico.Core.Repository.Interface;
using Pluviometrico.Data;
using Pluviometrico.Data.DatabaseContext;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace Pluviometrico.Core.Repository
{
    public class MeasuredRainfallRepositoryPostgreSQL : IMeasuredRainfallRepository
    {
        private readonly PostgreSQLContext _context;

        public MeasuredRainfallRepositoryPostgreSQL(PostgreSQLContext context)
        {
            _context = context;
        }

        //TODO: Remove .Take
        //TODO: Add Classes for each response type? Like with fields Distance and Source.
        //TODO: Make distance calculation function work
        public Task<List<MeasuredRainfallDTO>> FilterByYear(int year)
        {
             return _context.MeasuredRainfallList.Where(m => m.Ano == year).Select(m => 
             new MeasuredRainfallDTO 
             { 
                 Source = "CEMADEN",
                 City = m.Municipio,
                 UF = m.UF,
                 Day = m.Dia,
                 Month = m.Mes,
                 Year = m.Ano,
                 Hour = m.Hora,
                 StationCode = m.CodEstacaoOriginal,
                 StationName = m.NomeEstacaoOriginal,
                 RainfallIndex = m.ValorMedida
             }).Take(10).ToListAsync();
        }

        public Task<List<MeasuredRainfallDTO>> FilterByRainfallIndex(double index)
        {
            return _context.MeasuredRainfallList.Where(m => m.ValorMedida > index).Select(m =>
             new MeasuredRainfallDTO
             {
                 Source = "CEMADEN",
                 City = m.Municipio,
                 UF = m.UF,
                 Day = m.Dia,
                 Month = m.Mes,
                 Year = m.Ano,
                 Hour = m.Hora,
                 StationCode = m.CodEstacaoOriginal,
                 StationName = m.NomeEstacaoOriginal,
                 RainfallIndex = m.ValorMedida
             }).Take(10).ToListAsync();
        }

        public Task<List<MeasuredRainfallDTO>> FilterByDistance(double distance)
        {
            var response = _context.MeasuredRainfallList
                .Select(m => new MeasuredRainfallDTO
                    {
                        Source = "CEMADEN",
                        City = m.Municipio,
                        UF = m.UF,
                        Day = m.Dia,
                        Month = m.Mes,
                        Year = m.Ano,
                        Hour = m.Hora,
                        StationCode = m.CodEstacaoOriginal,
                        StationName = m.NomeEstacaoOriginal,
                        RainfallIndex = m.ValorMedida,
                        Distance = 6371 *
                            Math.Acos(
                                Math.Cos((Math.PI / 180) * (-22.913924)) * Math.Cos((Math.PI / 180) * (m.Latitude)) *
                                Math.Cos((Math.PI / 180) * (-43.084737) - (Math.PI / 180) * (m.Longitude)) +
                                Math.Sin((Math.PI / 180) * (-22.913924)) *
                                Math.Sin((Math.PI / 180) * (m.Latitude))
                            )
                    }
                )
                .Where(s => s.Distance < distance)
                .Take(10).ToListAsync();

            return response;
        }

        public async Task<List<MeasuredRainfallDTO>> FilterByDistanceAndRainfallIndex(double distance, double index)
        {
            var response = await _context.MeasuredRainfallList
                .Where(m => m.ValorMedida > index)
                .Select(m => new MeasuredRainfallDTO
                    {
                        Source = "CEMADEN",
                        City = m.Municipio,
                        UF = m.UF,
                        Day = m.Dia,
                        Month = m.Mes,
                        Year = m.Ano,
                        Hour = m.Hora,
                        StationCode = m.CodEstacaoOriginal,
                        StationName = m.NomeEstacaoOriginal,
                        RainfallIndex = m.ValorMedida,
                        Distance = 6371 *
                                Math.Acos(
                                    Math.Cos((Math.PI / 180) * (-22.913924)) * Math.Cos((Math.PI / 180) * (m.Latitude)) *
                                    Math.Cos((Math.PI / 180) * (-43.084737) - (Math.PI / 180) * (m.Longitude)) +
                                    Math.Sin((Math.PI / 180) * (-22.913924)) *
                                    Math.Sin((Math.PI / 180) * (m.Latitude))
                                )
                    }
                )
                .Where(s => s.Distance < distance).ToListAsync();

            return response.Take(10).ToList();
        }

        public async Task<List<MeasuredRainfallDTO>> FilterByDistanceAndDate(double distance, int year, int month, int day)
        {
            var response = await _context.MeasuredRainfallList
                .Where(m => 
                    m.Ano == year &&
                    m.Mes == month &&
                    m.Dia == day)
                .Select(m => 
                    new MeasuredRainfallDTO
                    {
                        Source = "CEMADEN",
                        City = m.Municipio,
                        UF = m.UF,
                        Day = m.Dia,
                        Month = m.Mes,
                        Year = m.Ano,
                        Hour = m.Hora,
                        StationCode = m.CodEstacaoOriginal,
                        StationName = m.NomeEstacaoOriginal,
                        RainfallIndex = m.ValorMedida,
                        Distance = 6371 *
                                Math.Acos(
                                    Math.Cos((Math.PI / 180) * (-22.913924)) * Math.Cos((Math.PI / 180) * (m.Latitude)) *
                                    Math.Cos((Math.PI / 180) * (-43.084737) - (Math.PI / 180) * (m.Longitude)) +
                                    Math.Sin((Math.PI / 180) * (-22.913924)) *
                                    Math.Sin((Math.PI / 180) * (m.Latitude))
                                )
                    }
                )
                .Where(s => s.Distance < distance)
                .ToListAsync();

            return response.Take(10).ToList();
        }

        public async Task<List<MeasuredRainfallDTO>> FilterByDistanceAndDateRange(DateTime firstDate, DateTime secondDate, double distance)
        {
            var dates = Utils.MaxMinDate(firstDate, secondDate);

            var response = await _context.MeasuredRainfallList
                .Select(m =>
                    new MeasuredRainfallDTO
                    {
                        Source = "CEMADEN",
                        City = m.Municipio,
                        UF = m.UF,
                        Day = m.Dia,
                        Month = m.Mes,
                        Year = m.Ano,
                        Hour = m.Hora,
                        StationCode = m.CodEstacaoOriginal,
                        StationName = m.NomeEstacaoOriginal,
                        RainfallIndex = m.ValorMedida,
                        Distance = 6371 *
                                Math.Acos(
                                    Math.Cos((Math.PI / 180) * (-22.913924)) * Math.Cos((Math.PI / 180) * (m.Latitude)) *
                                    Math.Cos((Math.PI / 180) * (-43.084737) - (Math.PI / 180) * (m.Longitude)) +
                                    Math.Sin((Math.PI / 180) * (-22.913924)) *
                                    Math.Sin((Math.PI / 180) * (m.Latitude))
                                ),
                        Date = new DateTime(m.Ano, m.Mes, m.Dia)
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
            var response = await _context.MeasuredRainfallList
                .Select(m =>
                    new MeasuredRainfallDTO
                    {
                        Source = "CEMADEN",
                        City = m.Municipio,
                        UF = m.UF,
                        StationCode = m.CodEstacaoOriginal,
                        StationName = m.NomeEstacaoOriginal,
                        RainfallIndex = m.ValorMedida,
                        Distance = 6371 *
                            Math.Acos(
                                Math.Cos((Math.PI / 180) * (-22.913924)) * Math.Cos((Math.PI / 180) * (m.Latitude)) *
                                Math.Cos((Math.PI / 180) * (-43.084737) - (Math.PI / 180) * (m.Longitude)) +
                                Math.Sin((Math.PI / 180) * (-22.913924)) *
                                Math.Sin((Math.PI / 180) * (m.Latitude))),
                    })
                .Where(s =>
                    s.Distance > distance &&
                    s.City == city
                ).Distinct().ToListAsync();

            return response.Take(limit).ToList();
        }

        public Task<List<MeasuredRainfallDTO>> GetAverageRainfallIndexByCity(string city, int limit)
        {
            var response = _context.MeasuredRainfallList
                .GroupBy(m =>
                    new
                    {
                        m.Municipio,
                        m.UF,
                        m.CodEstacaoOriginal,
                        m.NomeEstacaoOriginal,
                        Distancia = 6371 *
                            Math.Acos(
                                Math.Cos((Math.PI / 180) * (-22.913924)) * Math.Cos((Math.PI / 180) * (m.Latitude)) *
                                Math.Cos((Math.PI / 180) * (-43.084737) - (Math.PI / 180) * (m.Longitude)) +
                                Math.Sin((Math.PI / 180) * (-22.913924)) *
                                Math.Sin((Math.PI / 180) * (m.Latitude))
                            )
                    }
                )
                .Where(g => g.Key.Municipio == city)
                .Select(g =>
                new MeasuredRainfallDTO
                {
                    Source = "CEMADEN",
                    City = g.Key.Municipio,
                    UF = g.Key.UF,
                    StationCode = g.Key.CodEstacaoOriginal,
                    StationName = g.Key.NomeEstacaoOriginal,
                    Distance = g.Key.Distancia,
                    AverageRainfallIndex = g.Average(m => m.ValorMedida)
                }
            );

            return response.Take(limit).Distinct().ToListAsync();
        }

        public async Task<List<MeasuredRainfallDTO>> FilterByGeolocationAndCity(string city,
            double minLatitude, double maxLatitude,
            double minLongitude, double maxLongitude)
        {
            var response = await _context.MeasuredRainfallList
                .Where(m => 
                    m.Latitude > minLatitude &&
                    m.Latitude < maxLatitude &&
                    m.Longitude > minLongitude &&
                    m.Longitude < maxLongitude
                )
                .Select(m => new MeasuredRainfallDTO
                    {
                        Source = "CEMADEN",
                        City = m.Municipio,
                        UF = m.UF,
                        Day = m.Dia,
                        Month = m.Mes,
                        Year = m.Ano,
                        Hour = m.Hora,
                        StationCode = m.CodEstacaoOriginal,
                        StationName = m.NomeEstacaoOriginal,
                        RainfallIndex = m.ValorMedida,
                        Distance = 6371 *
                                Math.Acos(
                                    Math.Cos((Math.PI / 180) * (-22.913924)) * Math.Cos((Math.PI / 180) * (m.Latitude)) *
                                    Math.Cos((Math.PI / 180) * (-43.084737) - (Math.PI / 180) * (m.Longitude)) +
                                    Math.Sin((Math.PI / 180) * (-22.913924)) *
                                    Math.Sin((Math.PI / 180) * (m.Latitude))
                                )
                    }
                )
                .Where(s => s.City == city).ToListAsync();

            return response.Take(10).ToList();
        }

        public async Task<List<MeasuredRainfallDTO>> FilterByGeolocationAndDateRange(DateTime firstDate, DateTime secondDate,
            double minLatitude, double maxLatitude,
            double minLongitude, double maxLongitude)
        {
            var dates = Utils.MaxMinDate(firstDate, secondDate);

            var response = await _context.MeasuredRainfallList
                .Where(m => 
                    m.Latitude > minLatitude &&
                    m.Latitude < maxLatitude &&
                    m.Longitude > minLongitude &&
                    m.Longitude < maxLongitude
                )
                .Select(m => new MeasuredRainfallDTO
                    {
                        Source = "CEMADEN",
                        City = m.Municipio,
                        UF = m.UF,
                        Day = m.Dia,
                        Month = m.Mes,
                        Year = m.Ano,
                        Hour = m.Hora,
                        StationCode = m.CodEstacaoOriginal,
                        StationName = m.NomeEstacaoOriginal,
                        RainfallIndex = m.ValorMedida,
                        Distance = 6371 *
                                Math.Acos(
                                    Math.Cos((Math.PI / 180) * (-22.913924)) * Math.Cos((Math.PI / 180) * (m.Latitude)) *
                                    Math.Cos((Math.PI / 180) * (-43.084737) - (Math.PI / 180) * (m.Longitude)) +
                                    Math.Sin((Math.PI / 180) * (-22.913924)) *
                                    Math.Sin((Math.PI / 180) * (m.Latitude))
                                ),
                        Date = new DateTime(m.Ano, m.Mes, m.Dia)
                    }
                )
                .Where(s => 
                    s.Date >= dates.lesserDate &&
                    s.Date <= dates.greaterDate 
                ).ToListAsync();

            return response.Take(10).ToList();
        }

        public async Task<List<MeasuredRainfallDTO>> FilterByGeolocationAndRainfallIndex(double index,
            double minLatitude, double maxLatitude,
            double minLongitude, double maxLongitude)
        {
            var response = await _context.MeasuredRainfallList
                .Where(m =>
                    m.Latitude > minLatitude &&
                    m.Latitude < maxLatitude &&
                    m.Longitude > minLongitude &&
                    m.Longitude < maxLongitude
                )
                .Select(m => new MeasuredRainfallDTO
                {
                    Source = "CEMADEN",
                    City = m.Municipio,
                    UF = m.UF,
                    Day = m.Dia,
                    Month = m.Mes,
                    Year = m.Ano,
                    Hour = m.Hora,
                    StationCode = m.CodEstacaoOriginal,
                    StationName = m.NomeEstacaoOriginal,
                    RainfallIndex = m.ValorMedida,
                    Distance = 6371 *
                                Math.Acos(
                                    Math.Cos((Math.PI / 180) * (-22.913924)) * Math.Cos((Math.PI / 180) * (m.Latitude)) *
                                    Math.Cos((Math.PI / 180) * (-43.084737) - (Math.PI / 180) * (m.Longitude)) +
                                    Math.Sin((Math.PI / 180) * (-22.913924)) *
                                    Math.Sin((Math.PI / 180) * (m.Latitude))
                                )
                }
                )
                .Where(s => s.RainfallIndex >= index).ToListAsync();

            return response.Take(10).ToList();
        }
    }
}
