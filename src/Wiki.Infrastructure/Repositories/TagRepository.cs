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
    public class TagRepository : ITagRepository
    {
        private readonly SqlSettings settings;

        public TagRepository(SqlSettings settings)
        {
            this.settings = settings;
        }
        public async Task AddAsync(TextTag textTag)
        {
            using (IDbConnection connection = new OracleConnection(settings.ConnectionString))
            {
                var query = "Insert into Tags (tag) values (:Tag)";
                await connection.QueryAsync(query, new { Tag = textTag.Tag });
            }
        }

        public async Task<IEnumerable<TextTag>> GetAllAsync()
        {
            using (IDbConnection connection = new OracleConnection(settings.ConnectionString))
            {
                var query = "Select * from Tags";
                var tags = await connection.QueryAsync<TextTag>(query);

                return tags;
            }
        }
    }
}
