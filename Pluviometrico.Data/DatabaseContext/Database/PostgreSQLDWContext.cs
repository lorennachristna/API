using Microsoft.EntityFrameworkCore;
using Pluviometrico.Data.MachineLearningModel;

namespace Pluviometrico.Data.DatabaseContext.Database
{
    public class PostgreSQLDWContext : DWContext
    {
        public PostgreSQLDWContext(DbContextOptions<PostgreSQLDWContext> options) : base(options)
        {
        }

        public DbSet<Excecution> Excecutions { get; set; }

    }
}
