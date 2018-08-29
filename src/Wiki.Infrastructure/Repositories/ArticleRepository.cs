using System;
using System.Collections.Generic;
using System.Data;
using System.IO;
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
                paramList.Add("AuthorID", article.Master.Author.Id, direction: ParameterDirection.Input);
                paramList.Add("StatusID", article.Master.Status.Id, direction: ParameterDirection.Input);
                paramList.Add("Content", article.Master.Content, direction: ParameterDirection.Input);
                paramList.Add("Title", article.Master.Title, direction: ParameterDirection.Input);
                paramList.Add("Version", article.Master.Version, direction: ParameterDirection.Input);
                paramList.Add("Id", DbType.Int32, direction: ParameterDirection.Output);
                paramList.Add("CreatedAt", article.Master.CreatedAt, direction: ParameterDirection.Input);
                if(article.Master.Avatar == null)
                    paramList.Add("Avatar", null, direction: ParameterDirection.Input);
                else
                    paramList.Add("Avatar", article.Master.Avatar, direction: ParameterDirection.Input);
                connection.Execute("Insert into Texts (articleid, authorid, statusid, content, title, version, createdat, avatar) Values (:ArticleID, :AuthorID, :StatusID, :Content, :Title, :Version, :CreatedAt, :Avatar) returning Id into :Id", paramList);
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

        public async Task<IEnumerable<Article>> GetAllAsync(int? selectedStatus, int? selectedUser, int? selectedArticle, int? selectedSupervisor)
        {
            var articleQuery = "SELECT a.ID FROM Articles a";
            if(selectedArticle != null)
                articleQuery += $" where a.ID={selectedArticle}";

            using (IDbConnection connection = new OracleConnection(settings.ConnectionString))
            {
                var articles = await connection.QueryAsync<Article>(articleQuery);
                //var articles = await connection.QueryAsync<Article>(articleQuery, new { cat = "%" + selectedCategory + "%" });

                foreach (var article in articles)
                {
                    var category = await connection.QueryAsync<ArticleCategory>("Select * From Categories where ID = (Select categoryid from articles where id = :artid)", new { artid = article.Id });
                    article.Category = category.Single();

                    var textsQuery = $"Select Id, Title, Version from Texts where ArticleID = :artid";
                    if (selectedStatus != null)
                        textsQuery += $" and statusid={selectedStatus}";
                    if(selectedUser!=null)
                        textsQuery += $" and authorid={selectedUser}";
                    if(selectedSupervisor!=null)
                        textsQuery += $" and supervisorid={selectedSupervisor}";


                    //var textsQuery = "Select t.Id, Title from Texts t, Statuses s where t.statusid = s.id and ArticleID = :artid and t.Title like :tit";

                    var texts = connection.Query<Text>(textsQuery, new { artid = article.Id });

                    var tagsQuery = "Select * from textstags tt, tags t where t.ID = tt.TAGID and textid = :txid";
                    foreach (var text in texts)
                    {
                        var status = (await connection.QueryAsync<TextStatus>($"Select * from Statuses s where id = (Select statusid from texts where id={text.Id})")).Single();
                        var tags = await connection.QueryAsync<TextTag>(tagsQuery, new { txid = text.Id });
                        var userQuery = $"Select id, email from Users where id = (Select authorid from texts where id = {text.Id})";
                        var user = (await connection.QueryAsync<User>(userQuery)).Single();
                        var supervisorQuery = $"Select id, email from Users where id = (Select supervisorid from texts where id = {text.Id})";
                        var supervisor = (await connection.QueryAsync<User>(supervisorQuery)).SingleOrDefault();
                        text.Author = user;
                        text.Tags = tags;
                        text.Status = status;
                        text.SetSupervisor(supervisor);
                    }
                    article.Texts = texts;
                    article.Master = article.Texts.Where(x => x.Status.Status == "Master").SingleOrDefault();
                }

                return articles;
            }
        }


        public async Task<Article> GetAsync(int textid)
        {
            using(IDbConnection connection = new OracleConnection(settings.ConnectionString))
            {

            var textsQuery = $"Select Id, Articleid, Title, Content, Version, CreatedAt, textComment, Avatar from Texts where id = {textid}";
            var text = (await connection.QueryAsync<Text>(textsQuery)).Single();
            var article = new Article(text.ArticleId);
            var categoryQuery = $"Select * From Categories where ID = (Select categoryid from articles where id = {article.Id})";
            var category = (await connection.QueryAsync<ArticleCategory>(categoryQuery)).Single();
            var userQuery = $"Select id, email from Users where id = (Select authorid from texts where id = {text.Id})";
            var user = (await connection.QueryAsync<User>(userQuery)).Single();
            var supervisorQuery = $"Select id, email from Users where id = (Select supervisorid from texts where id = {text.Id})";
            var supervisor = (await connection.QueryAsync<User>(supervisorQuery)).SingleOrDefault();
            var statusQuery = $"Select * from statuses where id = (Select statusid from texts where id = {text.Id})";
            var status = (await connection.QueryAsync<TextStatus>(statusQuery)).Single();
            var tagsQuery = $"Select id, tag from textstags tt, tags t where t.ID = tt.TAGID and textid = {text.Id}";
            var tags = await connection.QueryAsync<TextTag>(tagsQuery);
            

            text.SetAuthor(user);
            text.SetStatus(status);
            text.SetTags(tags);
            text.SetSupervisor(supervisor);
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
            using (IDbConnection connection = new OracleConnection(settings.ConnectionString))
            {
                var paramList = new DynamicParameters();
                paramList.Add("Id", article.Master.Id, direction: ParameterDirection.Input);
                paramList.Add("ArticleID", article.Id, direction: ParameterDirection.Input);
                paramList.Add("AuthorID", article.Master.Author.Id, direction: ParameterDirection.Input);
                paramList.Add("StatusID", article.Master.Status.Id, direction: ParameterDirection.Input);
                paramList.Add("Content", article.Master.Content, direction: ParameterDirection.Input);
                paramList.Add("Title", article.Master.Title, direction: ParameterDirection.Input);
                paramList.Add("Version", article.Master.Version, direction: ParameterDirection.Input);
                if(article.Master.TextComment == null)
                    paramList.Add("TextComment", null, direction: ParameterDirection.Input);
                else
                    paramList.Add("TextComment", article.Master.TextComment, direction: ParameterDirection.Input);

                if (article.Master.Supervisor == null)
                    paramList.Add("SupervisorID", null, direction: ParameterDirection.Input);
                else
                    paramList.Add("SupervisorID", article.Master.Supervisor.Id, direction: ParameterDirection.Input);
                paramList.Add("CreatedAt", article.Master.CreatedAt, direction: ParameterDirection.Input);
                //connection.Execute("Insert into Texts (articleid, authorid, statusid, content, title, version, createdat) Values (:ArticleID, :AuthorID, :StatusID, :Content, :Title, :Version, :CreatedAt) returning Id into :Id", paramList);

                
                string query = $"Update texts set articleid=:ArticleID, authorid=:AuthorID, statusid=:StatusID, content=:Content, title=:Title, version=:Version, textcomment=:TextComment, createdat=:CreatedAt, supervisorid=:SupervisorId where id=:Id";
                Task task = connection.ExecuteAsync(query, paramList);

                paramList = new DynamicParameters();
                paramList.Add("Id", article.Master.Id, direction: ParameterDirection.Input);
                paramList.Add("CategoryId", article.Category.Id, direction: ParameterDirection.Input);
                query = $"Update articles set categoryid=:CategoryId where id=:Id";
                await connection.ExecuteAsync(query, paramList);
                await task;
            }
        }

        public async Task UpdateAsync(int textid, int status, string comment="")
        {
            using (IDbConnection connection = new OracleConnection(settings.ConnectionString))
            {
                string query;
                if(comment == "")
                    query = $"Update texts set statusid={status} where id={textid}";
                else
                    query = $"Update texts set statusid={status}, textcomment='{comment}' where id={textid}";
                await connection.QueryAsync(query);
            }
        }
    }
}
