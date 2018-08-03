using AutoMapper;
using System;
using System.Collections.Generic;
using System.Text;
using System.Threading.Tasks;
using Wiki.Core.Repositories;
using Wiki.Infrastructure.DTO;

namespace Wiki.Infrastructure.Services
{
    public class AgentService : IAgentService
    {
        private readonly IAgentRepository agentRepository;
        private readonly IMapper mapper;

        public AgentService(IAgentRepository agentRepository, IMapper mapper)
        {
            this.agentRepository = agentRepository;
            this.mapper = mapper;
        }

        public async Task<AgentDto> GetAsync(int id)
        {
            var agent = await agentRepository.GetAsync(id);
            return mapper.Map<AgentDto>(agent);
        }
    }
}
