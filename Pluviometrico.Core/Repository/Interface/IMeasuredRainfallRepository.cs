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
    }
}
