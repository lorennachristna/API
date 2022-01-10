using Pluviometrico.Core.DTOs;
using Pluviometrico.Data;
using System;

namespace Pluviometrico.Core
{
    public static class Utils
    {
        public static (DateTime greaterDate, DateTime lesserDate) MaxMinDate(DateTime firstDate, DateTime secondDate)
        {
            DateTime greaterDate;
            DateTime lesserDate;

            if (firstDate > secondDate)
            {
                greaterDate = firstDate;
                lesserDate = secondDate;
            }
            else
            {
                greaterDate = secondDate;
                lesserDate = firstDate;
            }
            return (greaterDate, lesserDate);
        }

        public static object FormattedResponse(MeasuredRainfall source, double? distance)
        {
            var result = new MeasuredRainfallDTO
            {
                City = source.Municipio,
                UF = source.UF,
                Day = source.Dia,
                Month = source.Mes,
                Year = source.Ano,
                Hour = source.Hora,
                StationCode = source.CodEstacaoOriginal,
                StationName = source.NomeEstacaoOriginal,
                Distance = distance,
                RainfallIndex = source.ValorMedida
            };
            return (object)result;
        }
    }
}
