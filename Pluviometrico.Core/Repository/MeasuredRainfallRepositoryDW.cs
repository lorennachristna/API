using Microsoft.EntityFrameworkCore;
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

        public async Task<List<object>> FilterByMonthAndYear(int month, int year)
        {
            var response = _context.FactRainList
                .Include(f => f.Time)
                .Where(f => f.Time.Month == month.ToString() && f.Time.Year == year.ToString());

            var facts = await response.Take(10).Select(fact => (object) fact).ToListAsync();

            return facts;
        }

        public Task<List<object>> FilterByDistanceAndYearRange(int greaterThanYear, int lessThanYear, double distance)
        {
            var yearRange = new List<string>();

            for (int i = greaterThanYear; i <= lessThanYear; i++)
            {
                yearRange.Add(i.ToString());
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
                                Math.Cos((Math.PI / 180) * (-22.9060000000000)) * Math.Cos((Math.PI / 180) * (f.Location.Latitude)) *
                                Math.Cos((Math.PI / 180) * (-43.0530000000000) - (Math.PI / 180) * (f.Location.Longitude)) +
                                Math.Sin((Math.PI / 180) * (-22.9060000000000)) *
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
                .Where(f => f.Time.Year == year.ToString())
                .Select(f => new
                {
                    Source = f,
                    Distance = 6371 *
                            Math.Acos(
                                Math.Cos((Math.PI / 180) * (-22.9060000000000)) * Math.Cos((Math.PI / 180) * (f.Location.Latitude)) *
                                Math.Cos((Math.PI / 180) * (-43.0530000000000) - (Math.PI / 180) * (f.Location.Longitude)) +
                                Math.Sin((Math.PI / 180) * (-22.9060000000000)) *
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
                .Where(f => f.Time.Year == year.ToString())
                .GroupBy(f => new { f.Location.Town, f.Time.Month, f.Time.Year})
                .Select(g => (object) new { 
                    g.Key.Town, 
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
                .Where(f => f.Time.Year == year.ToString())
                .GroupBy(f => new
                {
                    f.Location.Town,
                    f.Time.Month,
                    Distance = 6371 *
                            Math.Acos(
                                Math.Cos((Math.PI / 180) * (-22.9060000000000)) * Math.Cos((Math.PI / 180) * (f.Location.Latitude)) *
                                Math.Cos((Math.PI / 180) * (-43.0530000000000) - (Math.PI / 180) * (f.Location.Longitude)) +
                                Math.Sin((Math.PI / 180) * (-22.9060000000000)) *
                                Math.Sin((Math.PI / 180) * (f.Location.Latitude))
                            )
                })
                .Where(g => g.Key.Distance < distance)
                .Select(g => (object) new { 
                    g.Key.Town,
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
                .Where(f => f.Time.Year == year.ToString() && f.Time.Month == month.ToString())
                .GroupBy(f => new
                {
                    f.Station.StationCode,
                    f.Station.StationName,
                    f.Location.Town,
                    f.Time.Month,
                    f.Time.Year,
                    Distance = 6371 *
                            Math.Acos(
                                Math.Cos((Math.PI / 180) * (-22.9060000000000)) * Math.Cos((Math.PI / 180) * (f.Location.Latitude)) *
                                Math.Cos((Math.PI / 180) * (-43.0530000000000) - (Math.PI / 180) * (f.Location.Longitude)) +
                                Math.Sin((Math.PI / 180) * (-22.9060000000000)) *
                                Math.Sin((Math.PI / 180) * (f.Location.Latitude))
                            )
                })
                .Where(g => g.Key.Distance < distance)
                .Select(g => (object) new
                {
                    g.Key.StationCode,
                    g.Key.StationName,
                    g.Key.Town,
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

        public Task<List<object>> FilterByDistance(double distance)
        {
            var response = _context.FactRainList
                .Include(f => f.Location)
                .Select(f => new {
                    Source = f,
                    Distance = 6371 *
                            Math.Acos(
                                Math.Cos((Math.PI / 180) * (-22.9060000000000)) * Math.Cos((Math.PI / 180) * (f.Location.Latitude)) *
                                Math.Cos((Math.PI / 180) * (-43.0530000000000) - (Math.PI / 180) * (f.Location.Longitude)) +
                                Math.Sin((Math.PI / 180) * (-22.9060000000000)) *
                                Math.Sin((Math.PI / 180) * (f.Location.Latitude))
                            )
                })
                .Where(s => s.Distance < distance)
                .Select(w => (object) w);

            return response.Take(10).ToListAsync();

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
                                Math.Cos((Math.PI / 180) * (-22.9060000000000)) * Math.Cos((Math.PI / 180) * (f.Location.Latitude)) *
                                Math.Cos((Math.PI / 180) * (-43.0530000000000) - (Math.PI / 180) * (f.Location.Longitude)) +
                                Math.Sin((Math.PI / 180) * (-22.9060000000000)) *
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
                .GroupBy(f => new { f.Location.Town, f.Time.Year })
                .Select(g => (object) new
                {
                    g.Key.Town,
                    g.Key.Year,
                    Sum = g.Sum(f => f.RainfallIndex)
                });

            return response.ToListAsync();        }

        public Task<List<object>> GetMeasureByCityAndYearFilterByDistance(double distance)
        {
            var response = _context.FactRainList
                .Include(f => f.Station)
                .GroupBy(f => new { 
                    f.Station.StationCode,
                    f.Station.StationName,
                    Distance = 6371 *
                            Math.Acos(
                                Math.Cos((Math.PI / 180) * (-22.9060000000000)) * Math.Cos((Math.PI / 180) * (f.Location.Latitude)) *
                                Math.Cos((Math.PI / 180) * (-43.0530000000000) - (Math.PI / 180) * (f.Location.Longitude)) +
                                Math.Sin((Math.PI / 180) * (-22.9060000000000)) *
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
                    f.Location.Town,
                    Distance = 6371 *
                            Math.Acos(
                                Math.Cos((Math.PI / 180) * (-22.9060000000000)) * Math.Cos((Math.PI / 180) * (f.Location.Latitude)) *
                                Math.Cos((Math.PI / 180) * (-43.0530000000000) - (Math.PI / 180) * (f.Location.Longitude)) +
                                Math.Sin((Math.PI / 180) * (-22.9060000000000)) *
                                Math.Sin((Math.PI / 180) * (f.Location.Latitude))
                            )
                })
                .Where(g => g.Key.Distance < distance)
                .Select(g => (object) new
                {
                    g.Key.Month,
                    g.Key.Year,
                    g.Key.Town,
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
                var dateTime = new DateTime(int.Parse(fact.Time.Year), int.Parse(fact.Time.Month), int.Parse(fact.Time.Day), int.Parse(fact.Time.Hour), int.Parse(fact.Time.Minute), 0);

                result.Add(new MeasuredRainfall(
                        fact.Id,
                        fact.Location.Town,
                        fact.Station.StationCode,
                        fact.Location.UF,
                        fact.Station.StationName,
                        fact.Location.Latitude,
                        fact.Location.Longitude,
                        dateTime.ToString(),
                        fact.RainfallIndex,
                        int.Parse(fact.Time.Hour),
                        int.Parse(fact.Time.Day),
                        int.Parse(fact.Time.Minute),
                        int.Parse(fact.Time.Month),
                        int.Parse(fact.Time.Year),
                        dateTime.ToString(),
                        fact.Location.State,
                        fact.Location.NeighborHood,
                        fact.Location.City
                    ));
            }

            return result;
        }

    }
}
