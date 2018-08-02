using System;
using System.Collections.Generic;
using System.Data;
using System.Threading.Tasks;
using Dapper;
using Oracle.ManagedDataAccess.Client;
using Wiki.Core.Domain;
using Wiki.Core.Repositories;
using Wiki.Infrastructure.Settings;

namespace Wiki.Infrastructure.Repositories
{
    public class OracleUserRepository : IUserRepository
    {
        private readonly SqlSettings settings;
        public OracleUserRepository(SqlSettings settings)
        {
            this.settings = settings;
        }
        public Task AddAsync(User user)
        {
            throw new NotImplementedException();
        }

        public Task<IEnumerable<User>> GetAllAsync()
        {
            throw new NotImplementedException();
        }

        public async Task<User> GetAsync(int id)
        {


            return null;
        }

        public Task<User> GetAsync(string email)
        {
            return null;
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
                var xd = con.Query<User>("SELECT * FROM Users where Email = :email", new {Email = "user1@email.com"});
                //OracleDataReader reader = cmd.ExecuteReader();
                //Console.WriteLine(reader.GetString(0));
                
                Console.WriteLine("Press RETURN to exit.");
                //Console.ReadLine();
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
