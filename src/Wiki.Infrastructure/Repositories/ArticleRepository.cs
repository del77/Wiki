using System;
using System.Collections.Generic;
using System.Data;
using System.Linq;
using System.Text;
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

        public async Task<IEnumerable<Article>> GetAllAsync(IEnumerable<string> selectedTags, string title, string selectedCategory)
        {
            if (title == null)
                title = "";
            if (selectedCategory == null)
                selectedCategory = "";

            using (IDbConnection connection = new OracleConnection(settings.ConnectionString))
            {
                var articleQuery = "SELECT a.ID, Category FROM Articles a, Categories c where a.categoryid = c.id and c.Category like :cat";
                var articles = await connection.QueryAsync<Article>(articleQuery, new { cat = "%" + selectedCategory + "%" });
                foreach(var article in articles)
                {
                    var textsQuery = "Select t.Id, Status, Title from Texts t, Statuses s where t.statusid = s.id and ArticleID = :artid and t.Title like :tit";
                    var texts = connection.Query<Text>(textsQuery, new { artid = article.Id, tit = "%" + title + "%" });
                    foreach (var text in texts)
                    {
                        var tags = await connection.QueryAsync<string>("Select tag from textstags tt, tags t where t.ID = tt.TAGID and textid = :txid", new { txid = text.Id });
                        text.Tags = tags;
                    }
                    article.Texts = texts;
                    article.Master = article.Texts.Where(x => x.Status == "Master").SingleOrDefault();
                }
                //var xd = connection.Query<Article>("SELECT ID, Title, Category FROM Articles where Id = :id2", new { id2 = id });
                //OracleDataReader reader = cmd.ExecuteReader();
                //Console.WriteLine(reader.GetString(0));

                //return xd.SingleOrDefault();
                return articles;
                //Console.ReadLine();
            }
        }


        public async Task<Article> GetAsync(int id)
        {
                using(IDbConnection connection = new OracleConnection(settings.ConnectionString))
                {
                
                
                var xd = connection.Query<Article>("SELECT ID, Title, Category FROM Articles where Id = :id2", new { id2 = id });
                //OracleDataReader reader = cmd.ExecuteReader();
                //Console.WriteLine(reader.GetString(0));
                
                return xd.SingleOrDefault();
                //Console.ReadLine();
                }
        }

        public async Task<IEnumerable<IEnumerable<string>>> GetFilterInfo()
        {
            using (IDbConnection connection = new OracleConnection(settings.ConnectionString))
            {
                
                var categories = await connection.QueryAsync<string>("SELECT CATEGORY from Categories");
                var tags = await connection.QueryAsync<string>("SELECT TAG from Tags");
                var statuses = await connection.QueryAsync<string>("SELECT STATUS from Statuses");
                var filterInfo = new List<List<string>>
                {
                    new List<string>(categories),
                    new List<string>(tags),
                    new List<string>(statuses)
                };

                return filterInfo;
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
