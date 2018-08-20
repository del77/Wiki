using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;
using Wiki.Infrastructure.DTO;
using Wiki.Infrastructure.Services;
using Wiki.Web.Extensions;
using Wiki.Web.ViewModels;

namespace Wiki.Web.Pages.Articles
{
    public class GetModel : PageModel
    {
        private readonly IArticleService articleService;
        private readonly IHttpContextAccessor httpContextAccessor;

        [BindProperty]
        public ViewModels.Article Article { get; set; }
        [BindProperty]
        public string ContentComparision { get; set; }
        public string TitleComparision { get; set; }

        public GetModel(IArticleService articleService, IHttpContextAccessor httpContextAccessor)
        {
            this.articleService = articleService;
            this.httpContextAccessor = httpContextAccessor;
        }

        public async Task<IActionResult> OnGetAsync(int articleid, int textid)
        {
            var article = await articleService.GetAsync(textid);
            var masterForArticle = (await articleService.BrowseAsync(null, new int[0], 0, 1)).Where(x => x.Id == article.Id).SingleOrDefault();
            ArticleDetailsDto masterForArticleDetails;
            if (masterForArticle.Master != null)
            {
                masterForArticleDetails = await articleService.GetAsync(masterForArticle.Master.Id);

                var diffHelper = new HtmlDiff.HtmlDiff(masterForArticleDetails.Master.Content, article.Master.Content);
                ContentComparision = diffHelper.Build();
                diffHelper = new HtmlDiff.HtmlDiff(masterForArticleDetails.Master.Title, article.Master.Title);
                TitleComparision = diffHelper.Build();
            }
            if (!httpContextAccessor.HttpContext.User.IsInRole("Read") && article.Master.Status.Id != 1)
                return Page();
            //Article = new ViewModels.Article
            //{
            //    ArticleId = article.Id,
            //    TextId = article.Master.Id,
            //    Version = article.Master.Version,
            //    Comment = article.Master.TextComment,
            //    Category = new ViewModels.CategoryFilter
            //    {
            //        Id = article.Category.Id,
            //        Category = article.Category.Category
            //    },
            //    Content = article.Master.Content,
            //    Status = new ViewModels.StatusFilter
            //    {
            //        Id = article.Master.Status.Id,
            //        Status = article.Master.Status.Status
            //    },
            //    Title = article.Master.Title,
            //    Author = new Author
            //    {
            //        Id = article.Master.Author.Id,
            //        Email = article.Master.Author.Email
            //    }
            //};
            //var tags = new List<TagFilter>();
            //foreach (var tag in article.Master.Tags)
            //{
            //    tags.Add(new TagFilter
            //    {
            //        Id = tag.Id,
            //        Tag = tag.Tag
            //    });
            //}
            //Article.Tags = tags;
            Article = CreateArticle(article);
            return Page();
        }



        private Article CreateArticle(ArticleDetailsDto newArt)
        {
            Article article = new ViewModels.Article
            {
                ArticleId = newArt.Id,
                TextId = newArt.Master.Id,
                Version = newArt.Master.Version,
                Comment = newArt.Master.TextComment,
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