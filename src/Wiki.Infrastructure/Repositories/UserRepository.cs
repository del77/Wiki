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
                con.Open();
                string sql = "INSERT INTO Users (email, password, salt) Values (:email, :password, :salt)";
                var affectedRows = con.Execute(sql, new { email = user.Email, password = user.Password, salt = user.Salt });
                //con.Query<User>("SELECT * FROM Users where Email = :email", new { Email = "user1@email.com" });
            }
        }
        public Task<IEnumerable<User>> GetAllAsync()
        {
            throw new NotImplementedException();
        }

        public async Task<User> GetAsync(int id)
        {


            return null;
        }

        public async Task<User> GetAsync(string email)
        {
            try
            {
                // Please replace the connection string attribute settings
                
        
                using(IDbConnection con = new OracleConnection(settings.ConnectionString))
                {
                con.Open();
                //Console.WriteLine("Connected to Oracle Database {0}", con.ServerVersion);

                //var orderDetail = con.Query<User>("Select * from TABLE1", null);
                //var newuser = new User(Guid.NewGuid(), "user2@email.com", "secret", "salt");
                //con.Execute("Insert into Users (Id, Email, Password, Salt) Values (:id, :email, :pass, :salt)", new {id = newuser.Id.ToByteArray(), email = newuser.Email, pass = newuser.Password, salt = "asssdd" });
                var xd = con.Query<User>("SELECT * FROM Users where Email = :Email", new { Email = email });
                //OracleDataReader reader = cmd.ExecuteReader();
                //Console.WriteLine(reader.GetString(0));
                
                Console.WriteLine("Press RETURN to exit.");
                    //Console.ReadLine();
                 return xd.SingleOrDefault();
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
