using Pluviometrico.Data.DatabaseContext.Database;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

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
            }

            return DatabaseType.PostgreSQL;
        }

    }
}
