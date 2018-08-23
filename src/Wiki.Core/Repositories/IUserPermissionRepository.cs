using System;
using System.Collections.Generic;
using System.Text;
using System.Threading.Tasks;
using Wiki.Core.Domain;

namespace Wiki.Core.Repositories
{
    public interface IUserPermissionRepository : IRepository
    {
        Task Update(UserPermission userPermission);
        Task AddAsync(UserPermission userPermission);
        Task RemoveAsync(int permissionId, int userId);
    }
}
