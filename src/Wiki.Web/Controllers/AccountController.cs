using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Authentication;
using Microsoft.AspNetCore.Authentication.Cookies;
using Microsoft.AspNetCore.Mvc;

namespace Wiki.Web.Controllers
{
    [Route("[controller]/[action]")]
    public class AccountController : Controller
    {
        //private readonly ILogger _logger;

        public AccountController()
        {
            //_logger = logger;
        }

        [HttpPost]
        public async Task<IActionResult> Logout()
        {
            //_logger.LogInformation($"User {User.Identity.Name} logged out at {DateTime.UtcNow}.");

            
            await HttpContext.SignOutAsync(CookieAuthenticationDefaults.AuthenticationScheme);

            return RedirectToPage("/Account/SignedOut");
        }
    }
}