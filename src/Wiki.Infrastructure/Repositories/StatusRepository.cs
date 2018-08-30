using Dapper;
using Oracle.ManagedDataAccess.Client;
using System;
using System.Collections.Generic;
using System.Data;
using System.Text;
using System.Threading.Tasks;
using Wiki.Core.Domain;
using Wiki.Core.Repositories;
using Wiki.Infrastructure.Settings;

namespace Wiki.Infrastructure.Repositories
{
    public class StatusRepository : IStatusRepository
    {
        private readonly SqlSettings settings;

        public StatusRepository(SqlSettings settings)
        {
            this.settings = settings;
        }

        public async Task<IEnumerable<TextStatus>> GetAllAsync()
        {
            using (IDbConnection connection = new OracleConnection(settings.ConnectionString))
            {
                var query = "Select * from Statuses";
                var statuses = await connection.QueryAsync<TextStatus>(query);

                return statuses;
            }
        }
    }
}
