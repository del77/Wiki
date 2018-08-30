using System;
using System.Collections.Generic;
using System.Text;
using System.Threading.Tasks;
using Wiki.Core.Domain;

namespace Wiki.Core.Repositories
{
    public interface IPermissionRepository : IRepository
    {
        Task<IEnumerable<UserPermission>> GetAllAsync();
    }
}
