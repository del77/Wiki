using Dapper;
using Oracle.ManagedDataAccess.Client;
using System;
using System.Collections.Generic;
using System.Data;
using System.Linq;
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
                var agent = await con.QuerySingleOrDefaultAsync<Agent>("SELECT * FROM Agents where userId = :ID", new { ID = id});
                if (agent != null)
                {
                    var permissions = con.Query<string>("Select PERMISSION From Agentspermissions a, permissions p where p.id = a.permissionid and a.agentid = :ID", new { ID = id });
                    agent.Permissions = permissions;
                }
                //OracleDataReader reader = cmd.ExecuteReader();

                Console.WriteLine("Press RETURN to exit.");
                //Console.ReadLine();
                return agent;
            }
        }
    }
}
