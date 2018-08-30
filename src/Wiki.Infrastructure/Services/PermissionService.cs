using AutoMapper;
using System;
using System.Collections.Generic;
using System.Text;
using System.Threading.Tasks;
using Wiki.Core.Repositories;
using Wiki.Infrastructure.DTO;

namespace Wiki.Infrastructure.Services
{
    public class PermissionService : IPermissionService
    {
        private readonly IPermissionRepository permissionRepository;
        private readonly IMapper mapper;

        public PermissionService(IPermissionRepository permissionRepository, IMapper mapper)
        {
            this.permissionRepository = permissionRepository;
            this.mapper = mapper;
        }

        public async Task<IEnumerable<UserPermissionDto>> BrowseAsync()
        {
            var permissions = await permissionRepository.GetAllAsync();
            return mapper.Map<IEnumerable<UserPermissionDto>>(permissions);
        }

        
    }
}
