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
                var user = con.Query<User>("SELECT * FROM Users where id = :Id", new { Id = id });
                return user.SingleOrDefault();
            }
        }

        public async Task<User> GetAsync(string email)
        {
            try
            {
                // Please replace the connection string attribute settings
                
        
                using(IDbConnection con = new OracleConnection(settings.ConnectionString))
                {
                    var user = con.Query<User>("SELECT * FROM Users where Email = :Email", new { Email = email });
                    return user.SingleOrDefault();
                }
            }
            catch (Exception ex)
            {
                Console.WriteLine("Error : {0}", ex);
            }
            return null;
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
