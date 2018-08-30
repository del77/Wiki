using System;
using System.Collections.Generic;
using System.Text;
using System.Threading.Tasks;
using Wiki.Core.Domain;

namespace Wiki.Infrastructure.Services
{
    public interface ITagService : IService
    {
        Task CreateAsync(string tag);
        Task<IEnumerable<TextTag>> GetAllAsync();
    }
}
