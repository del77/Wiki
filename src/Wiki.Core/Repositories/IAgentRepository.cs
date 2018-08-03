using System;
using System.Collections.Generic;
using System.Text;
using System.Threading.Tasks;
using Wiki.Core.Domain;

namespace Wiki.Core.Repositories
{
    public interface IAgentRepository : IRepository
    {
        Task<Agent> GetAsync(int id);
        
        //Task<IEnumerable<Agent>> GetAllAsync();
        //Task AddAsync(Agent user);
        //Task UpdateAsync(Agent user);
        //Task RemoveAsync(int id);
    }
}
