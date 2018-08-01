using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
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
        public string User {get; set;}

        [BindProperty]
        public string Customer { get; set; }
        public IndexModel(IUserService userService)
        {
            //this.commandDispatcher = commandDispatcher;    
            this.userService = userService;
        }
        public async Task OnGet()
        {
            System.Console.WriteLine("abc");
            //var userdto = await userService.GetAsync("user1@email.com");
            //User = userdto.Email;
        }
    }
}
