using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Wiki.Core.Domain;
using Wiki.Core.Repositories;

namespace Wiki.Infrastructure.Repositories
{
    public class UserRepository_memory// : IUserRepository
    {
        private static readonly ISet<User> users = new HashSet<User>
        {
            //new User(1, "user1@email.com", "secret", "salt"),
            //new User(2, "user2@email.com", "secret", "salt")
        };

        public async Task AddAsync(User user)
        {
            throw new NotImplementedException();
        }

        public async Task<IEnumerable<User>> GetAllAsync()
        {
            throw new NotImplementedException();
        }

        public async Task<User> GetAsync(int id)
        {
            return await Task.FromResult(users.SingleOrDefault(x => x.Id == id));
        }

        public async Task<User> GetAsync(string email)
        {
            //return await Task.FromResult(users.SingleOrDefault(x => x.Email == email));
            return null;
        }

        public async Task RemoveAsync(int id)
        {
            throw new NotImplementedException();
        }

        public async Task UpdateAsync(User user)
        {
            throw new NotImplementedException();
        }
    }
}
