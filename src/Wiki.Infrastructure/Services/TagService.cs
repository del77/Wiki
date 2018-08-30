using AutoMapper;
using System;
using System.Collections.Generic;
using System.Text;
using System.Threading.Tasks;
using Wiki.Core.Domain;
using Wiki.Core.Repositories;

namespace Wiki.Infrastructure.Services
{
    public class TagService : ITagService
    {
        private readonly ITagRepository tagRepository;
        private readonly IMapper mapper;

        public TagService(ITagRepository tagRepository, IMapper mapper)
        {
            this.tagRepository = tagRepository;
            this.mapper = mapper;
        }

        public async Task<IEnumerable<TextTag>> GetAllAsync()
        {
            var tags = await tagRepository.GetAllAsync();
            return mapper.Map<IEnumerable<TextTag>>(tags);
        }

        public async Task CreateAsync(string tagName)
        {
            var tag = new TextTag(tagName);
            await tagRepository.AddAsync(tag);
        }
    }
}
