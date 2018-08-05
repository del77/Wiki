using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using System.Text.Encodings.Web;
using System.Threading.Tasks;
using AutoMapper;
using Wiki.Core.Domain;
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
        public async Task<IEnumerable<ArticleDto>> BrowseAsync(string title)
        {
            //var articles = articleRepository.
            IDictionary<double, Text> texts = new Dictionary<double, Text>();
            texts.Add(1.0, new Text { ArticleId = 1, Status = "Done", Tags = new []{"tag1"}, Title = "tytul"});
            texts.Add(2.0, new Text { ArticleId = 1, Status = "Done", Tags = new[] { "tag2" }, Title = "tytul2" });
            var xd = new List<Article>
            {
                new Article()
                {
                    Id = 1,
                    Category = "Mainc",
                    Master = (Text) texts[1.0],
                    Texts = texts
                }
            };
            var xdd = mapper.Map<ArticleDto>(xd[0]);
            return new [] {xdd};
        }

        public async Task<ArticleDto> GetAsync(int id)
        {
            var article = await articleRepository.GetAsync(id);
            return mapper.Map<ArticleDto>(article);
        }
    }
}
