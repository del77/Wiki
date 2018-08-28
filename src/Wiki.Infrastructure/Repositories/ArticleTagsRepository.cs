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
    public class ArticleTagsRepository : IArticleTagsRepository
    {
        private readonly SqlSettings settings;

        public ArticleTagsRepository(SqlSettings settings)
        {
            this.settings = settings;
        }

        public async Task AddAsync(TextTag tag)
        {
            using (IDbConnection con = new OracleConnection(settings.ConnectionString))
            {
                string query = $"insert into textstags (textid, tagid) values ({tag.TextId}, {tag.Id})";
                await con.QueryAsync(query);
            }
        }

        public async Task<IEnumerable<TextTag>> GetTags()
        {
            using (IDbConnection connection = new OracleConnection(settings.ConnectionString))
            {
                return await connection.QueryAsync<TextTag>("SELECT * from Tags");
            }
        }

        public async Task RemoveAsync(int textid, int tagId)
        {
            using (IDbConnection con = new OracleConnection(settings.ConnectionString))
            {
                string query = $"delete from textstags where textid={textid} and tagid={tagId}";
                await con.QueryAsync(query);
            }
        }
    }
}
