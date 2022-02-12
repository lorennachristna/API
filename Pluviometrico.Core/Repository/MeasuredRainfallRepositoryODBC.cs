using Pluviometrico.Core.DTOs;
using Pluviometrico.Core.Repository.Interface;
using System;
using System.Collections.Generic;
using System.Data.Odbc;
using System.Globalization;
using System.Threading.Tasks;
using Pluviometrico.Core.Repository.ODBC;

namespace Pluviometrico.Core.Repository
{
    public class MeasuredRainfallRepositoryODBC : IMeasuredRainfallRepository
    {
        private ODBCProperties _properties;

        public MeasuredRainfallRepositoryODBC(ODBCProperties properties)
        {
            _properties = properties;
        }

        public async Task<List<MeasuredRainfallDTO>> FilterByYear(int year)
        {

            var response = await ExecuteODBCQuery(
                @$"SELECT f.fonte, l.municipio, l.UF, t.dia, t.mes, t.ano, t.hora, e.cod_estacao_original, e.nome_estacao_original, fato.indice_pluv
                FROM {_properties.Schema}fato_chuva_cemaden fato
                INNER JOIN {_properties.Schema}dim_tempo t ON t.id_tempo = fato.id_tempo
                INNER JOIN {_properties.Schema}dim_localidade_cemaden l ON l.id = fato.id_localidade
                INNER JOIN {_properties.Schema}dim_fonte f ON f.id = fato.id_fonte
                INNER JOIN {_properties.Schema}dim_estacao e ON e.id = fato.id_estacao
                WHERE t.ano = {year}"
            );

            return response;
        }

        public async Task<List<MeasuredRainfallDTO>> FilterByRainfallIndex(double index)
        {
            var response = await ExecuteODBCQuery(
                @$"
                SELECT f.fonte, l.municipio, l.UF, t.dia, t.mes, t.ano, t.hora, e.cod_estacao_original, e.nome_estacao_original, fato.indice_pluv
                FROM {_properties.Schema}fato_chuva_cemaden fato
                INNER JOIN {_properties.Schema}dim_tempo t ON t.id_tempo = fato.id_tempo
                INNER JOIN {_properties.Schema}dim_localidade_cemaden l ON l.id = fato.id_localidade
                INNER JOIN {_properties.Schema}dim_fonte f ON f.id = fato.id_fonte
                INNER JOIN {_properties.Schema}dim_estacao e ON e.id = fato.id_estacao
                WHERE fato.indice_pluv > {index.ToString(CultureInfo.InvariantCulture)}
                "
            );

            return response;
        }

