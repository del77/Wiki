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
        private readonly IUserPermissionRepository userPermissionRepository;
        private readonly IPermissionRepository permissionRepository;

        public UserService(IUserRepository userRepository, IMapper mapper, IEncrypter encrypter, IUserPermissionRepository userPermissionRepository, IPermissionRepository permissionRepository)
        {
            this.userRepository = userRepository;
            this.mapper = mapper;
            this.encrypter = encrypter;
            this.userPermissionRepository = userPermissionRepository;
            this.permissionRepository = permissionRepository;
        }
        public async Task<IEnumerable<UserDto>> BrowseAsync()
        {
            var users = await userRepository.GetAllAsync();
            return mapper.Map<IEnumerable<UserDto>>(users);
        }

        public async Task<UserDto> GetAsync(string email)
        {
            var user = await userRepository.GetAsync(email);
            return mapper.Map<UserDto>(user);
        }

        public async Task<UserDto> GetAsync(int id)
        {
            var user = await userRepository.GetAsync(id);
            return mapper.Map<UserDto>(user);
        }

        public async Task<IEnumerable<UserPermissionDto>> GetPermissionsInfo()
        {
            var permissions = await permissionRepository.GetAllAsync();
            return mapper.Map<IEnumerable<UserPermissionDto>>(permissions);
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
            user = new User(email, hash, salt);
            await userRepository.AddAsync(user);
        }

        public async Task Update(int userId, string email)
        {
            var user = await userRepository.GetAsync(userId);
            user.SetEmail(email);
            await userRepository.UpdateAsync(user);
        }

        public async Task UpdatePermissions(int userId, IEnumerable<int> permissions)
        {
            var user = await userRepository.GetAsync(userId);
            if (user == null)
                throw new Exception("user doesn't exist");
            var permissionsInfo = await permissionRepository.GetAllAsync();
            var tasks = new List<Task>();
            foreach(var permission in permissionsInfo)
            {
                tasks.Add(userPermissionRepository.RemoveAsync(permission.Id, userId));
            }
            await Task.WhenAll(tasks);
            tasks.Clear();
            foreach (var permission in permissions)
            {
                UserPermission up = new UserPermission(permission, userId);
                tasks.Add(userPermissionRepository.AddAsync(up));
            }
            await Task.WhenAll(tasks);
        }
    }
}
