using System;
using System.Collections.Generic;
using System.Data;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Identity;
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
            var xd = new HttpContextAccessor().HttpContext.User.Claims.Where(x => x.Type.Contains("role"));
            
            System.Console.WriteLine("abc");

            var ff = new HttpContextAccessor().HttpContext.User.Claims.Where(x => x.Type.Contains("role")).Where(y => y.Value == "Read").SingleOrDefault();
            //var articledto = await articleService.GetAsync(1);
            //var userdto = await userService.GetAsync("user1@email.com");
            //User = userdto.Email;
        }
    }
}