        public async Task<List<MeasuredRainfallDTO>> FilterByDistance(double distance)
        {
            var response = await ExecuteODBCQuery(
                @$"
                SELECT CALC.fonte, CALC.municipio, CALC.UF, CALC.dia, CALC.mes, CALC.ano, CALC.hora, CALC.cod_estacao_original, CALC.nome_estacao_original, CALC.DISTANCIA, CALC.indice_pluv
                FROM (SELECT f.fonte, l.municipio, l.UF, t.dia, t.mes, t.ano, t.hora, e.cod_estacao_original, e.nome_estacao_original, fato.indice_pluv, {_properties.DistanceCalculation} AS distancia
                FROM {_properties.Schema}fato_chuva_cemaden fato 
                INNER JOIN {_properties.Schema}dim_localidade_cemaden l ON l.id = fato.id_localidade
                INNER JOIN {_properties.Schema}dim_tempo t ON t.id_tempo = fato.id_tempo
                INNER JOIN {_properties.Schema}dim_fonte f ON f.id = fato.id_fonte
                INNER JOIN {_properties.Schema}dim_estacao e ON e.id = fato.id_estacao) AS CALC
                WHERE CALC.DISTANCIA < {distance.ToString(CultureInfo.InvariantCulture)}
                ");

            return response;
        }

        public async Task<List<MeasuredRainfallDTO>> FilterByDistanceAndRainfallIndex(double distance, double index)
        {
            var response = await ExecuteODBCQuery(
                @$"
                SELECT fonte, municipio, UF, dia, mes, ano, hora, cod_estacao_original, nome_estacao_original, CALC.DISTANCIA, indice_pluv
                FROM (SELECT f.fonte, l.municipio, l.UF, t.dia, t.mes, t.ano, t.hora, e.cod_estacao_original, e.nome_estacao_original, fato.indice_pluv, {_properties.DistanceCalculation} AS distancia
                FROM {_properties.Schema}fato_chuva_cemaden fato 
                INNER JOIN {_properties.Schema}dim_localidade_cemaden l ON l.id = fato.id_localidade
                INNER JOIN {_properties.Schema}dim_tempo t ON t.id_tempo = fato.id_tempo
                INNER JOIN {_properties.Schema}dim_fonte f ON f.id = fato.id_fonte
                INNER JOIN {_properties.Schema}dim_estacao e ON e.id = fato.id_estacao) AS CALC
                WHERE CALC.DISTANCIA < {distance.ToString(CultureInfo.InvariantCulture)} 
                AND CALC.indice_pluv > {index.ToString(CultureInfo.InvariantCulture)}
                ");

            return response;
        }

        public async Task<List<MeasuredRainfallDTO>> FilterByDistanceAndDate(double distance, int year, int month, int day)
        {
            var response = await ExecuteODBCQuery(
                @$"SELECT fonte, municipio, UF, dia, mes, ano, hora, cod_estacao_original, nome_estacao_original, CALC.DISTANCIA, CALC.indice_pluv
                FROM(SELECT f.fonte, l.municipio, l.UF, t.dia, t.mes, t.ano, t.hora, e.cod_estacao_original, e.nome_estacao_original, fato.indice_pluv, {_properties.DistanceCalculation} AS distancia
                FROM {_properties.Schema}fato_chuva_cemaden fato
                INNER JOIN {_properties.Schema}dim_localidade_cemaden l ON l.id = fato.id_localidade
                INNER JOIN {_properties.Schema}dim_tempo t ON t.id_tempo = fato.id_tempo
                INNER JOIN {_properties.Schema}dim_fonte f ON f.id = fato.id_fonte
                INNER JOIN {_properties.Schema}dim_estacao e ON e.id = fato.id_estacao) AS CALC
                WHERE CALC.DISTANCIA < {distance.ToString(CultureInfo.InvariantCulture)}
                AND dia = {day}
                AND mes = {month}
                AND ano = {year}");

            return response;
        }

        public async Task<List<MeasuredRainfallDTO>> FilterByDistanceAndDateRange(DateTime firstDate, DateTime secondDate, double distance)
        {
            var response = await ExecuteODBCQuery(@$"
                SELECT fonte, municipio, UF, dia, mes, ano, hora, cod_estacao_original, nome_estacao_original, CALC.DISTANCIA, CALC.indice_pluv
                FROM(SELECT f.fonte, l.municipio, l.UF, t.dia, t.mes, t.ano, t.hora, e.cod_estacao_original, e.nome_estacao_original, fato.indice_pluv, {_properties.DistanceCalculation} AS distancia
                FROM {_properties.Schema}fato_chuva_cemaden fato
                INNER JOIN {_properties.Schema}dim_localidade_cemaden l ON l.id = fato.id_localidade
                INNER JOIN {_properties.Schema}dim_tempo t ON t.id_tempo = fato.id_tempo
                INNER JOIN {_properties.Schema}dim_fonte f ON f.id = fato.id_fonte
                INNER JOIN {_properties.Schema}dim_estacao e ON e.id = fato.id_estacao) AS CALC
                WHERE CALC.DISTANCIA < {distance.ToString(CultureInfo.InvariantCulture)}
                AND cast((ano || '-' || mes || '-' || dia) as date) BETWEEN '{firstDate.ToString("yyyy-MM-dd")}' AND '{secondDate.ToString("yyyy-MM-dd")}'");

            return response;
        }

        public async Task<List<MeasuredRainfallDTO>> FilterByDistanceAndCity(double distance, string city, int limit)
        {
            var response = await ExecuteODBCQuery(
                @$"
                SELECT DISTINCT fonte, municipio, UF, cod_estacao_original, nome_estacao_original, CALC.DISTANCIA
                FROM (SELECT f.fonte, l.municipio, l.UF, e.cod_estacao_original, e.nome_estacao_original, fato.indice_pluv, {_properties.DistanceCalculation} AS distancia
                FROM {_properties.Schema}fato_chuva_cemaden fato 
                INNER JOIN {_properties.Schema}dim_localidade_cemaden l ON l.id = fato.id_localidade
                INNER JOIN {_properties.Schema}dim_fonte f ON f.id = fato.id_fonte
                INNER JOIN {_properties.Schema}dim_estacao e ON e.id = fato.id_estacao) AS CALC
                WHERE municipio = '{city}'
                AND calc.distancia > {distance.ToString(CultureInfo.InvariantCulture)}
                ORDER BY calc.distancia
                LIMIT {limit}
                "
            );

            return response;
        }

        public async Task<List<MeasuredRainfallDTO>> GetAverageRainfallIndexByCity(string city, int limit)
        {
            var response = await ExecuteODBCQuery(
                @$"
                SELECT DISTINCT fonte, municipio, UF, cod_estacao_original, nome_estacao_original, CALC.DISTANCIA, AVG(indice_pluv)
                FROM (SELECT f.fonte, l.municipio, l.UF, t.dia, t.mes, t.ano, t.hora, e.cod_estacao_original, e.nome_estacao_original, fato.indice_pluv, {_properties.DistanceCalculation} AS distancia
                FROM {_properties.Schema}fato_chuva_cemaden fato 
                INNER JOIN {_properties.Schema}dim_localidade_cemaden l ON l.id = fato.id_localidade
                INNER JOIN {_properties.Schema}dim_tempo t ON t.id_tempo = fato.id_tempo
                INNER JOIN {_properties.Schema}dim_fonte f ON f.id = fato.id_fonte
                INNER JOIN {_properties.Schema}dim_estacao e ON e.id = fato.id_estacao) AS CALC
                WHERE municipio = '{city}'
                GROUP BY fonte, municipio, UF, cod_estacao_original, nome_estacao_original, CALC.DISTANCIA
                ORDER BY calc.distancia
                LIMIT {limit}
                "
            );

            return response;
        }

        public async Task<List<MeasuredRainfallDTO>> FilterByGeolocationAndCity(string city, double minLatitude, double maxLatitude, double minLongitude, double maxLongitude)
        {
            var response = await ExecuteODBCQuery(
                @$"
                SELECT fonte, municipio, UF, dia, mes, ano, hora, cod_estacao_original, nome_estacao_original, CALC.DISTANCIA, CALC.indice_pluv
                FROM (SELECT f.fonte, l.municipio, l.UF, t.dia, t.mes, t.ano, t.hora, e.cod_estacao_original, e.nome_estacao_original, fato.indice_pluv, {_properties.DistanceCalculation} AS distancia
                FROM {_properties.Schema}fato_chuva_cemaden fato 
                INNER JOIN {_properties.Schema}dim_localidade_cemaden l ON l.id = fato.id_localidade
                INNER JOIN {_properties.Schema}dim_tempo t ON t.id_tempo = fato.id_tempo
                INNER JOIN {_properties.Schema}dim_fonte f ON f.id = fato.id_fonte
                INNER JOIN {_properties.Schema}dim_estacao e ON e.id = fato.id_estacao
	                 WHERE NOT ((l.latitude < {minLatitude.ToString(CultureInfo.InvariantCulture)})
		                   OR (l.latitude > {maxLatitude.ToString(CultureInfo.InvariantCulture)})
		                   OR (l.long < {minLongitude.ToString(CultureInfo.InvariantCulture)})
		                   OR (l.long > {maxLongitude.ToString(CultureInfo.InvariantCulture)})
		                   )
	                 ) AS CALC
                WHERE municipio = '{city}'
                "
            );

            return response;
        }

        public async Task<List<MeasuredRainfallDTO>> FilterByGeolocationAndDateRange(DateTime firstDate, DateTime secondDate, double minLatitude, double maxLatitude, double minLongitude, double maxLongitude)
        {
            var response = await ExecuteODBCQuery(
            @$"
            SELECT fonte, municipio, UF, dia, mes, ano, hora, cod_estacao_original, nome_estacao_original, CALC.DISTANCIA, CALC.indice_pluv
            FROM(SELECT f.fonte, l.municipio, l.UF, t.dia, t.mes, t.ano, t.hora, e.cod_estacao_original, e.nome_estacao_original, fato.indice_pluv, {_properties.DistanceCalculation} AS distancia
            FROM {_properties.Schema}fato_chuva_cemaden fato
            INNER JOIN {_properties.Schema}dim_localidade_cemaden l ON l.id = fato.id_localidade
            INNER JOIN {_properties.Schema}dim_tempo t ON t.id_tempo = fato.id_tempo
            INNER JOIN {_properties.Schema}dim_fonte f ON f.id = fato.id_fonte
            INNER JOIN {_properties.Schema}dim_estacao e ON e.id = fato.id_estacao
                 WHERE NOT((l.latitude < {minLatitude.ToString(CultureInfo.InvariantCulture)})
                       OR(l.latitude > {maxLatitude.ToString(CultureInfo.InvariantCulture)})
		               OR(l.long < {minLongitude.ToString(CultureInfo.InvariantCulture)})
		               OR(l.long > {maxLongitude.ToString(CultureInfo.InvariantCulture)})
		               )
	             ) AS CALC
            WHERE cast((ano || '-' || mes || '-' || dia) as date) BETWEEN '{firstDate.ToString("yyyy-MM-dd")}' AND '{secondDate.ToString("yyyy-MM-dd")}'");

            return response;
        }

        public async Task<List<MeasuredRainfallDTO>> FilterByGeolocationAndRainfallIndex(double index, double minLatitude, double maxLatitude, double minLongitude, double maxLongitude)
        {
            var response = await ExecuteODBCQuery(
            @$"
            SELECT fonte, municipio, UF, dia, mes, ano, hora, cod_estacao_original, nome_estacao_original, CALC.DISTANCIA, CALC.indice_pluv
            FROM(SELECT f.fonte, l.municipio, l.UF, t.dia, t.mes, t.ano, t.hora, e.cod_estacao_original, e.nome_estacao_original, fato.indice_pluv, {_properties.DistanceCalculation} AS distancia
            FROM {_properties.Schema}fato_chuva_cemaden fato
            INNER JOIN {_properties.Schema}dim_localidade_cemaden l ON l.id = fato.id_localidade
            INNER JOIN {_properties.Schema}dim_tempo t ON t.id_tempo = fato.id_tempo
            INNER JOIN {_properties.Schema}dim_fonte f ON f.id = fato.id_fonte
            INNER JOIN {_properties.Schema}dim_estacao e ON e.id = fato.id_estacao
                 WHERE NOT((l.latitude < {minLatitude.ToString(CultureInfo.InvariantCulture)})
                       OR(l.latitude > {maxLatitude.ToString(CultureInfo.InvariantCulture)})
		               OR(l.long < {minLongitude.ToString(CultureInfo.InvariantCulture)})
		               OR(l.long > {maxLongitude.ToString(CultureInfo.InvariantCulture)})
		               )
	             ) AS CALC
            WHERE CALC.indice_pluv >= {index.ToString(CultureInfo.InvariantCulture)}
            "
            );

            return response;
        }

        private async Task<List<MeasuredRainfallDTO>> ExecuteODBCQuery(
            string queryString,
            bool withDistance = false,
            bool withAvgIndex = false,
            bool withDateTime = false,
            bool withRainfallIndex = false
            )
        {
            var command = new OdbcCommand(queryString);

            var result = new List<MeasuredRainfallDTO>();

            using (OdbcConnection connection = new OdbcConnection(_properties.ConnectionString))
            {
                command.Connection = connection;
                connection.Open();

                using (var reader = await command.ExecuteReaderAsync())
                {
                    var sourceOrdinal = reader.GetOrdinal("fonte");  //"ordinal" is the position where a certain column is
                    var cityOrdinal = reader.GetOrdinal("municipio");
                    var ufOrdinal = reader.GetOrdinal("UF");
                    var stationCodeOrdinal = reader.GetOrdinal("cod_estacao_original");
                    var stationNameOrdinal = reader.GetOrdinal("nome_estacao_original");

                    var distanceOrdinal = 0;
                    var averageRainfallIndexOrdinal = 0;

                    var dayOrdinal = 0;
                    var monthOrdinal = 0;
                    var yearOrdinal = 0;
                    var hourOrdinal = 0;
                    var rainfallIndexOrdinal = 0;

                    if (withDateTime)
                    {
                        dayOrdinal = reader.GetOrdinal("dia");
                        monthOrdinal = reader.GetOrdinal("mes");
                        yearOrdinal = reader.GetOrdinal("ano");
                        hourOrdinal = reader.GetOrdinal("hora");
                    }

                    if (withDistance)
                    {
                        distanceOrdinal = reader.GetOrdinal("distancia");
                    }

                    if (withAvgIndex)
                    {
                        averageRainfallIndexOrdinal = reader.GetOrdinal("AVG(indice_pluv)");
                    }

                    if (withRainfallIndex)
                    {
                        rainfallIndexOrdinal = reader.GetOrdinal("indice_pluv");
                    }

                    //var count = 0;

                    while (reader.Read() 
                        //&& count < 5
                        )
                    {
                        //count++;
                        double? distance = null;
                        double? averageIndex = null;
                        int? day = null;
                        int? month = null;
                        int? year = null;
                        int? hour = null;
                        double? rainfallIndex = null;

                        if (withDistance)
                            distance = reader.IsDBNull(distanceOrdinal) ? null : (double) reader.GetDecimal(distanceOrdinal);

                        if (withAvgIndex)
                            averageIndex = reader.IsDBNull(averageRainfallIndexOrdinal) ? null : (double) reader.GetDecimal(averageRainfallIndexOrdinal);

                        if (withDateTime)
                        {
                            day = reader.IsDBNull(dayOrdinal) ? null : reader.GetInt32(dayOrdinal);
                            month = reader.IsDBNull(monthOrdinal) ? null : reader.GetInt32(monthOrdinal);
                            year = reader.IsDBNull(yearOrdinal) ? null : reader.GetInt32(yearOrdinal);
                            hour = reader.IsDBNull(hourOrdinal) ? null : reader.GetInt32(hourOrdinal);
                        }

                        if (withRainfallIndex)
                            rainfallIndex = reader.IsDBNull(rainfallIndexOrdinal) ? null : (double)reader.GetDecimal(rainfallIndexOrdinal);

                        result.Add(new MeasuredRainfallDTO
                        {
                            Source = reader.IsDBNull(sourceOrdinal) ? null : reader.GetString(sourceOrdinal),
                            City = reader.IsDBNull(cityOrdinal) ? null : reader.GetString(cityOrdinal),
                            UF = reader.IsDBNull(ufOrdinal) ? null : reader.GetString(ufOrdinal),
                            Day = day,
                            Month = month,
                            Year = year,
                            Hour = hour,
                            StationCode = reader.IsDBNull(stationCodeOrdinal) ? null : reader.GetString(stationCodeOrdinal),
                            StationName = reader.IsDBNull(stationNameOrdinal) ? null : reader.GetString(stationNameOrdinal),
                            Distance = distance,
                            RainfallIndex = rainfallIndex,
                            AverageRainfallIndex = averageIndex,
                        });

                    }
                }
                return result;
            }

        }
    }
}
