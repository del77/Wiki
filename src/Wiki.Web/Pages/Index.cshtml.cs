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

        //ICommandDispatcher commandDispatcher; dodac do konstruktora
        IUserService userService;
        IArticleService articleService;
        private readonly ISuggestionService suggestionService;

        public readonly ClaimsPrincipal User;

        [BindProperty]
        public Suggestion Suggestion { get; set; }
        public IndexModel(IUserService userService, IArticleService articleService, ISuggestionService suggestionService, IHttpContextAccessor httpContextAccessor)
        {
            //this.commandDispatcher = commandDispatcher;    
            this.userService = userService;
            this.articleService = articleService;
            this.suggestionService = suggestionService;
            User = httpContextAccessor.HttpContext.User;
        }
       

        public async Task OnGet()
        {
            var xd = new HttpContextAccessor().HttpContext.User.Claims.Where(x => x.Type.Contains("role"));
            
            System.Console.WriteLine("abc");

            var ff = new HttpContextAccessor().HttpContext.User.Claims.Where(x => x.Type.Contains("role")).Where(y => y.Value == "Read").SingleOrDefault();
            //var articledto = await articleService.GetAsync(1);
            //var userdto = await userService.GetAsync("user1@email.com");
            //User = userdto.Email;
        }

        public async Task OnPostAsync()
        {
            int? user = Suggestion.IsAnonymous ? (int?)null : Convert.ToInt32(User.Claims.FirstOrDefault(x => x.Type == "http://schemas.xmlsoap.org/ws/2005/05/identity/claims/nameidentifier").Value);
            await suggestionService.AddAsync(user, null, Suggestion.Content);
        }
    }
}
