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

        public async Task AddAsync(Article article)
        {
            using (IDbConnection connection = new OracleConnection(settings.ConnectionString))
            {
                //// art
                var paramList = new DynamicParameters();
                int articleId;
                if (article.Id == 0)
                {
                    paramList.Add("CategoryID", article.Category.Id, direction: ParameterDirection.Input);
                    paramList.Add("Id", DbType.Int32, direction: ParameterDirection.Output);

                    connection.Execute("Insert into Articles (CategoryID) Values (:CategoryID) returning Id into :Id", paramList);
                    articleId = paramList.Get<int>("Id");
                }
                else
                {
                    articleId = article.Id;
                }
                //// art

                //// text
                paramList = new DynamicParameters();
                paramList.Add("ArticleID", articleId, direction: ParameterDirection.Input);
                paramList.Add("AuthorID", 3, direction: ParameterDirection.Input);
                paramList.Add("StatusID", article.Master.Status.Id, direction: ParameterDirection.Input);
                paramList.Add("Content", article.Master.Content, direction: ParameterDirection.Input);
                paramList.Add("Title", article.Master.Title, direction: ParameterDirection.Input);
                paramList.Add("Version", article.Master.Version, direction: ParameterDirection.Input);
                paramList.Add("Id", DbType.Int32, direction: ParameterDirection.Output);
                connection.Execute("Insert into Texts (articleid, authorid, statusid, content, title, version) Values (:ArticleID, :AuthorID, :StatusID, :Content, :Title, :Version) returning Id into :Id", paramList);
                var textId = paramList.Get<int>("Id");
                //// text

                //// textstags
                foreach (var tag in article.Master.Tags)
                {
                    await connection.QueryAsync("insert into textstags values (:textid, :tagid)", new { textid = textId, tagid = tag.Id });
                }
                //// textstags
            }
        }

        public async Task<IEnumerable<Article>> GetAllAsync(IEnumerable<int> selectedTags, string title, int selectedCategory, int selectedStatus)
        {
            if (title == null)
                title = "";

            var articleQuery = "SELECT a.ID FROM Articles a";
            if (selectedCategory != 0)
                articleQuery += $" where categoryid={selectedCategory}";

            using (IDbConnection connection = new OracleConnection(settings.ConnectionString))
            {
                var articles = await connection.QueryAsync<Article>(articleQuery);
                //var articles = await connection.QueryAsync<Article>(articleQuery, new { cat = "%" + selectedCategory + "%" });

                foreach (var article in articles)
                {
                    var category = await connection.QueryAsync<ArticleCategory>("Select * From Categories where ID = (Select categoryid from articles where id = :artid)", new { artid = article.Id });
                    article.Category = category.Single();


                    var textsQuery = $"Select Id, Title from Texts where ArticleID = :artid and Title like :tit";
                    if (selectedStatus != 0)
                        textsQuery += $" and statusid={selectedStatus}";

                    var tagsCount = selectedTags.Count();
                    if (tagsCount > 0)
                    {
                        StringBuilder tagsString = new StringBuilder();
                        for (int i = 0; i < tagsCount; i++)
                        {
                            tagsString.Append(selectedTags.ElementAt(i));
                            if (i != tagsCount - 1)
                                tagsString.Append(", ");
                        }
                        textsQuery += $" and Id in (Select textid from textstags where tagid in ({tagsString}) group by textid having count(tagid) = {tagsCount})";
                    }
                    //var textsQuery = "Select t.Id, Title from Texts t, Statuses s where t.statusid = s.id and ArticleID = :artid and t.Title like :tit";
                    
                    var texts = connection.Query<Text>(textsQuery, new { artid = article.Id, tit = "%" + title + "%" });

                    var tagsQuery = "Select * from textstags tt, tags t where t.ID = tt.TAGID and textid = :txid";
                    foreach (var text in texts)
                    {
                        var status = (await connection.QueryAsync<TextStatus>($"Select * from Statuses s where id = (Select statusid from texts where id={text.Id})")).Single();
                        var tags = await connection.QueryAsync<TextTag>(tagsQuery, new { txid = text.Id });
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


        public async Task<Article> GetAsync(int textid)
        {
                using(IDbConnection connection = new OracleConnection(settings.ConnectionString))
                {

                var textsQuery = $"Select Id, Articleid, Title, Content, Version from Texts where id = {textid}";
                var text = (await connection.QueryAsync<Text>(textsQuery)).Single();
                var article = new Article(text.ArticleId);
                var categoryQuery = $"Select * From Categories where ID = (Select categoryid from articles where id = {article.Id})";
                var category = (await connection.QueryAsync<ArticleCategory>(categoryQuery)).Single();
                var userQuery = $"Select id, email from Users where id = (Select authorid from texts where id = {text.Id})";
                var user = (await connection.QueryAsync<User>(userQuery)).Single();
                var statusQuery = $"Select * from statuses where id = (Select statusid from texts where id = {text.Id})";
                var status = (await connection.QueryAsync<TextStatus>(statusQuery)).Single();
                var tagsQuery = $"Select id, tag from textstags tt, tags t where t.ID = tt.TAGID and textid = {text.Id}";
                var tags = await connection.QueryAsync<TextTag>(tagsQuery);

                text.SetAuthor(user);
                text.SetStatus(status);
                text.SetTags(tags);
                article.SetCategory(category);
                article.SetMaster(text);
                //OracleDataReader reader = cmd.ExecuteReader();
                //Console.WriteLine(reader.GetString(0));

                return article;
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

        public async Task UpdateAsync(int textid, int status)
        {
            using (IDbConnection connection = new OracleConnection(settings.ConnectionString))
            {
                var query = $"Update texts set statusid={status} where id={textid}";
                await connection.QueryAsync(query);
            }
        }
    }
}
