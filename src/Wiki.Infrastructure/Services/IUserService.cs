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
        Task LoginAsync(string email, string password);
        Task RegisterAsync(string email, string password);
    }
}
