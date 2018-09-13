using System;
using System.Collections.Generic;
using System.Text;
using System.Threading.Tasks;
using Wiki.Infrastructure.DTO;

namespace Wiki.Infrastructure.Services
{
    public interface IPermissionService : IService
    {
        Task<IEnumerable<UserPermissionDto>> BrowseAsync();
    }
}
