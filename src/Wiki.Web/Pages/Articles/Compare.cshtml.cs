using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using DiffMatchPatch;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;
using Wiki.Infrastructure.DTO;
using Wiki.Infrastructure.Services;
using Wiki.Web.ViewModels;

namespace Wiki.Web.Pages.Articles
{
    public class CompareModel : PageModel
    {
        private readonly IArticleService articleService;

        public CompareModel(IArticleService articleService)
        {
            this.articleService = articleService;
        }
        public Article Article1 { get; set; }
        public Article Article2 { get; set; }
        //public Article Article1 { get; set; }
        //public Article Article2 { get; set; }
        public List<Diff> diff { get; set; }

        public async Task OnGet(int articleid, int textid)
        {
            var article1 = await articleService.GetAsync(textid);
            var master = (await articleService.BrowseAsync(null, new int[0], 0, 1)).Where(x => x.Id==article1.Id).SingleOrDefault();
            var article2 = await articleService.GetAsync(master.Master.Id);
            //Article1 = CreateArticle(article1);
            //Article2 = CreateArticle(article2);
            
            var diffHelper = new HtmlDiff.HtmlDiff(article2.Master.Content, article1.Master.Content);
            article1.Master.Content = diffHelper.Build();

            //dmp.diff_cleanupSemantic(diff);
            Article1 = CreateArticle(article1);
            Article2 = CreateArticle(article2);

        }

        private Article CreateArticle(ArticleDetailsDto newArt)
        {
            Article article = new ViewModels.Article
            {
                ArticleId = newArt.Id,
                TextId = newArt.Master.Id,
                Category = new ViewModels.CategoryFilter
                {
                    Id = newArt.Category.Id,
                    Category = newArt.Category.Category
                },
                Content = newArt.Master.Content,
                Status = new ViewModels.StatusFilter
                {
                    Id = newArt.Master.Status.Id,
                    Status = newArt.Master.Status.Status
                },
                Title = newArt.Master.Title,
                Author = new Author
                {
                    Id = newArt.Master.Author.Id,
                    Email = newArt.Master.Author.Email
                }
            };
            var tags = new List<TagFilter>();
            foreach (var tag in newArt.Master.Tags)
            {
                tags.Add(new TagFilter
                {
                    Id = tag.Id,
                    Tag = tag.Tag
                });
            }
            article.Tags = tags;
            return article;
        }
    }
}