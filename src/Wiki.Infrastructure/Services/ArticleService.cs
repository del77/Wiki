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



        public async Task<IEnumerable<ArticleDto>> BrowseAsync(string title, IEnumerable<int> selectedTags, int selectedCategory)
        {
            var articles = await articleRepository.GetAllAsync(selectedTags, title, selectedCategory);
            //IDictionary<double, Text> texts = new Dictionary<double, Text>();
            //texts.Add(1.0, new Text { ArticleId = 1, Status = "Done", Tags = new []{"tag1"}, Title = "tytul"});
            //texts.Add(2.0, new Text { ArticleId = 1, Status = "Done", Tags = new[] { "tag2" }, Title = "tytul2" });
            //var xd = new List<Article>
            //{
            //    new Article()
            //    {
            //        Id = 1,
            //        Category = "Mainc",
            //        Master = (Text) texts[1.0],
            //        //Texts = texts
            //    }
            //};
            return mapper.Map<IEnumerable<ArticleDto>>(articles);
        }


        public async Task<ArticleDto> GetAsync(int id)
        {
            var article = await articleRepository.GetAsync(id);
            return mapper.Map<ArticleDto>(article);
        }

        public async Task<FilterInfo> GetFilterInfo()
        {
            var categories = await articleRepository.GetCategories();
            var tags = await articleRepository.GetTags();
            var statuses = await articleRepository.GetStatuses();
            var filter = new FilterInfo();
            filter.Categories = mapper.Map<IEnumerable<ArticleCategoryDto>>(categories);
            filter.Tags = mapper.Map<IEnumerable<TextTagDto>>(tags);
            filter.Statuses = mapper.Map<IEnumerable<TextStatusDto>>(statuses);
            return filter;
        }

        public async Task AddAsync(string title, string content, string[] selectedTags, string selectedCategory)
        {
            var article = new Article(content, selectedCategory);
            var text = new Text(title, selectedTags);
            await articleRepository.AddAsync(article, text);
        }

    }
}
