using System;
using System.Collections.Generic;
using System.Threading.Tasks;
using Wiki.Infrastructure.DTO;

namespace Wiki.Infrastructure.Services
{
    public interface IArticleService : IService
    {
        Task<ArticleDto> GetAsync(int id);
        Task<IEnumerable<ArticleDto>> BrowseAsync(string title, IEnumerable<string> selectedTags, string selectedCategory);
        Task<IEnumerable<IEnumerable<string>>> GetFilterInfo();
    }
}
