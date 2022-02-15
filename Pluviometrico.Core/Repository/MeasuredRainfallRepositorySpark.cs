using Microsoft.Spark.Sql;
using Nest;
using Pluviometrico.Core.DTOs;
using Pluviometrico.Core.Repository.Interface;
using System;
using System.Collections.Generic;
using System.Globalization;
using System.IO;
using System.Linq;
using System.Text;
using System.Text.Json;
using System.Threading.Tasks;

namespace Pluviometrico.Core.Repository
{
    public class MeasuredRainfallRepositorySpark : IMeasuredRainfallRepository
    {

        private string[] stationColumnNames = { "id", "cod_estacao_original", "nome_estacao_original" };
        private string[] timeColumnNames = { "id_tempo", "hora", "dia", "semana", "mes", "semestre", "ano" };
        private string[] locationColumnNames = { "id", "latitude", "long", "rua", "bairro", "municipio", "uf" };
        private string[] sourceColumnNames = { "id", "fonte", "url" };
        private string[] factColumnNames = { "id_fonte", "id_tempo", "id_localidade", "id_estacao", "indice_pluv" };

        public MeasuredRainfallRepositorySpark()
        {
        }

        public async Task<List<MeasuredRainfallDTO>> FilterByYear(int year)
        {
            SparkSession spark = SparkSession
                .Builder()
                .AppName("sparkQueries")
                .GetOrCreate();

            SetUpSparkTables(spark);

            var response = await ExecuteSparkQuery(spark, 
                @$"SELECT f.fonte, l.municipio, l.UF, t.dia, t.mes, t.ano, t.hora, e.cod_estacao_original, e.nome_estacao_original, indice_pluv
                FROM fato_chuva_cemaden fato
                INNER JOIN dim_tempo t ON t.id_tempo = fato.id_tempo
                INNER JOIN dim_localidade_cemaden l ON l.id = fato.id_localidade
                INNER JOIN dim_fonte f ON f.id = fato.id_fonte
                INNER JOIN dim_estacao e ON e.id = fato.id_estacao
                WHERE t.ano = {year}");

            spark.Stop();

            return response;
        }

        public async Task<List<MeasuredRainfallDTO>> FilterByRainfallIndex(double index)
        {
            SparkSession spark = SparkSession
                .Builder()
                .AppName("sparkQueries")
                .GetOrCreate();

            SetUpSparkTables(spark);

            var response = await ExecuteSparkQuery(spark, 
                @$"SELECT f.fonte, l.municipio, l.UF, t.dia, t.mes, t.ano, t.hora, e.cod_estacao_original, e.nome_estacao_original, indice_pluv
                FROM fato_chuva_cemaden fato
                INNER JOIN dim_tempo t ON t.id_tempo = fato.id_tempo
                INNER JOIN dim_localidade_cemaden l ON l.id = fato.id_localidade
                INNER JOIN dim_fonte f ON f.id = fato.id_fonte
                INNER JOIN dim_estacao e ON e.id = fato.id_estacao
                WHERE indice_pluv > {index}");

            spark.Stop();

            return response;
        }

        public async Task<List<MeasuredRainfallDTO>> FilterByDistance(double distance)
        {
            SparkSession spark = SparkSession
                .Builder()
                .AppName("sparkQueries")
                .GetOrCreate();

            SetUpSparkTables(spark);

            var response = await ExecuteSparkQuery(spark,
                @$"SELECT fonte, municipio, UF, dia, mes, ano, hora, cod_estacao_original, nome_estacao_original, CALC.DISTANCIA, indice_pluv
                FROM (SELECT *, (6371 * acos(cos(radians(-22.913924)) * cos(radians(l.latitude)) * cos(radians(-43.084737) - radians(l.long)) + sin(radians(-22.913924)) * sin(radians(l.latitude)))) AS distancia
                FROM fato_chuva_cemaden fato
                INNER JOIN dim_localidade_cemaden l ON l.id = fato.id_localidade
                INNER JOIN dim_tempo t ON t.id_tempo = fato.id_tempo
                INNER JOIN dim_fonte f ON f.id = fato.id_fonte
                INNER JOIN dim_estacao e ON e.id = fato.id_estacao) AS CALC
                WHERE CALC.DISTANCIA < {distance}");

            spark.Stop();

            return response;
        }

        public async Task<List<MeasuredRainfallDTO>> FilterByDistanceAndRainfallIndex(double distance, double index)
        {
            SparkSession spark = SparkSession
                .Builder()
                .AppName("sparkQueries")
                .GetOrCreate();

            SetUpSparkTables(spark);

            var response = await ExecuteSparkQuery(spark,
                @$"SELECT fonte, municipio, UF, dia, mes, ano, hora, cod_estacao_original, nome_estacao_original, CALC.DISTANCIA, indice_pluv
                FROM (SELECT *, (6371 * acos(cos(radians(-22.913924)) * cos(radians(l.latitude)) * cos(radians(-43.084737) - radians(l.long)) + sin(radians(-22.913924)) * sin(radians(l.latitude)))) AS distancia
                FROM fato_chuva_cemaden fato
                INNER JOIN dim_localidade_cemaden l ON l.id = fato.id_localidade
                INNER JOIN dim_tempo t ON t.id_tempo = fato.id_tempo
                INNER JOIN dim_fonte f ON f.id = fato.id_fonte
                INNER JOIN dim_estacao e ON e.id = fato.id_estacao) AS CALC
                WHERE CALC.DISTANCIA < {distance} AND indice_pluv > {index}");

            spark.Stop();

            return response;
        }

        public async Task<List<MeasuredRainfallDTO>> FilterByDistanceAndDate(double distance, int year, int month, int day)
        {
            SparkSession spark = SparkSession
                .Builder()
                .AppName("sparkQueries")
                .GetOrCreate();

            SetUpSparkTables(spark);

            var response = await ExecuteSparkQuery(spark,
                @$"SELECT fonte, municipio, UF, dia, mes, ano, hora, cod_estacao_original, nome_estacao_original, CALC.DISTANCIA, indice_pluv
                FROM (SELECT *, (6371 * acos(cos(radians(-22.913924)) * cos(radians(l.latitude)) * cos(radians(-43.084737) - radians(l.long)) + sin(radians(-22.913924)) * sin(radians(l.latitude)))) AS distancia
                FROM fato_chuva_cemaden fato
                INNER JOIN dim_localidade_cemaden l ON l.id = fato.id_localidade
                INNER JOIN dim_tempo t ON t.id_tempo = fato.id_tempo
                INNER JOIN dim_fonte f ON f.id = fato.id_fonte
                INNER JOIN dim_estacao e ON e.id = fato.id_estacao) AS CALC
                WHERE CALC.DISTANCIA < {distance} AND dia = {day} AND mes = {month} AND ano = {year}");

            spark.Stop();

            return response;
        }

        public async Task<List<MeasuredRainfallDTO>> FilterByDistanceAndDateRange(DateTime firstDate, DateTime secondDate, double distance)
        {
            SparkSession spark = SparkSession
                .Builder()
                .AppName("sparkQueries")
                .GetOrCreate();

            SetUpSparkTables(spark);

            var response = await ExecuteSparkQuery(spark,
                @$"SELECT fonte, municipio, UF, dia, mes, ano, hora, cod_estacao_original, nome_estacao_original, CALC.DISTANCIA, indice_pluv
                FROM (SELECT *, (6371 * acos(cos(radians(-22.913924)) * cos(radians(l.latitude)) * cos(radians(-43.084737) - radians(l.long)) + sin(radians(-22.913924)) * sin(radians(l.latitude)))) AS distancia
                FROM fato_chuva_cemaden fato
                INNER JOIN dim_localidade_cemaden l ON l.id = fato.id_localidade
                INNER JOIN dim_tempo t ON t.id_tempo = fato.id_tempo
                INNER JOIN dim_fonte f ON f.id = fato.id_fonte
                INNER JOIN dim_estacao e ON e.id = fato.id_estacao) AS CALC
                WHERE CALC.DISTANCIA < {distance}
                AND to_date(ano || '-' || mes || '-' || dia, 'yyyy-M-d') BETWEEN '{firstDate.ToString("yyyy-MM-dd")}' AND '{secondDate.ToString("yyyy-MM-dd")}'");

            spark.Stop();

            return response;
        }

        public async Task<List<MeasuredRainfallDTO>> FilterByDistanceAndCity(double distance, string city, int limit)
        {
            SparkSession spark = SparkSession
                .Builder()
                .AppName("sparkQueries")
                .GetOrCreate();

            SetUpSparkTables(spark);

            var response = await ExecuteSparkQuery(spark,
                @$"SELECT DISTINCT fonte, municipio, UF, cod_estacao_original, nome_estacao_original, CALC.DISTANCIA
                FROM (SELECT *, (6371 * acos(cos(radians(-22.913924)) * cos(radians(l.latitude)) * cos(radians(-43.084737) - radians(l.long)) + sin(radians(-22.913924)) * sin(radians(l.latitude)))) AS distancia
                FROM fato_chuva_cemaden fato
                INNER JOIN dim_localidade_cemaden l ON l.id = fato.id_localidade
                INNER JOIN dim_fonte f ON f.id = fato.id_fonte
                INNER JOIN dim_estacao e ON e.id = fato.id_estacao) AS CALC
                WHERE municipio = '{city}'
                AND calc.distancia > {distance}
                ORDER BY calc.distancia
                LIMIT {limit}");

            spark.Stop();

            return response;
        }

        public async Task<List<MeasuredRainfallDTO>> GetAverageRainfallIndexByCity(string city, int limit)
        {
            SparkSession spark = SparkSession
                .Builder()
                .AppName("sparkQueries")
                .GetOrCreate();

            SetUpSparkTables(spark);

            var response = await ExecuteSparkQuery(spark,
                @$"SELECT DISTINCT fonte, municipio, UF, cod_estacao_original, nome_estacao_original, CALC.DISTANCIA, AVG(indice_pluv) as media
                FROM (SELECT *, (6371 * acos(cos(radians(-22.913924)) * cos(radians(l.latitude)) * cos(radians(-43.084737) - radians(l.long)) + sin(radians(-22.913924)) * sin(radians(l.latitude)))) AS distancia
                FROM fato_chuva_cemaden fato
                INNER JOIN dim_localidade_cemaden l ON l.id = fato.id_localidade
                INNER JOIN dim_tempo t ON t.id_tempo = fato.id_tempo
                INNER JOIN dim_fonte f ON f.id = fato.id_fonte
                INNER JOIN dim_estacao e ON e.id = fato.id_estacao) AS CALC
                WHERE municipio = '{city}'
                GROUP BY fonte, municipio, UF, cod_estacao_original, nome_estacao_original, CALC.DISTANCIA
                ORDER BY calc.distancia
                LIMIT {limit}");

            spark.Stop();

            return response;
        }

        public async Task<List<MeasuredRainfallDTO>> FilterByGeolocationAndCity(string city, double minLatitude, double maxLatitude, double minLongitude, double maxLongitude)
        {
            SparkSession spark = SparkSession
                .Builder()
                .AppName("sparkQueries")
                .GetOrCreate();

            SetUpSparkTables(spark);

            var response = await ExecuteSparkQuery(spark,
                @$"SELECT fonte, municipio, UF, dia, mes, ano, hora, cod_estacao_original, nome_estacao_original, CALC.DISTANCIA, indice_pluv
                FROM (SELECT *, (6371 * acos(cos(radians(-22.913924)) * cos(radians(l.latitude)) * cos(radians(-43.084737) - radians(l.long)) + sin(radians(-22.913924)) * sin(radians(l.latitude)))) AS distancia
                FROM fato_chuva_cemaden fato
                INNER JOIN dim_localidade_cemaden l ON l.id = fato.id_localidade
                INNER JOIN dim_tempo t ON t.id_tempo = fato.id_tempo
                INNER JOIN dim_fonte f ON f.id = fato.id_fonte
                INNER JOIN dim_estacao e ON e.id = fato.id_estacao
                     WHERE NOT ((l.latitude < {minLatitude.ToString(CultureInfo.InvariantCulture)})
                           OR (l.latitude > {maxLatitude.ToString(CultureInfo.InvariantCulture)})
                           OR (l.long < {minLongitude.ToString(CultureInfo.InvariantCulture)})
                           OR (l.long > {maxLongitude.ToString(CultureInfo.InvariantCulture)})
                           )
                     ) AS CALC
                WHERE municipio = '{city}'");

            spark.Stop();

            return response;
        }

        public async Task<List<MeasuredRainfallDTO>> FilterByGeolocationAndDateRange(DateTime firstDate, DateTime secondDate, double minLatitude, double maxLatitude, double minLongitude, double maxLongitude)
        {
            SparkSession spark = SparkSession
                .Builder()
                .AppName("sparkQueries")
                .GetOrCreate();

            SetUpSparkTables(spark);

            var response = await ExecuteSparkQuery(spark,
                @$"SELECT fonte, municipio, UF, dia, mes, ano, hora, cod_estacao_original, nome_estacao_original, CALC.DISTANCIA, indice_pluv
                FROM (SELECT *, (6371 * acos(cos(radians(-22.913924)) * cos(radians(l.latitude)) * cos(radians(-43.084737) - radians(l.long)) + sin(radians(-22.913924)) * sin(radians(l.latitude)))) AS distancia
                FROM fato_chuva_cemaden fato
                INNER JOIN dim_localidade_cemaden l ON l.id = fato.id_localidade
                INNER JOIN dim_tempo t ON t.id_tempo = fato.id_tempo
                INNER JOIN dim_fonte f ON f.id = fato.id_fonte
                INNER JOIN dim_estacao e ON e.id = fato.id_estacao
                     WHERE NOT ((l.latitude < { minLatitude.ToString(CultureInfo.InvariantCulture)})
                           OR (l.latitude > { maxLatitude.ToString(CultureInfo.InvariantCulture)})
                           OR (l.long < { minLongitude.ToString(CultureInfo.InvariantCulture)})
                           OR (l.long > { maxLongitude.ToString(CultureInfo.InvariantCulture)})
                           )
                     ) AS CALC
                WHERE to_date(ano || '-' || mes || '-' || dia, 'yyyy-M-d') BETWEEN '{firstDate.ToString("yyyy-MM-dd")}' AND '{secondDate.ToString("yyyy-MM-dd")}'");

            return response;
        }

        public async Task<List<MeasuredRainfallDTO>> FilterByGeolocationAndRainfallIndex(double index, double minLatitude, double maxLatitude, double minLongitude, double maxLongitude)
        {
            SparkSession spark = SparkSession
                .Builder()
                .AppName("sparkQueries")
                .GetOrCreate();

            SetUpSparkTables(spark);

            var response = await ExecuteSparkQuery(spark,
                @$"SELECT fonte, municipio, UF, dia, mes, ano, hora, cod_estacao_original, nome_estacao_original, CALC.DISTANCIA, indice_pluv
                FROM (SELECT *, (6371 * acos(cos(radians(-22.913924)) * cos(radians(l.latitude)) * cos(radians(-43.084737) - radians(l.long)) + sin(radians(-22.913924)) * sin(radians(l.latitude)))) AS distancia
                FROM fato_chuva_cemaden fato
                INNER JOIN dim_localidade_cemaden l ON l.id = fato.id_localidade
                INNER JOIN dim_tempo t ON t.id_tempo = fato.id_tempo
                INNER JOIN dim_fonte f ON f.id = fato.id_fonte
                INNER JOIN dim_estacao e ON e.id = fato.id_estacao
                     WHERE NOT ((l.latitude < { minLatitude.ToString(CultureInfo.InvariantCulture)})
                           OR (l.latitude > { maxLatitude.ToString(CultureInfo.InvariantCulture)})
                           OR (l.long < { minLongitude.ToString(CultureInfo.InvariantCulture)})
                           OR (l.long > { maxLongitude.ToString(CultureInfo.InvariantCulture)})
                           )
                     ) AS CALC
                WHERE indice_pluv >= {index}");

            return response;
        }


        private void SetUpSparkTables(SparkSession spark)
        {
            CreateTableOnSpark(spark, "dimestacao.csv", "id INT, cod_estacao_original STRING, nome_estacao_original STRING", stationColumnNames, "dim_estacao");
            CreateTableOnSpark(spark, "dimtempo.csv", "id_tempo INT, hora INT, dia INT, semana INT, mes INT, semestre INT, ano INT", timeColumnNames, "dim_tempo");
            CreateTableOnSpark(spark, "dimlocalidadecemaden.csv", "id INT, latitude DOUBLE, long DOUBLE, rua STRING, bairro STRING, municipio STRING, uf STRING", locationColumnNames, "dim_localidade_cemaden");
            CreateTableOnSpark(spark, "dimfonte.csv", "id INT, fonte STRING, url STRING", sourceColumnNames, "dim_fonte");
            CreateTableOnSpark(spark, "fatochuvacemaden.csv", "id_fonte INT, id_tempo INT, id_localidade INT, id_estacao INT, indice_pluv DOUBLE", factColumnNames, "fato_chuva_cemaden");
        }

        private void CreateTableOnSpark(SparkSession spark, string csvTablePath, string schema, string[] stationColumnNames, string tableName)
        {
            //TODO: mudar para variável de ambiente
            var csvPath = @"C:\Users\rodol\OneDrive\Área de Trabalho\Dados\csv";

            var dfStation = spark.Read()
                .Option("header", true)
                .Schema(schema)
                .Csv(Path.Combine(csvPath, csvTablePath));
            dfStation = dfStation.ToDF(stationColumnNames);
            var stationTable = dfStation; //TODO: indexação
            stationTable.CreateOrReplaceTempView(tableName);
        }

        private async Task<List<MeasuredRainfallDTO>> ExecuteSparkQuery(SparkSession spark, string query)
        {
            var result = await Task.Run(() => spark.Sql(query));

            var response = new List<MeasuredRainfallDTO>();

            foreach (var t in result.ToJSON().Collect())
            {
                var jsonString = t.Values[0].ToString();
                response.Add(JsonSerializer.Deserialize<MeasuredRainfallDTO>(jsonString));
            }

            Console.WriteLine(result.Count());

            return response;
        }
    }
}
