using System;
using System.Collections.Generic;
using System.Threading.Tasks;
using Wiki.Core.Domain;

namespace Wiki.Core.Repositories
{
    public interface IArticleRepository : IRepository
    {
        Task<Article> GetAsync(int id);
        Task<IEnumerable<Article>> GetAllAsync(IEnumerable<string> selectedTags, string title, string selectedCategory);
        Task AddAsync(Article article, Text text);
        Task UpdateAsync(Article article);
        Task RemoveAsync(int id);
        Task<IEnumerable<ArticleCategory>> GetCategories();
        Task<IEnumerable<TextTag>> GetTags();
        Task<IEnumerable<TextStatus>> GetStatuses();
    }
}
