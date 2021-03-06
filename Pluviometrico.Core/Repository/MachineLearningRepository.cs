using Pluviometrico.Data.DatabaseContext.Database;
using System.Linq;

namespace Pluviometrico.Core.Repository
{
    public class MachineLearningRepository
    {
        private readonly PostgreSQLDWContext _context;
        
        public MachineLearningRepository(PostgreSQLDWContext context)
        {
            _context = context;
        }

        public DatabaseType GetBestPerformingDatabase(int queryNumber)
        {
            var database = _context.Excecutions
                .Where(e => e.QueryNumber == queryNumber)
                .GroupBy(e => new { e.DBMS })
                .Select(g => new
                {
                    g.Key.DBMS,
                    averageExecutionTime = g.Average(e => e.ExecutionTime)
                }).OrderBy(g => g.averageExecutionTime).FirstOrDefault().DBMS;

            DatabaseType type;

            switch (database)
            {
                case ("ElasticSearch"):
                    type = DatabaseType.ElasticSearch;
                    break;
                case ("PostgreSQL"):
                    type = DatabaseType.PostgreSQL;
                    break;
                case ("MonetDB"):
                    type = DatabaseType.MonetDB;
                    break;
                case ("Apache Drill"):
                    type = DatabaseType.ApacheDrill;
                    break;
                case ("Apache Spark"):
                    type = DatabaseType.ApacheSpark;
                    break;
            }

            return DatabaseType.ApacheSpark;
        }

    }
}
