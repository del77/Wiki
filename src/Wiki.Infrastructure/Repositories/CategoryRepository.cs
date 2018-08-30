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
    public class CategoryRepository : ICategoryRepository
    {
        private readonly SqlSettings settings;

        public CategoryRepository(SqlSettings settings)
        {
            this.settings = settings;
        }

        public async Task AddAsync(ArticleCategory articleCategory)
        {
            using (IDbConnection connection = new OracleConnection(settings.ConnectionString))
            {
                var query = "Insert into Categories (category) values (:Category)";
                await connection.QueryAsync(query, new { Category = articleCategory.Category });
            }
        }

        public async Task<IEnumerable<ArticleCategory>> GetAllAsync()
        {
            using (IDbConnection connection = new OracleConnection(settings.ConnectionString))
            {
                var query = "Select * from Categories";
                var categories = await connection.QueryAsync<ArticleCategory>(query);

                return categories;
            }
        }
    }
}
