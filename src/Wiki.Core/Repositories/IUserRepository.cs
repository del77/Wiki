using System;
using System.Collections.Generic;
using System.Threading.Tasks;
using Wiki.Core.Domain;

namespace Wiki.Core.Repositories
{
    public interface IUserRepository : IRepository
    {
        Task<User> GetAsync(int id);
        Task<User> GetAsync(string email);
        Task<IEnumerable<User>> GetAllAsync();
        Task<IEnumerable<UserPermission>> GetPermissions();
        Task AddAsync(User user);
        Task UpdateAsync(User user);
        Task RemoveAsync(int id);
    }
}
