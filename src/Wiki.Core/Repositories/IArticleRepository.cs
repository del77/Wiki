using System;
using System.Collections.Generic;
using System.Threading.Tasks;
using Wiki.Core.Domain;

namespace Wiki.Core.Repositories
{
    public interface IArticleRepository : IRepository
    {
        Task<Article> GetAsync(int textid);
        Task<IEnumerable<Article>> GetAllAsync(int? selectedStatus, int? selectedUser, int? selectedArticle);
        Task AddAsync(Article article);
        Task UpdateAsync(Article article);
        Task RemoveAsync(int id);
        Task<IEnumerable<ArticleCategory>> GetCategories();
        Task<IEnumerable<TextTag>> GetTags();
        Task<IEnumerable<TextStatus>> GetStatuses();
        Task UpdateAsync(int textid, int status, string comment);
    }
}
