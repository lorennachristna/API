using Microsoft.EntityFrameworkCore;

namespace Pluviometrico.Data.DatabaseContext
{
    public class PostgreSQLContext : DbContext
    {
        public PostgreSQLContext()
        {
        }

        public PostgreSQLContext(DbContextOptions<PostgreSQLContext> options) : base(options)
        {
        }

        public DbSet<MeasuredRainfall> MeasuredRainfallList { get; set; }
    }
}
