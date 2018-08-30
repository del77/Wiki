using System;
using System.Collections.Generic;
using System.Text;
using System.Threading.Tasks;
using Wiki.Core.Domain;

namespace Wiki.Core.Repositories
{
    public interface ICategoryRepository : IRepository
    {
        Task<IEnumerable<ArticleCategory>> GetAllAsync();
        Task AddAsync(ArticleCategory articleCategory);
    }
}
