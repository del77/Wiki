using System;
using System.Collections.Generic;
using System.Data;
using System.Linq;
using System.Security.Claims;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;
using Wiki.Infrastructure.Commands;
using Wiki.Infrastructure.DTO;
using Wiki.Infrastructure.Services;
using Wiki.Web.ViewModels;

namespace Wiki.Web.Pages
{
    public class IndexModel : PageModel
    { 
        readonly IUserService userService;
        IArticleService articleService;
        private readonly ISuggestionService suggestionService;

        public readonly ClaimsPrincipal User_;

        [BindProperty]
        public Suggestion Suggestion { get; set; }

        [BindProperty]
        public List<Article> Articles { get; set; }

        public IndexModel(IUserService userService, IArticleService articleService, ISuggestionService suggestionService, IHttpContextAccessor httpContextAccessor)
        {
            //this.commandDispatcher = commandDispatcher;    
            this.userService = userService;
            this.articleService = articleService;
            this.suggestionService = suggestionService;
            User_ = httpContextAccessor.HttpContext.User;
        }
       

        public async Task OnGetAsync()
        {
            var articles = (await articleService.BrowseAsync(1, null, null, null)).Where(x => x.Master != null);
            var rnd = new Random();
            var randomNumbers = Enumerable.Range(0, articles.Count()).OrderBy(x => rnd.Next()).Take(4).ToList();
            Articles = new List<Article>();
            foreach(var listid in randomNumbers)
            {
                var article = await articleService.GetAsync(articles.ElementAt(listid).Master.Id);
                var itemlist = new Article
                {
                    TextId = article.Master.Id,
                    Title = article.Master.Title
                };
                if (article.Master.Avatar.Length != 0)
                {
                    itemlist.Avatar = String.Format("data:image/gif;base64,{0}", Convert.ToBase64String(article.Master.Avatar));
                }
                else
                {
                    itemlist.Avatar = String.Format("https://www.unesale.com/ProductImages/Large/notfound.png");
                }
                Articles.Add(itemlist);
            }
        }

        public async Task<IActionResult> OnPostAsync()
        {
            int? user = Suggestion.IsAnonymous ? (int?)null : Convert.ToInt32(User_.Claims.FirstOrDefault(x => x.Type == "http://schemas.xmlsoap.org/ws/2005/05/identity/claims/nameidentifier").Value);
            await suggestionService.AddAsync(user, null, Suggestion.Content);
            return RedirectToPage("Index");
        }
    }
}
