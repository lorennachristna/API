using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Pluviometrico.Core.DTOs
{
    public class MeasuredRainfallDTO
    {
        public string? Source { get; set; }
        public string City { get; set; }
        public string UF { get; set; }
        public int? Day { get; set; }
        public int? Month { get; set; }
        public int? Year { get; set; }
        public int Hour { get; set; }
        public string StationCode { get; set; }
        public string StationName { get; set; }
        public double? Distance { get; set; }
        public double RainfallIndex { get; set; }
    }
}
