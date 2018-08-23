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
                var affectedRows = con.Execute(sql, new { email = user.Email, password = user.Password, salt = user.Salt });
                //con.Query<User>("SELECT * FROM Users where Email = :email", new { Email = "user1@email.com" });
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
            try
            {
                // Please replace the connection string attribute settings
                
        
                using(IDbConnection con = new OracleConnection(settings.ConnectionString))
                {
                    var user = (await con.QueryAsync<User>("SELECT * FROM Users where Email = :Email", new { Email = email })).Single();
                    //var permissions = await con.QueryAsync<int>("Select permissionid from userpermissions where userid = :Userid", new { Userid = id });
                    var permissions = await con.QueryAsync<UserPermission>("Select p.id, permission from permissions p, userpermissions up where p.id = permissionid and userid = :Userid", new { Userid = user.Id });
                    //var userPermissions = new HashSet<UserPermission>();
                    //foreach(var permission in permissions)
                    //{
                    //    userPermissions.Add(new UserPermission(permission));
                    //}
                    user.SetPermissions(new HashSet<UserPermission>(permissions));
                    return user;
                }
            }
            catch (Exception ex)
            {
                Console.WriteLine("Error : {0}", ex);
            }
            return null;
        }

        public async Task<IEnumerable<UserPermission>> GetPermissions()
        {
            using (IDbConnection connection = new OracleConnection(settings.ConnectionString))
            {
                return await connection.QueryAsync<UserPermission>("SELECT * from Permissions");
            }
        }

        public Task RemoveAsync(int id)
        {
            throw new NotImplementedException();
        }

        public Task UpdateAsync(User user)
        {
            throw new NotImplementedException();
        }
    }
}
