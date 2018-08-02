using System;
using System.Collections.Generic;
using System.Threading.Tasks;
using AutoMapper;
using Wiki.Core.Repositories;
using Wiki.Infrastructure.DTO;

namespace Wiki.Infrastructure.Services
{
    public class ArticleService : IArticleService
    {
        private readonly IArticleRepository articleRepository;
        private readonly IMapper mapper;
        public ArticleService(IArticleRepository articleRepository, IMapper mapper)
        {
            this.mapper = mapper;
            this.articleRepository = articleRepository;
        }
        public async Task<IEnumerable<ArticleDto>> BrowseTitlesAsync()
        {
            throw new NotImplementedException();
        }

        public async Task<ArticleDto> GetAsync(int id)
        {
            var article = await articleRepository.GetAsync(id);
            return mapper.Map<ArticleDto>(article);
        }
    }
}
