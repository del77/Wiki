using System;
using System.Collections.Generic;
using System.Text;
using System.Threading.Tasks;
using Wiki.Core.Domain;

namespace Wiki.Infrastructure.Services
{
    public interface IStatusService : IService
    {
        Task<IEnumerable<TextStatus>> GetAllAsync();
    }
}
