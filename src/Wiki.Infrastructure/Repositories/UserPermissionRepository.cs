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
    public class UserPermissionRepository : IUserPermissionRepository
    {
        private readonly SqlSettings settings;
        public UserPermissionRepository(SqlSettings settings)
        {
            this.settings = settings;
        }

        public async Task AddAsync(UserPermission userPermission)
        {
            using (IDbConnection con = new OracleConnection(settings.ConnectionString))
            {
                string query = $"insert into userpermissions (permissionid, userid) values ({userPermission.Id}, {userPermission.UserId})";
                await con.QueryAsync(query);

            }
        }

        public async Task RemoveAsync(int permissionId, int userId)
        {
            using (IDbConnection con = new OracleConnection(settings.ConnectionString))
            {
                string query = $"delete from userpermissions where permissionid={permissionId} and userid={userId}";
                await con.QueryAsync(query);
            }
        }

        public async Task Update(UserPermission userPermission)
        {
            using (IDbConnection con = new OracleConnection(settings.ConnectionString))
            {
                string query = $"select count(*) from userpermissions where userid={userPermission.UserId} and permissionid={userPermission.Id}";
                var hasPermission = await con.QueryAsync<int>(query);

            }
        }
    }
}
