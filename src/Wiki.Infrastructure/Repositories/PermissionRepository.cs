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
    public class PermissionRepository : IPermissionRepository
    {
        private readonly SqlSettings settings;

        public PermissionRepository(SqlSettings settings)
        {
            this.settings = settings;
        }

        public async Task<IEnumerable<UserPermission>> GetAllAsync()
        {
            using (IDbConnection connection = new OracleConnection(settings.ConnectionString))
            {
                var query = "Select * from Permissions";
                var tags = await connection.QueryAsync<UserPermission>(query);

                return tags;
            }
        }
    }
}
