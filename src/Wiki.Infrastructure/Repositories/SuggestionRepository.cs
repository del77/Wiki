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
    public class SuggestionRepository : ISuggestionRepository
    {
        private readonly SqlSettings settings;

        public SuggestionRepository(SqlSettings settings)
        {
            this.settings = settings;
        }

        public async Task AddAsync(Suggestion suggestion)
        {
            using (IDbConnection connection = new OracleConnection(settings.ConnectionString))
            {
                var query = $"insert into suggestions (authorid, content, textid) values ({suggestion.AuthorId}, {suggestion.TextId}, {suggestion.Content})";
                await connection.QueryAsync(query);
            }
        }
    }
}
