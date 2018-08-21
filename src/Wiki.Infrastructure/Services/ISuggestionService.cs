using System;
using System.Collections.Generic;
using System.Text;
using System.Threading.Tasks;
using Wiki.Infrastructure.DTO;

namespace Wiki.Infrastructure.Services
{
    public interface ISuggestionService : IService
    {
        Task AddAsync(int? authorId, int? textId, string content);
        Task<IEnumerable<SuggestionDto>> BrowseAsync();
        Task<SuggestionDto> GetAsync(int id);
        Task MakeServed(int id);
    }
}
