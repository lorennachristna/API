using Pluviometrico.Core.DTOs;
using Pluviometrico.Data;
using System;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace Pluviometrico.Core.Repository.Interface
{
    public interface IMeasuredRainfallRepository
    {
        //Consulta 1
        //Devolve todos os campos
        //Filtra por ano e mês
        Task<List<MeasuredRainfallDTO>> FilterByYear(int year);

        //Consultas 2 e 3
        //Devolve todos os campos
        //Filtra por índice pluviométrico
        Task<List<MeasuredRainfallDTO>> FilterByRainfallIndex(double index);

        //Consulta 4
        //Devolve todos os campos e a distância calculada
        //Filtra por distância
        Task<List<MeasuredRainfallDTO>> FilterByDistance(double distance);

        //Consulta 5
        //Devolve todos os campos e a distância calculada
        //Filtra por distância
        Task<List<MeasuredRainfallDTO>> FilterByDistanceAndRainfallIndex(double distance, double index);

        //Consulta 6
        //Devolve todos os campos e a distância calculada
        //Filtra por distância, ano, mês, dia
        Task<List<MeasuredRainfallDTO>> FilterByDistanceAndDate(double distance, int year, int month, int day);

        //Consulta 7
        //Devolve todos os campos e a distância calculada
        //Filtra por distância e intervalo de datas
        Task<List<MeasuredRainfallDTO>> FilterByDistanceAndDateRange(DateTime firstDate, DateTime secondDate, double distance);

        //Consulta 8, 9 e 10
        //Devolve todos os campos e a distância calculada
        //Filtra por distância e cidade, limitando a um número de registros
        Task<List<MeasuredRainfallDTO>> FilterByDistanceAndCity(double distance, string city, int limit);

        //Consulta 11
        //Devolve todos os campos, distância calculada e média de índice pluviométrico
        //Filtra por cidade, limitando o número de registros
        Task<List<MeasuredRainfallDTO>> GetAverageRainfallIndexByCity(string city, int limit);

        //Consulta 13
        Task<List<MeasuredRainfallDTO>> FilterByGeolocationAndCity(string city, 
            double minLatitude, double maxLatitude,
            double minLongitude, double maxLongitude);
        
        //Consulta 14
        Task<List<MeasuredRainfallDTO>> FilterByGeolocationAndDateRange(DateTime firstDate, DateTime secondDate,
            double minLatitude, double maxLatitude,
            double minLongitude, double maxLongitude);

        //Consulta 15
        Task<List<MeasuredRainfallDTO>> FilterByGeolocationAndRainfallIndex(double index,
            double minLatitude, double maxLatitude,
            double minLongitude, double maxLongitude);











        //2
        //Devolve todos os campos
        //Filtra por intervalo de anos e uma distância
        Task<List<object>> FilterByDistanceAndYearRange(int greaterThanYear, int lessThanYear, double distance);

        //3
        //Devolve todos os campos
        //Filtra por ano e distância
        Task<List<object>> FilterByDistanceAndYear(int year, double distance);

        //4
        //Devolve município, mês, ano e
        //Valores de chuva agregados (somados) por município, mês e ano
        //Filtra por ano
        Task<List<object>> GetMeasureByCityFilterByDate(int year);

        //5
        //Devolve município, mês, distância e
        //valores de chuva agregados (somados) por município, mês, ano e distância
        //Filtra por ano e distância
        Task<List<object>> GetMeasureByCityFilterByYearAndDistance(int year, double distance);

        //TODO: incluir filtro por mês
        //6
        //Devolve código e nome da estação, município, mês, ano, distância e
        //média de valores de chuva (agregação) por estação, município, mês, ano e distância
        //Filtra por ano e distância E MÊS
        Task<List<object>> GetAverageMeasureByCityAndStationFilterByDateAndDistance(int year, double distance, int month);

        //7
        //Devolve todos os campos
        Task<List<MeasuredRainfall>> GetAll();

        //9
        //Devolve todos os campos e a distância calculada
        Task<List<object>> GetAllWithDistance();

        //10
        //Devolve município, ano e
        //valores de chuva agregados (somados) por município e ano
        Task<List<object>> GetMeasureByCityAndYear();

        //11
        //Devolve código e nome da estação, distância e
        //valores de chuva agregados (somados) por município e ano
        //Filtra por distância
        Task<List<object>> GetMeasureByCityAndYearFilterByDistance(double distance);

        //12
        //Devolve mês, ano, município, distância e
        //valores de chuva agregados (somados) por mês, ano, município e distância
        //Filtra por distância
        Task<List<object>> GetMeasureByCityAndDateFilterByDistance(double distance);
    }
}
