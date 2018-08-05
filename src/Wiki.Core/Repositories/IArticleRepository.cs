using System;
using System.Collections.Generic;
using System.Threading.Tasks;
using Wiki.Core.Domain;

namespace Wiki.Core.Repositories
{
    public interface IArticleRepository : IRepository
    {
        Task<Article> GetAsync(int id);
        Task<IEnumerable<Article>> GetAllAsync();
        Task AddAsync(Article article);
        Task UpdateAsync(Article article);
        Task RemoveAsync(int id);
    }
}
