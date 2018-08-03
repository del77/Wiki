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
    public class AgentRepository : IAgentRepository
    {
        private readonly SqlSettings settings;

        public AgentRepository(SqlSettings settings)
        {
            this.settings = settings;
        }

        public async Task<Agent> GetAsync(int id)
        {
            using (IDbConnection con = new OracleConnection(settings.ConnectionString))
            {
                con.Open();
                var response = await con.QuerySingleOrDefaultAsync<Agent>("SELECT * FROM Agents where userId = :ID", new { ID = id});
                if (response != null)
                {
                    var xd = con.Query("Select Permission From Agentspermissions where agentid = ID", new { ID = id });
                }
                //OracleDataReader reader = cmd.ExecuteReader();
                //Console.WriteLine(reader.GetString(0));

                Console.WriteLine("Press RETURN to exit.");
                //Console.ReadLine();
                return response;
            }
        }
    }
}
