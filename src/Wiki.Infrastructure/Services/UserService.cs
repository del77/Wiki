using System;
using System.Collections.Generic;
using System.Threading.Tasks;
using AutoMapper;
using Wiki.Core.Domain;
using Wiki.Core.Repositories;
using Wiki.Infrastructure.DTO;

namespace Wiki.Infrastructure.Services
{
    public class UserService : IUserService
    {
        private readonly IUserRepository userRepository;
        private readonly IMapper mapper;
        public UserService(IUserRepository userRepository, IMapper mapper)
        {
            this.userRepository = userRepository;
            this.mapper = mapper;
        }
        public Task<IEnumerable<UserDto>> BrowseAsync()
        {
            throw new NotImplementedException();
        }

        public async Task<UserDto> GetAsync(string email)
        {
            var user = await userRepository.GetAsync(email);
            return mapper.Map<UserDto>(user);
        }

        public Task LoginAsync(string email, string password)
        {
            throw new NotImplementedException();
        }

        public Task RegisterAsync(Guid userId, string email, string password)
        {
            throw new NotImplementedException();
        }
    }
}
