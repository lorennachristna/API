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
        Task<List<object>> FilterByYear(int year);

        //Consultas 2 e 3
        //Devolve todos os campos
        //Filtra por índice pluviométrico
        Task<List<object>> FilterByRainfallIndex(double index);

        //Consulta 4
        //Devolve todos os campos e a distância calculada
        //Filtra por distância
        Task<List<object>> FilterByDistance(double distance);

        //Consulta 5
        //Devolve todos os campos e a distância calculada
        //Filtra por distância
        Task<List<object>> FilterByDistanceAndRainfallIndex(double distance, double index);

        //Consulta 6
        //Devolve todos os campos e a distância calculada
        //Filtra por distância, ano, mês, dia
        Task<List<object>> FilterByDistanceAndDate(double distance, int year, int month, int day);

        //Consulta 7
        //Devolve todos os campos e a distância calculada
        //Filtra por distância e intervalo de datas
        Task<List<object>> FilterByDistanceAndDateRange(DateTime firstDate, DateTime secondDate, double distance);

        //Consulta 8, 9 e 10
        //Devolve todos os campos e a distância calculada
        //Filtra por distância e cidade, limitando a um número de registros
        Task<List<object>> FilterByDistanceAndCity(double distance, string city, int limit);












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
