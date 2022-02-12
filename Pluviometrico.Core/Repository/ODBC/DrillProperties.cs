using Microsoft.Extensions.Configuration;

namespace Pluviometrico.Core.Repository.ODBC
{
    public class DrillProperties : ODBCProperties
    {
        public DrillProperties(IConfiguration configuration) : base()
        {
            Schema = "dfs.tmp.";
            ConnectionString = configuration.GetConnectionString("sqlApacheDrill");
            DistanceCalculation = "(6371 * acos(cos(radians(cast(-22.913924 as float))) * cos(radians(cast(l.latitude as float))) * cos(radians(cast(-43.084737 as float)) - radians(cast(l.long as float))) + sin(radians(cast(-22.913924 as float))) * sin(radians(cast(l.latitude as float)))))";
        }
    }
}
