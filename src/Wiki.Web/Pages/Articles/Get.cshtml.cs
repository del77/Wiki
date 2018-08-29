using System;
using System.Collections.Generic;
using System.Linq;
using System.Security.Claims;
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
        private readonly ISuggestionService suggestionService;
        private readonly IHttpContextAccessor httpContextAccessor;
        public readonly ClaimsPrincipal User;
        public readonly int UserId;
        [BindProperty]
        public ViewModels.Article Article { get; set; }
        [BindProperty]
        public string ContentComparision { get; set; }
        [BindProperty]
        public List<Article> OtherVersions { get; set; }
        public string TitleComparision { get; set; }

        [BindProperty]
        public Suggestion Suggestion { get; set; }

        public GetModel(IArticleService articleService, ISuggestionService suggestionService, IHttpContextAccessor httpContextAccessor)
        {
            this.articleService = articleService;
            this.suggestionService = suggestionService;
            this.httpContextAccessor = httpContextAccessor;
            User = httpContextAccessor.HttpContext.User;
            var claims = (httpContextAccessor.HttpContext.User.Claims.FirstOrDefault(x => x.Type == "http://schemas.xmlsoap.org/ws/2005/05/identity/claims/nameidentifier"));
            if (claims != null)
                UserId = Convert.ToInt32(claims.Value);
        }

        public async Task<IActionResult> OnGetAsync(int articleid, int textid)
        {
            var article = await articleService.GetAsync(textid);
            var masterForArticle = (await articleService.BrowseAsync(1, null, article.Id, null)).SingleOrDefault();
            ArticleDetailsDto masterForArticleDetails;
            if (masterForArticle.Master != null)
            {
                masterForArticleDetails = await articleService.GetAsync(masterForArticle.Master.Id);

                var diffHelper = new HtmlDiff.HtmlDiff(masterForArticleDetails.Master.Content, article.Master.Content);
                ContentComparision = diffHelper.Build();
                diffHelper = new HtmlDiff.HtmlDiff(masterForArticleDetails.Master.Title, article.Master.Title);
                TitleComparision = diffHelper.Build();
            }
            if (!httpContextAccessor.HttpContext.User.IsInRole("Read") && article.Master.Status.Id != 1 && article.Master.Author.Id != UserId)
                return Page();


            Article = CreateArticle(article);

            var otherVersions = await articleService.BrowseAsync(null, null, article.Id, null);
            OtherVersions = new List<ViewModels.Article>();
            foreach (var item in otherVersions)
            {
                foreach (var text in item.Texts)
                {
                    var art = new Article();
                    art.Title = text.Title;
                    art.Category = new CategoryFilter
                    {
                        Id = item.Category.Id,
                        Category = item.Category.Category
                    };
                    var tags = new List<TagFilter>();
                    foreach (var tag in text.Tags)
                    {
                        tags.Add(new TagFilter
                        {
                            Id = tag.Id,
                            Tag = tag.Tag,
                            Checked = true
                        });
                    }
                    art.ArticleId = item.Id;
                    art.TextId = text.Id;
                    art.Tags = tags;
                    art.Version = text.Version;
                    art.Status = new StatusFilter
                    {
                        Id = text.Status.Id,
                        Status = text.Status.Status,
                        Selected = false
                    };



                    OtherVersions.Add(art);
                }
            }
            return Page();
        }

        public async Task OnPostAsync()
        {
            int? user = Suggestion.IsAnonymous ? (int?)null : Convert.ToInt32(User.Claims.FirstOrDefault(x => x.Type == "http://schemas.xmlsoap.org/ws/2005/05/identity/claims/nameidentifier").Value);
            await suggestionService.AddAsync(user, Article.TextId, Suggestion.Content);
            await OnGetAsync(Article.ArticleId, Article.TextId);
        }



        private Article CreateArticle(ArticleDetailsDto newArt)
        {
            Article article = new ViewModels.Article
            {
                ArticleId = newArt.Id,
                TextId = newArt.Master.Id,
                Version = newArt.Master.Version,
                Comment = newArt.Master.TextComment,
                CreatedAt = newArt.Master.CreatedAt,
                Avatar = String.Format("data:image/gif;base64,{0}", Convert.ToBase64String(newArt.Master.Avatar)),
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
            if (newArt.Master.Supervisor != null)
                article.Supervisor = new User
                {
                    Id = newArt.Master.Supervisor.Id,
                    Email = newArt.Master.Supervisor.Email
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