using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Wiki.Core.Domain;
using Wiki.Core.Repositories;

namespace Wiki.Infrastructure.Repositories
{
    public class UserRepository : IUserRepository
    {
        private static readonly ISet<User> users = new HashSet<User>
        {
            new User(Guid.NewGuid(), "user1@email.com", "secret", "salt"),
            new User(Guid.NewGuid(), "user2@email.com", "secret", "salt")
        };

        public async Task AddAsync(User user)
        {
            throw new NotImplementedException();
        }

        public async Task<IEnumerable<User>> GetAllAsync()
        {
            throw new NotImplementedException();
        }

        public async Task<User> GetAsync(Guid id)
        {
            return await Task.FromResult(users.SingleOrDefault(x => x.Id == id));
        }

        public async Task<User> GetAsync(string email)
        {
            return await Task.FromResult(users.SingleOrDefault(x => x.Email == email));
        }

        public async Task RemoveAsync(Guid id)
        {
            throw new NotImplementedException();
        }

        public async Task UpdateAsync(User user)
        {
            throw new NotImplementedException();
        }
    }
}
