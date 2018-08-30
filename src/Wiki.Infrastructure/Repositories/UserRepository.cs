using System;
using System.Collections.Generic;
using System.Data;
using System.Linq;
using System.Threading.Tasks;
using Dapper;
using Oracle.ManagedDataAccess.Client;
using Wiki.Core.Domain;
using Wiki.Core.Repositories;
using Wiki.Infrastructure.Settings;

namespace Wiki.Infrastructure.Repositories
{
    public class UserRepository : IUserRepository
    {
        private readonly SqlSettings settings;
        public UserRepository(SqlSettings settings)
        {
            this.settings = settings;
        }

        public async Task AddAsync(User user)
        {
            using (IDbConnection con = new OracleConnection(settings.ConnectionString))
            {
                string sql = "INSERT INTO Users (email, password, salt) Values (:email, :password, :salt)";
                await con.ExecuteAsync(sql, new { email = user.Email, password = user.Password, salt = user.Salt });
            }
        }
        public async Task<IEnumerable<User>> GetAllAsync()
        {
            using (IDbConnection con = new OracleConnection(settings.ConnectionString))
            {
                string sql = "SELECT * FROM Users";
                var users = await con.QueryAsync<User>(sql);
                return users;
            }

        }

        public async Task<User> GetAsync(int id)
        {
            using (IDbConnection con = new OracleConnection(settings.ConnectionString))
            {
                var user = (await con.QueryAsync<User>("SELECT * FROM Users where id = :Id", new { Id = id })).Single();
                var permissions = await con.QueryAsync<UserPermission>("Select p.id, permission from permissions p, userpermissions up where p.id = permissionid and userid = :Userid", new { Userid = user.Id });
                user.SetPermissions(new HashSet<UserPermission>(permissions));
                return user;
            }
        }

        public async Task<User> GetAsync(string email)
        {
            using(IDbConnection con = new OracleConnection(settings.ConnectionString))
            {
                var user = (await con.QueryAsync<User>("SELECT * FROM Users where Email = :Email", new { Email = email })).Single();
                var permissions = await con.QueryAsync<UserPermission>("Select p.id, permission from permissions p, userpermissions up where p.id = permissionid and userid = :Userid", new { Userid = user.Id });

                user.SetPermissions(new HashSet<UserPermission>(permissions));
                return user;
            }
        }

        public Task RemoveAsync(int id)
        {
            throw new NotImplementedException();
        }

        public async Task UpdateAsync(User user)
        {
            using (IDbConnection con = new OracleConnection(settings.ConnectionString))
            {
                string sql = "update Users set email=:email, password=:password, salt=:salt where id=:id";
                await con.ExecuteAsync(sql, new { email = user.Email, password = user.Password, salt = user.Salt, id = user.Id });
            }
        }
    }
}
