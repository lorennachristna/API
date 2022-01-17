using Microsoft.EntityFrameworkCore;
using Pluviometrico.Data.DWModels;

namespace Pluviometrico.Data.DatabaseContext
{
    public class DWContext : DbContext
    {
        protected DWContext()
        {
        }

        public DWContext(DbContextOptions options) : base(options) { }

        public DbSet<FactRain> FactRainList { get; set; }
        public DbSet<DimensionLocation> DimLocationList { get; set; }
        public DbSet<DimensionSource> DimSourceList { get; set; }
        public DbSet<DimensionStation> DimStationList { get; set; }
        public DbSet<DimensionTime> DimTimeList { get; set; }
    }
}
