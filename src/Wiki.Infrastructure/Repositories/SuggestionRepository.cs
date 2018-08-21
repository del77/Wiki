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
using Wiki.Infrastructure.Extensions;
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
            string q1, q2;
            var paramList = new DynamicParameters();
            paramList.Add("Content", suggestion.Content, direction: ParameterDirection.Input);
            paramList.Add("Served", suggestion.Served, direction: ParameterDirection.Input);
            if (suggestion.Author != null)
                paramList.Add("Authorid", suggestion.Author.Id, direction: ParameterDirection.Input);
            else
                paramList.Add("Authorid", null, direction: ParameterDirection.Input);

            if (suggestion.Text != null)
                paramList.Add("Textid", suggestion.Text.Id, direction: ParameterDirection.Input);
            else
                paramList.Add("Textid", null, direction: ParameterDirection.Input);

            using (IDbConnection connection = new OracleConnection(settings.ConnectionString))
            {
                //var query = $"insert into suggestions (authorid, content, textid) values ({suggestion.Author..NullString()}, '{suggestion.Content}', {suggestion.TextId.NullString()})";
                connection.ExecuteAsync("Insert into Suggestions (authorid, content, textid, served) Values (:Authorid, :Content, :Textid, :Served)", paramList);
                
            }
        }

        public async Task<IEnumerable<Suggestion>> GetAllAsync()
        {
            using (IDbConnection connection = new OracleConnection(settings.ConnectionString))
            {
                string query = "select * from suggestions";
                var suggestions = await connection.QueryAsync<Suggestion>(query);
                foreach(var suggestion in suggestions)
                {
                    var userQuery = $"Select id, email from Users where id = (Select authorid from suggestions where id = {suggestion.Id})";
                    var user = (await connection.QueryAsync<User>(userQuery)).SingleOrDefault();
                    var textsQuery = $"Select Id, Title, Version from Texts where id = (Select textid from suggestions where id = {suggestion.Id})";
                    var text = (await connection.QueryAsync<Text>(textsQuery)).SingleOrDefault();
                    suggestion.SetAuthor(user);
                    suggestion.SetText(text);
                }
                //var articles = await connection.QueryAsync<Article>(articleQuery, new { cat = "%" + selectedCategory + "%" });

                
                return suggestions;

            }
        }

        public async Task<Suggestion> GetAsync(int id)
        {
            using (IDbConnection connection = new OracleConnection(settings.ConnectionString))
            {
                string query = $"select * from suggestions where id = {id}";
                var suggestion = (await connection.QueryAsync<Suggestion>(query)).Single();

                var userQuery = $"Select id, email from Users where id = (Select authorid from suggestions where id = {suggestion.Id})";
                var user = (await connection.QueryAsync<User>(userQuery)).SingleOrDefault();
                var textsQuery = $"Select Id, Title, Version from Texts where id = (Select textid from suggestions where id = {suggestion.Id})";
                var text = (await connection.QueryAsync<Text>(textsQuery)).SingleOrDefault();
                suggestion.SetAuthor(user);
                suggestion.SetText(text);
               

                return suggestion;

            }
        }

        public async Task UpdateAsync(Suggestion suggestion)
        {
            var paramList = new DynamicParameters();
            paramList.Add("Id", suggestion.Id, direction: ParameterDirection.Input);
            paramList.Add("Content", suggestion.Content, direction: ParameterDirection.Input);
            paramList.Add("Served", suggestion.Served, direction: ParameterDirection.Input);
            if (suggestion.Author != null)
                paramList.Add("Authorid", suggestion.Author.Id, direction: ParameterDirection.Input);
            else
                paramList.Add("Authorid", null, direction: ParameterDirection.Input);

            if (suggestion.Text != null)
                paramList.Add("Textid", suggestion.Text.Id, direction: ParameterDirection.Input);
            else
                paramList.Add("Textid", null, direction: ParameterDirection.Input);

            using (IDbConnection connection = new OracleConnection(settings.ConnectionString))
            {
                await connection.ExecuteAsync("update Suggestions set authorid=:Authorid, content=:Content, textid=:Textid, served=:Served where id=:Id", paramList);
            }
        }
    }
}
