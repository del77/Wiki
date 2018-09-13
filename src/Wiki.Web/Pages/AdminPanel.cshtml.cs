using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Http;
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
        private readonly IArticleService articleService;
        private readonly int userId;
        public AdminPanelModel(IUserService userService, ITagService tagService, ICategoryService categoryService, IArticleService articleService, IHttpContextAccessor httpContextAccessor)
        {
            this.userService = userService;
            this.tagService = tagService;
            this.categoryService = categoryService;
            this.articleService = articleService;
            var claims = (httpContextAccessor.HttpContext.User.Claims.FirstOrDefault(x => x.Type == "http://schemas.xmlsoap.org/ws/2005/05/identity/claims/nameidentifier"));
            if (claims != null)
                userId = Convert.ToInt32(claims.Value);
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

        public async Task OnPostAddInstruction(string content, string title)
        {
            await articleService.AddAsync(0, title, content, 101, new int[] { }, 0, userId, 1.0, new byte[] { });
        }
    }
}