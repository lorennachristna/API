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
        public Task<List<object>> FilterByYear(int year)
        {
             return _context.MeasuredRainfallList.Where(m => m.Ano == year).Select(m => (object) m).Take(10).ToListAsync();
        }

        public Task<List<object>> FilterByRainfallIndex(double index)
        {
            return _context.MeasuredRainfallList.Where(m => m.ValorMedida > index).Select(m => (object)m).Take(10).ToListAsync();
        }

        public Task<List<object>> FilterByDistance(double distance)
        {
            var response = _context.MeasuredRainfallList
                .Select(m =>
                    new {
                        Source = m,
                        Distancia = 6371 *
                            Math.Acos(
                                Math.Cos((Math.PI / 180) * (-22.9060000000000)) * Math.Cos((Math.PI / 180) * (m.Latitude)) *
                                Math.Cos((Math.PI / 180) * (-43.0530000000000) - (Math.PI / 180) * (m.Longitude)) +
                                Math.Sin((Math.PI / 180) * (-22.9060000000000)) *
                                Math.Sin((Math.PI / 180) * (m.Latitude))
                            )
                    }
                )
                .Where(s => s.Distancia < distance)
                .Select(w => (object)w);

            return response.Take(10).ToListAsync();
        }

        public Task<List<object>> FilterByDistanceAndRainfallIndex(double distance, double index)
        {
            var response = _context.MeasuredRainfallList
                .Where(m => m.ValorMedida > index)
                .Select(m =>
                    new {
                        Source = m,
                        Distancia = 6371 *
                            Math.Acos(
                                Math.Cos((Math.PI / 180) * (-22.9060000000000)) * Math.Cos((Math.PI / 180) * (m.Latitude)) *
                                Math.Cos((Math.PI / 180) * (-43.0530000000000) - (Math.PI / 180) * (m.Longitude)) +
                                Math.Sin((Math.PI / 180) * (-22.9060000000000)) *
                                Math.Sin((Math.PI / 180) * (m.Latitude))
                            )
                    }
                )
                .Where(s => s.Distancia < distance)
                .Select(w => (object)w);

            return response.Take(10).ToListAsync();
        }

        public Task<List<object>> FilterByDistanceAndDate(double distance, int year, int month, int day)
        {
            var response = _context.MeasuredRainfallList
                .Where(m => 
                    m.Ano == year &&
                    m.Mes == month &&
                    m.Dia == day)
                .Select(m =>
                    new {
                        Source = m,
                        Distancia = 6371 *
                            Math.Acos(
                                Math.Cos((Math.PI / 180) * (-22.9060000000000)) * Math.Cos((Math.PI / 180) * (m.Latitude)) *
                                Math.Cos((Math.PI / 180) * (-43.0530000000000) - (Math.PI / 180) * (m.Longitude)) +
                                Math.Sin((Math.PI / 180) * (-22.9060000000000)) *
                                Math.Sin((Math.PI / 180) * (m.Latitude))
                            )
                    }
                )
                .Where(s => s.Distancia < distance)
                .Select(w => (object)w);

            return response.Take(10).ToListAsync();
        }

        public Task<List<object>> FilterByDistanceAndDateRange(DateTime firstDate, DateTime secondDate, double distance)
        {
            var dates = Utils.MaxMinDate(firstDate, secondDate);

            var response = _context.MeasuredRainfallList
                .Select(m =>
                    new
                    {
                        Source = m,
                        Distancia = 6371 *
                            Math.Acos(
                                Math.Cos((Math.PI / 180) * (-22.9060000000000)) * Math.Cos((Math.PI / 180) * (m.Latitude)) *
                                Math.Cos((Math.PI / 180) * (-43.0530000000000) - (Math.PI / 180) * (m.Longitude)) +
                                Math.Sin((Math.PI / 180) * (-22.9060000000000)) *
                                Math.Sin((Math.PI / 180) * (m.Latitude))),
                        Data = new DateTime(m.Ano, m.Mes, m.Dia)
                    })
                .Where(s =>
                    s.Distancia < distance &&
                    s.Data <= dates.greaterDate &&
                    s.Data >= dates.lesserDate
                );

            return response.Select(r => (object)r).Take(10).ToListAsync();
        }

        public async Task<List<object>> FilterByDistanceAndCity(double distance, string city, int limit)
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
                        Distance = 6371 *
                            Math.Acos(
                                Math.Cos((Math.PI / 180) * (-22.9060000000000)) * Math.Cos((Math.PI / 180) * (m.Latitude)) *
                                Math.Cos((Math.PI / 180) * (-43.0530000000000) - (Math.PI / 180) * (m.Longitude)) +
                                Math.Sin((Math.PI / 180) * (-22.9060000000000)) *
                                Math.Sin((Math.PI / 180) * (m.Latitude))),
                    })
                .Where(s =>
                    s.Distance > distance &&
                    s.City == city
                ).Distinct().Take(limit).ToListAsync();

            return  response.Select(r => (object)r).ToList();
        }








        public Task<List<object>> FilterByDistanceAndYearRange(int greaterThanYear, int lessThanYear, double distance)
        {
            var response = _context.MeasuredRainfallList
                .Select(m =>
                new
                {
                    Source = m,
                    Distancia = 6371 *
                            Math.Acos(
                                Math.Cos((Math.PI / 180) * (-22.9060000000000)) * Math.Cos((Math.PI / 180) * (m.Latitude)) *
                                Math.Cos((Math.PI / 180) * (-43.0530000000000) - (Math.PI / 180) * (m.Longitude)) +
                                Math.Sin((Math.PI / 180) * (-22.9060000000000)) *
                                Math.Sin((Math.PI / 180) * (m.Latitude)))})
                .Where(n =>
                    n.Distancia < distance &&
                    n.Source.Ano >= greaterThanYear &&
                    n.Source.Ano <= lessThanYear)
                .Select(w => (object) w);

            return response.Take(10).ToListAsync();
        }

        public Task<List<object>> FilterByDistanceAndYear(int year, double distance)
        {
            var response = _context.MeasuredRainfallList
                .Select(m =>
                    new
                    {
                        Source = m,
                        Distancia = 6371 *
                            Math.Acos(
                                Math.Cos((Math.PI / 180) * (-22.9060000000000)) * Math.Cos((Math.PI / 180) * (m.Latitude)) *
                                Math.Cos((Math.PI / 180) * (-43.0530000000000) - (Math.PI / 180) * (m.Longitude)) +
                                Math.Sin((Math.PI / 180) * (-22.9060000000000)) *
                                Math.Sin((Math.PI / 180) * (m.Latitude))
                            )
                    }
                )
                .Where(n => n.Distancia < distance && n.Source.Ano == year)
                .Select(n => (object) n);

            return response.Take(10).ToListAsync();
        }

        public Task<List<object>> GetMeasureByCityFilterByDate(int year)
        {
            var response = _context.MeasuredRainfallList
                .Where(m => m.Ano == year)
                .GroupBy(m => new {
                    m.Municipio,
                    m.Mes,
                    m.Ano })
                .Select(g => (object) new {
                    g.Key.Municipio,
                    g.Key.Mes,
                    g.Key.Ano,
                    soma = g.Sum(s => s.ValorMedida)
                });

            return response.Take(10).ToListAsync();
        }

        public Task<List<object>> GetMeasureByCityFilterByYearAndDistance(int year, double distance)
        {
            var response = _context.MeasuredRainfallList
                .GroupBy(m => new {
                    m.Municipio,
                    m.Mes,
                    m.Ano,
                    distancia = 6371 *
                            Math.Acos(
                                Math.Cos((Math.PI / 180) * (-22.9060000000000)) * Math.Cos((Math.PI / 180) * (m.Latitude)) *
                                Math.Cos((Math.PI / 180) * (-43.0530000000000) - (Math.PI / 180) * (m.Longitude)) +
                                Math.Sin((Math.PI / 180) * (-22.9060000000000)) *
                                Math.Sin((Math.PI / 180) * (m.Latitude))
                            )
                })
                .Where(g => g.Key.Ano == year && g.Key.distancia < distance)
                .Select(g => (object)new
                {
                    g.Key.Municipio,
                    g.Key.Mes,
                    soma = g.Sum(s => s.ValorMedida),
                    g.Key.distancia
                });

            return response.Take(10).ToListAsync();

        }

        public Task<List<object>> GetAverageMeasureByCityAndStationFilterByDateAndDistance(int year, double distance, int month)
        {
            var response = _context.MeasuredRainfallList
                .GroupBy(m =>
                    new
                    {
                        m.CodEstacaoOriginal,
                        m.NomeEstacaoOriginal,
                        m.Municipio,
                        m.Mes,
                        m.Ano,
                        Distancia = 6371 *
                            Math.Acos(
                                Math.Cos((Math.PI / 180) * (-22.9060000000000)) * Math.Cos((Math.PI / 180) * (m.Latitude)) *
                                Math.Cos((Math.PI / 180) * (-43.0530000000000) - (Math.PI / 180) * (m.Longitude)) +
                                Math.Sin((Math.PI / 180) * (-22.9060000000000)) *
                                Math.Sin((Math.PI / 180) * (m.Latitude))
                            )
                    }
                )
                .Where(g => g.Key.Distancia < distance && g.Key.Ano == year && g.Key.Mes == month)
                .Select(g =>
                (object) new
                {
                    g.Key.CodEstacaoOriginal,
                    g.Key.NomeEstacaoOriginal,
                    g.Key.Municipio,
                    g.Key.Mes,
                    g.Key.Ano,
                    g.Key.Distancia,
                    media = g.Average(m => m.ValorMedida)
                }
            );

            return response.ToListAsync();
        }

        public Task<List<MeasuredRainfall>> GetAll()
        {
            return _context.MeasuredRainfallList.Take(10).ToListAsync();
        }

        public Task<List<object>> GetAllWithDistance()
        {
            var response = _context.MeasuredRainfallList
                .Select(m =>
                    (object) new
                    {
                        Source = m,
                        Distancia = 6371 *
                            Math.Acos(
                                Math.Cos((Math.PI / 180) * (-22.9060000000000)) * Math.Cos((Math.PI / 180) * (m.Latitude)) *
                                Math.Cos((Math.PI / 180) * (-43.0530000000000) - (Math.PI / 180) * (m.Longitude)) +
                                Math.Sin((Math.PI / 180) * (-22.9060000000000)) *
                                Math.Sin((Math.PI / 180) * (m.Latitude))
                            )
                    }
                );

            return response.Take(10).ToListAsync();
        }

        public Task<List<object>> GetMeasureByCityAndYear()
        {
            var response = _context.MeasuredRainfallList
                .GroupBy(m =>
                    new
                    {
                        m.Municipio,
                        m.Ano
                    }
                )
                .Select(g =>
                    (object) new
                    {
                        g.Key.Municipio,
                        g.Key.Ano,
                        soma = g.Sum(s => s.ValorMedida)
                    }
                );

            return response.Take(10).ToListAsync();
        }

        public Task<List<object>> GetMeasureByCityAndYearFilterByDistance(double distance)
        {
            var response = _context.MeasuredRainfallList
                .GroupBy(m =>
                    new
                    {
                        m.CodEstacaoOriginal,
                        m.NomeEstacaoOriginal,
                        Distancia = 6371 *
                            Math.Acos(
                                Math.Cos((Math.PI / 180) * (-22.9060000000000)) * Math.Cos((Math.PI / 180) * (m.Latitude)) *
                                Math.Cos((Math.PI / 180) * (-43.0530000000000) - (Math.PI / 180) * (m.Longitude)) +
                                Math.Sin((Math.PI / 180) * (-22.9060000000000)) *
                                Math.Sin((Math.PI / 180) * (m.Latitude))
                            )
                    })
                .Where(g => g.Key.Distancia < distance)
                .Select(g =>
                    (object) new
                    {
                        g.Key.CodEstacaoOriginal,
                        g.Key.NomeEstacaoOriginal,
                        g.Key.Distancia,
                        soma = g.Sum(s => s.ValorMedida)
                    }
                );

            return response.Take(10).ToListAsync();
        }

        public Task<List<object>> GetMeasureByCityAndDateFilterByDistance(double distance)
        {
            var response = _context.MeasuredRainfallList
                .GroupBy(m =>
                    new
                    {
                        m.Mes,
                        m.Ano,
                        m.Municipio,
                        Distancia = 6371 *
                            Math.Acos(
                                Math.Cos((Math.PI / 180) * (-22.9060000000000)) * Math.Cos((Math.PI / 180) * (m.Latitude)) *
                                Math.Cos((Math.PI / 180) * (-43.0530000000000) - (Math.PI / 180) * (m.Longitude)) +
                                Math.Sin((Math.PI / 180) * (-22.9060000000000)) *
                                Math.Sin((Math.PI / 180) * (m.Latitude))
                            )
                    })
                .Where(g => g.Key.Distancia < distance)
                .Select(g =>
                    (object) new
                    {
                        g.Key.Mes,
                        g.Key.Ano,
                        g.Key.Municipio,
                        g.Key.Distancia,
                        soma = g.Sum(s => s.ValorMedida)
                    }
                );

            return response.Take(10).ToListAsync();
        }

        private static double CalculateDistance(double latitude, double longitude)
        {

            var response = 6371 *
                            Math.Acos(
                                Math.Cos(ToRadians(-22.9060000000000)) * Math.Cos(ToRadians(latitude)) *
                                Math.Cos(ToRadians(-43.0530000000000) - ToRadians(longitude)) +
                                Math.Sin(ToRadians(-22.9060000000000)) *
                                Math.Sin(ToRadians(latitude))
                            );
            return response;
        }

        private static double ToRadians(double value)
        {
            return (Math.PI / 180) * value;
        }

    }
}
