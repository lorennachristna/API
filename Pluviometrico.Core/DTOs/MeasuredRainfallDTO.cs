using System;

namespace Pluviometrico.Core.DTOs
{
    public class MeasuredRainfallDTO : IEquatable<MeasuredRainfallDTO>
    {
        public string Source { get; set; }
        public string City { get; set; }
        public string UF { get; set; }
        public int? Day { get; set; }
        public int? Month { get; set; }
        public int? Year { get; set; }
        public int? Hour { get; set; }
        public string StationCode { get; set; }
        public string StationName { get; set; }
        public double? Distance { get; set; }
        public double? RainfallIndex { get; set; }
        public double? AverageRainfallIndex { get; set; }
        public DateTime? Date { get; set; }


        public bool Equals(MeasuredRainfallDTO other)
        {

            //Check whether the compared object is null.
            if (Object.ReferenceEquals(other, null)) return false;

            //Check whether the compared object references the same data.
            if (Object.ReferenceEquals(this, other)) return true;

            //Check whether the products' properties are equal.
            return Source.Equals(other.Source) &&
                City.Equals(other.City) &&
                Day.Equals(other.Day) &&
                Month.Equals(other.Month) &&
                Year.Equals(other.Year) &&
                Hour.Equals(other.Hour) &&
                StationCode.Equals(other.StationCode) &&
                StationName.Equals(other.StationName) &&
                Distance.Equals(other.Distance) &&
                RainfallIndex.Equals(other.RainfallIndex) &&
                AverageRainfallIndex.Equals(other.AverageRainfallIndex) &&
                Date.Equals(other.Date);
        }

        public override int GetHashCode()
        {
            int hashSource = Source == null ? 0 : Source.GetHashCode();
            int hashCity = City == null ? 0 : City.GetHashCode();
            int hashDay = Day == null ? 0 : Day.GetHashCode();
            int hashMonth = Month == null ? 0 : Month.GetHashCode();
            int hashYear = Year == null ? 0 : Year.GetHashCode();
            int hashHour = Hour == null ? 0 : Hour.GetHashCode();
            int hashStationCode = StationCode == null ? 0 : StationCode.GetHashCode();
            int hashStationName = StationName == null ? 0 : StationName.GetHashCode();
            int hashDistance = Distance == null ? 0 : Distance.GetHashCode();
            int hashRainfallIndex = RainfallIndex == null ? 0 : RainfallIndex.GetHashCode();
            int hashAverageRainfallIndex = AverageRainfallIndex == null ? 0 : AverageRainfallIndex.GetHashCode();
            int hashDate = Date == null ? 0 : Date.GetHashCode();

            return hashSource ^ hashCity ^ hashDay ^ hashMonth ^ hashYear ^ hashHour ^ hashStationCode ^ hashStationName ^ hashDistance ^ hashRainfallIndex ^ hashAverageRainfallIndex ^ hashDate;
        }




    }
}
