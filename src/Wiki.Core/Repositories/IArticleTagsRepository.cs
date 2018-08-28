using System;
using System.Collections.Generic;
using System.Text;
using System.Threading.Tasks;
using Wiki.Core.Domain;

namespace Wiki.Core.Repositories
{
    public interface IArticleTagsRepository : IRepository
    {
        Task AddAsync(TextTag tag);
        Task RemoveAsync(int textid, int tagId);
        Task<IEnumerable<TextTag>> GetTags();
    }
}
