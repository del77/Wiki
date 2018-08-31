using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;
using Wiki.Infrastructure.Services;
using Wiki.Web.ViewModels;

namespace Wiki.Web.Pages
{
    [Authorize(Roles = "Admin")]
    public class AdminPanelModel : PageModel
    {
        private readonly IUserService userService;
        private readonly ITagService tagService;
        private readonly ICategoryService categoryService;

        public AdminPanelModel(IUserService userService, ITagService tagService, ICategoryService categoryService)
        {
            this.userService = userService;
            this.tagService = tagService;
            this.categoryService = categoryService;
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

        public async Task OnPostAsync(string tag, string category)
        {
            if (tag != null)
                await tagService.CreateAsync(tag);
            else
                await categoryService.CreateAsync(category);
        }
    }
}