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
        [BindProperty]
        public List<Permission> Permissions { get; set; }

        public async Task OnGetAsync(int userId)
        {
            var permissionsTask = userService.GetPermissionsInfo();
            Permissions = new List<Permission>();
            var permissions = await permissionsTask;
            foreach (var permission in permissions)
            {
                Permissions.Add(new Permission
                {
                    Id = permission.Id,
                    PermissionName = permission.Permission
                });
            }

            var user = await userService.GetAsync(userId);
            //var userPermissions = await userService.GetPermissions(id);
            User = new User
            {
                Id = user.Id,
                Email = user.Email,
            };
            var userPermissions = new List<Permission>();
            foreach(var permission in user.Permissions)
            {
                Permissions.Where(x => x.Id == permission.Id).Single().Checked = true;
            }
            
        }

        public async Task OnPostAsync(int[] selectedPermissions, int userId)
        {
            await userService.UpdatePermissions(userId, selectedPermissions);
        }
    }
}