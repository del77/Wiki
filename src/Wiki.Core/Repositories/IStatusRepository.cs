using System;
using System.Collections.Generic;
using System.Text;
using System.Threading.Tasks;
using Wiki.Core.Domain;

namespace Wiki.Core.Repositories
{
    public interface IStatusRepository : IRepository
    {
        Task<IEnumerable<TextStatus>> GetAllAsync();
    }
}
