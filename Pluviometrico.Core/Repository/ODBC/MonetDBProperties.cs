using Microsoft.Extensions.Configuration;

namespace Pluviometrico.Core.Repository.ODBC
{
    public class MonetDBProperties : ODBCProperties
    {
        public MonetDBProperties(IConfiguration configuration) : base()
        {
            Schema = "";
            ConnectionString = configuration.GetConnectionString("sqlMonetDB");
            DistanceCalculation = "(6371 * acos(cos(radians(-22.913924)) * cos(radians(l.latitude)) * cos(radians(-43.084737) - radians(l.long)) + sin(radians(-22.913924)) * sin(radians(l.latitude))))";
        }
    }
}
