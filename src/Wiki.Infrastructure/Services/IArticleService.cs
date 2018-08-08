using System;
using System.Collections.Generic;
using System.Threading.Tasks;
using Wiki.Infrastructure.DTO;

namespace Wiki.Infrastructure.Services
{
    public interface IArticleService : IService
    {
        Task<ArticleDto> GetAsync(int id);
        Task<IEnumerable<ArticleDto>> BrowseAsync(string title, IEnumerable<int> selectedTags, int selectedCategory);
        Task<FilterInfo> GetFilterInfo();
        Task AddAsync(string title, string content, string[] selectedTags, string selectedCategory);
    }
}
