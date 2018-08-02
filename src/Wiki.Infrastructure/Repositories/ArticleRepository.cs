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
    public class ArticleRepository : IArticleRepository
    {
        private readonly SqlSettings settings;
        
        public ArticleRepository(SqlSettings settings)
        {
            this.settings = settings;
        }

        public async Task AddAsync(Article article)
        {
            throw new NotImplementedException();
        }

        public async Task<IEnumerable<string>> GetAllTitlesAsync()
        {
            throw new NotImplementedException();
        }

        public async Task<Article> GetAsync(int id)
        {
            using(IDbConnection connection = new OracleConnection(settings.ConnectionString))
                {
                connection.Open();
                
                var xd = connection.Query<Article>("SELECT ID, Title, Category FROM Articles where Id = :id2", new { id2 = id });
                //OracleDataReader reader = cmd.ExecuteReader();
                //Console.WriteLine(reader.GetString(0));
                
                return xd.SingleOrDefault();
                //Console.ReadLine();
                }
        }

        public async Task RemoveAsync(int id)
        {
            throw new NotImplementedException();
        }

        public async Task UpdateAsync(Article article)
        {
            throw new NotImplementedException();
        }
    }
}
