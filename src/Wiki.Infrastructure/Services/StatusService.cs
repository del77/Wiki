using AutoMapper;
using System;
using System.Collections.Generic;
using System.Text;
using System.Threading.Tasks;
using Wiki.Core.Domain;
using Wiki.Core.Repositories;

namespace Wiki.Infrastructure.Services
{
    public class StatusService : IStatusService
    {
        private readonly IStatusRepository statusRepository;
        private readonly IMapper mapper;

        public StatusService(IStatusRepository statusRepository, IMapper mapper)
        {
            this.statusRepository = statusRepository;
            this.mapper = mapper;
        }

        public async Task<IEnumerable<TextStatus>> GetAllAsync()
        {
            var statuses = await statusRepository.GetAllAsync();
            return mapper.Map<IEnumerable<TextStatus>>(statuses);
        }
    }
}
