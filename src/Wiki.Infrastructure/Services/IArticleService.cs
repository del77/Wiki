using System;
using System.Collections.Generic;
using System.Threading.Tasks;
using Wiki.Infrastructure.DTO;

namespace Wiki.Infrastructure.Services
{
    public interface IArticleService : IService
    {
        Task<ArticleDetailsDto> GetAsync(int articleid, int textid);
        Task<IEnumerable<ArticleDto>> BrowseAsync(string title, IEnumerable<int> selectedTags, int selectedCategory, int selectedStatus);
        Task<FilterInfo> GetFilterInfo();
        Task AddAsync(string title, string content, int[] selectedTags, int selectedCategory, int author);
        Task ChangeStatus(int textid, int status);
    }
}
