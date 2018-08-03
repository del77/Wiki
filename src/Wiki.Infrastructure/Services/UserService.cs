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
        private readonly IEncrypter encrypter;

        public UserService(IUserRepository userRepository, IMapper mapper, IEncrypter encrypter)
        {
            this.userRepository = userRepository;
            this.mapper = mapper;
            this.encrypter = encrypter;
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

        

        public async Task LoginAsync(string email, string password)
        {
            var user = await userRepository.GetAsync(email);
            if (user == null)
                throw new Exception("Invalid email or password" );
            var hash = encrypter.GetHash(password, user.Salt);
            if (user.Password == hash)
                return;

            throw new Exception("Invalid email or password");
        }

        public async Task RegisterAsync(string email, string password)
        {
            var user = await userRepository.GetAsync(email);
            if (user != null)
                throw new Exception($"User with email: {email} already exists.");

            var salt = encrypter.GetSalt();
            var hash = encrypter.GetHash(password, salt);
            user = new User(1, email, hash, salt);
            await userRepository.AddAsync(user);
        }
    }
}
