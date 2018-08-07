using System;
using System.Collections.Generic;
using System.Data;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Dapper;
using Dapper.Contrib.Extensions;
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

        public async Task AddAsync(Article article, Text text)
        {
            using (IDbConnection connection = new OracleConnection(settings.ConnectionString))
            {
                var param = new DynamicParameters();
                //param.Add("")
            }
        }

        public async Task<IEnumerable<Article>> GetAllAsync(IEnumerable<string> selectedTags, string title, string selectedCategory)
        {
            if (title == null)
                title = "";
            if (selectedCategory == null)
                selectedCategory = "";

            using (IDbConnection connection = new OracleConnection(settings.ConnectionString))
            {
                var articleQuery = "SELECT a.ID FROM Articles a";
                var articles = await connection.QueryAsync<Article>(articleQuery);
                //var articles = await connection.QueryAsync<Article>(articleQuery, new { cat = "%" + selectedCategory + "%" });

                foreach (var article in articles)
                {
                    var category = await connection.QueryAsync<ArticleCategory>("Select * From Categories where ID = (Select categoryid from articles where id = :artid)", new { artid = article.Id });
                    article.Category = category.Single();

                    var textsQuery = "Select t.Id, Title from Texts t, Statuses s where t.statusid = s.id and ArticleID = :artid and t.Title like :tit";
                    var texts = connection.Query<Text>(textsQuery, new { artid = article.Id, tit = "%" + title + "%" });
                    foreach (var text in texts)
                    {
                        var status = (await connection.QueryAsync<TextStatus>("Select * from Statuses s where id = :txid", new { txid = text.Id })).Single();
                        var tags = await connection.QueryAsync<TextTag>("Select t.id, tag from textstags tt, tags t where t.ID = tt.TAGID and textid = :txid", new { txid = text.Id });
                        text.Tags = tags;
                        text.Status = status;
                    }
                    article.Texts = texts;
                    article.Master = article.Texts.Where(x => x.Status.Status == "Master").SingleOrDefault();
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

        public async Task<IEnumerable<ArticleCategory>> GetCategories()
        {
            using (IDbConnection connection = new OracleConnection(settings.ConnectionString))
            {
                return await connection.QueryAsync<ArticleCategory>("SELECT * from Categories");   
            }
        }

        public async Task<IEnumerable<TextStatus>> GetStatuses()
        {
            using (IDbConnection connection = new OracleConnection(settings.ConnectionString))
            {
                return await connection.QueryAsync<TextStatus>("SELECT * from Statuses");
            }
        }

        public async Task<IEnumerable<TextTag>> GetTags()
        {
            using (IDbConnection connection = new OracleConnection(settings.ConnectionString))
            {
                return await connection.QueryAsync<TextTag>("SELECT * from Tags");
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
