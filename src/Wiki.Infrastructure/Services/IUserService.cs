using System;
using System.Collections.Generic;
using System.Threading.Tasks;
using Wiki.Infrastructure.DTO;

namespace Wiki.Infrastructure.Services
{
    public interface IUserService : IService
    {
        Task<UserDto> GetAsync(string email);
        Task<UserDto> GetAsync(int id);
        Task<IEnumerable<UserDto>> BrowseAsync();
        Task<IEnumerable<UserPermissionDto>> GetPermissionsInfo();
        Task LoginAsync(string email, string password);
        Task RegisterAsync(string email, string password);
        Task UpdatePermissions(int userId, IEnumerable<int> permissions);
        Task Update(int userId, string email);
    }
}
