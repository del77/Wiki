using System;
using System.Collections.Generic;
using System.Threading.Tasks;
using Wiki.Infrastructure.DTO;

namespace Wiki.Infrastructure.Services
{
    public interface IArticleService : IService
    {
        Task<ArticleDetailsDto> GetAsync(int textid);
        Task<IEnumerable<ArticleDto>> BrowseAsync(int? selectedStatus, int? selectedUser, int? selectedArticle, int? selectedSupervisor);
        Task<FilterInfo> GetFilterInfo();
        Task AddAsync(int articleId, string title, string content, int status, int[] selectedTags, int selectedCategory, int author, double version, byte[] image);
        Task ChangeStatus(int textid, int status, string comment="");
        Task UpdateVersion(int articleId, int textId, string title, string content, int status, int[] selectedTags, int selectedCategory, int author, double version, string textcomment);
        Task SetSupervisor(int textid, int userId);
    }
}
