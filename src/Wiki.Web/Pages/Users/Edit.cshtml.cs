using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;
using Wiki.Infrastructure.Services;
using Wiki.Web.ViewModels;

namespace Wiki.Web.Pages.Users
{
    public class EditModel : PageModel
    {
        private readonly IUserService userService;

        public EditModel(IUserService userService)
        {
            this.userService = userService;
        }

        [BindProperty]
        public User User { get; set; }

        public async Task OnGet(int id)
        {
            var user = await userService.GetAsync(id);
            //var userPermissions = await userService.GetPermissions(id);
            User = new User
            {
                Email = user.Email
            };
            
        }
    }
}