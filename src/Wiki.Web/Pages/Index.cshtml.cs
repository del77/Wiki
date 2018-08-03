using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;
using Wiki.Infrastructure.Commands;
using Wiki.Infrastructure.DTO;
using Wiki.Infrastructure.Services;

namespace Wiki.Web.Pages
{
    public class IndexModel : PageModel
    {

        //ICommandDispatcher commandDispatcher; dodac do konstruktora
        IUserService userService;
        IArticleService articleService;
        public string User {get; set;}

        [BindProperty]
        public string Customer { get; set; }
        public IndexModel(IUserService userService, IArticleService articleService)
        {
            //this.commandDispatcher = commandDispatcher;    
            this.userService = userService;
            this.articleService = articleService;
        }
        public async Task OnGet()
        {
            var xd = new HttpContextAccessor().HttpContext.User.Claims;
            System.Console.WriteLine("abc");
            var articledto = await articleService.GetAsync(1);
            //var userdto = await userService.GetAsync("user1@email.com");
            //User = userdto.Email;
        }
    }
}
