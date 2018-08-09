using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;
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

        public GetModel(IArticleService articleService, IHttpContextAccessor httpContextAccessor)
        {
            this.articleService = articleService;
            this.httpContextAccessor = httpContextAccessor;
        }

        public async Task<IActionResult> OnGet(int articleid, int textid)
        {
            var article = await articleService.GetAsync(articleid, textid);
            if (!httpContextAccessor.HttpContext.User.IsInRole("Read") && article.Master.Status.Id != 1)
                return Page();
            Article = new ViewModels.Article
            {
                ArticleId = article.Id,
                TextId = article.Master.Id,
                Category = new ViewModels.CategoryFilter
                {
                    Id = article.Category.Id,
                    Category = article.Category.Category
                },
                Content = article.Master.Content,
                Status = new ViewModels.StatusFilter
                {
                    Id = article.Master.Status.Id,
                    Status = article.Master.Status.Status
                },
                Title = article.Master.Title,
                Author = new Author
                {
                    Id = article.Master.Author.Id,
                    Email = article.Master.Author.Email
                }
            };
            var tags = new List<TagFilter>();
            foreach(var tag in article.Master.Tags)
            {
                tags.Add(new TagFilter
                {
                    Id = tag.Id,
                    Tag = tag.Tag
                });
            }
            Article.Tags = tags;
            return Page();
        }
    }
}