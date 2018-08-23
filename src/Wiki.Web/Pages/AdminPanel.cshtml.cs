using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;
using Wiki.Infrastructure.Services;
using Wiki.Web.ViewModels;

namespace Wiki.Web.Pages
{
    public class AdminPanelModel : PageModel
    {
        private readonly IUserService userService;

        public AdminPanelModel(IUserService userService)
        {
            this.userService = userService;
        }

        [BindProperty]
        public List<User> Users { get; set; }


        public async Task OnGet()
        {
            var users = await userService.BrowseAsync();

            Users = new List<User>();
            foreach (var user in users)
            {
                Users.Add(new User
                {
                    Id = user.Id,
                    Email = user.Email
                });
            }
        }
    }
}