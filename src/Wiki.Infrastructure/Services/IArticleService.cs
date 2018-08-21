using System;
using System.Collections.Generic;
using System.Threading.Tasks;
using Wiki.Infrastructure.DTO;

namespace Wiki.Infrastructure.Services
{
    public interface IArticleService : IService
    {
        Task<ArticleDetailsDto> GetAsync(int textid);
        Task<IEnumerable<ArticleDto>> BrowseAsync(string title, IEnumerable<int> selectedTags, int selectedCategory, int selectedStatus, int selectedArticle=0);
        Task<FilterInfo> GetFilterInfo();
        Task AddAsync(int articleId, string title, string content, int[] selectedTags, int selectedCategory, int author, double Version);
        Task ChangeStatus(int textid, int status, string comment="");
    }
}
