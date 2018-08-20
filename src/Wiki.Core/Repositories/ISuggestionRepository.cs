using System;
using System.Collections.Generic;
using System.Text;
using System.Threading.Tasks;
using Wiki.Core.Domain;

namespace Wiki.Core.Repositories
{
    public interface ISuggestionRepository : IRepository
    {
        Task AddAsync(Suggestion suggestion);
    }
}
