using System;
using System.Collections.Generic;
using System.Text;
using System.Threading.Tasks;

namespace Wiki.Infrastructure.Services
{
    public interface ISuggestionService : IService
    {
        Task AddAsync(int? authorId, int? textId, string content);
    }
}
