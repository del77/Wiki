using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Security.Claims;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Authentication;
using Microsoft.AspNetCore.Authentication.Cookies;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;
using Wiki.Infrastructure.Services;
using Wiki.Web.Extensions;
using Wiki.Web.ViewModels;

namespace Wiki.Web.Pages.Account
{
    public class LoginModel : PageModel
    {
        private readonly IUserService userService;
        private readonly IAgentService agentService;

        public LoginModel(IUserService userService)
        {
            this.userService = userService;
        }

        [BindProperty]
        public User User_ { get; set; }
        public string ReturnUrl { get; set; }

        public void OnGet()
        {

        }

        public async Task<IActionResult> OnPostAsync(string returnUrl = null)
        {
            ReturnUrl = returnUrl;

            if (ModelState.IsValid)
            {
                try
                {
                    await userService.LoginAsync(User_.Email, User_.Password);
                }
                catch(Exception e)
                {
                    ModelState.AddModelError(string.Empty, "Invalid login attempt." + e.Message);
                    return Page();
                }
                var user = await userService.GetAsync(User_.Email);

                var claims = new List<Claim>
                {
                    new Claim(ClaimTypes.Name, user.Email),
                    new Claim(ClaimTypes.NameIdentifier, user.Id.ToString())
                    //new Claim("FullName", user.FullName),
                };

                foreach (var permission in user.Permissions)
                {
                    claims.Add(new Claim(ClaimTypes.Role, permission.Permission));
                }
                
                var claimsIdentity = new ClaimsIdentity(
                    claims, CookieAuthenticationDefaults.AuthenticationScheme);

                var authProperties = new AuthenticationProperties();

                await HttpContext.SignInAsync(
                    CookieAuthenticationDefaults.AuthenticationScheme,
                    new ClaimsPrincipal(claimsIdentity),
                    authProperties);

                return LocalRedirect(Url.GetLocalUrl(returnUrl));
            }

            // Something failed. Redisplay the form.
            return Page();
        }
    }
}