using System;
using System.Collections.Generic;
using System.Linq;
using System.Security.Claims;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Authentication;
using Microsoft.AspNetCore.Authentication.Cookies;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;
using Wiki.Infrastructure.Services;
using Wiki.Web.Extensions;

namespace Wiki.Web.Pages.Account
{
    public class RegisterModel : PageModel
    {
        private readonly IUserService userService;

        public RegisterModel(IUserService userService)
        {
            this.userService = userService;
        }

        [BindProperty]
        public InputModel Input { get; set; }
        public string ReturnUrl { get; set; }

        public void OnGet()
        {

        }

        public async Task<IActionResult> OnPostAsync(string returnUrl = null)
        {
            ReturnUrl = returnUrl;

            if (ModelState.IsValid)
            {
                // Use Input.Email and Input.Password to authenticate the user
                // with your custom authentication logic.
                //
                // For demonstration purposes, the sample validates the user
                // on the email address maria.rodriguez@contoso.com with 
                // any password that passes model validation.

                try
                {
                    await userService.RegisterAsync(Input.Email, Input.Password);
                }
                catch (Exception e)
                {
                    ModelState.AddModelError(string.Empty, "Invalid register attempt.");
                    return Page();
                }
                


                //_logger.LogInformation($"User {user.Email} logged in at {DateTime.UtcNow}.");

                return LocalRedirect(Url.GetLocalUrl(returnUrl));
            }

            // Something failed. Redisplay the form.
            return Page();
        }


    }
}