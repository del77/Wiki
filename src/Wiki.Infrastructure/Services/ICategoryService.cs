using System;
using System.Collections.Generic;
using System.Text;
using System.Threading.Tasks;
using Wiki.Core.Domain;

namespace Wiki.Infrastructure.Services
{
    public interface ICategoryService : IService
    {
        Task CreateAsync(string categoryName);
        Task<IEnumerable<ArticleCategory>> GetAllAsync();
    }
}
