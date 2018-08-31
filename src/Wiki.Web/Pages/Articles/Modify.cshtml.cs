using System;
using System.Collections.Generic;
using System.Linq;
using System.Security.Claims;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Hosting.Internal;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;
using Wiki.Infrastructure.Services;

namespace Wiki.Web.Pages.Articles
{
    public class ModifyModel : PageModel
    {
        private readonly IArticleService articleService;
        
        private readonly ClaimsPrincipal user;
        private readonly int userId;

        public ModifyModel(IArticleService articleSerivce, IHttpContextAccessor httpContextAccessor)
        {
            this.articleService = articleSerivce;
            
            user = httpContextAccessor.HttpContext.User;
            var claims = user.Claims.FirstOrDefault(x => x.Type == "http://schemas.xmlsoap.org/ws/2005/05/identity/claims/nameidentifier");
            if (claims != null)
                userId = Convert.ToInt32(claims.Value);
        }
        public async Task<IActionResult> OnGetAsync(int textid, int status)
        {
            if ( (status == 22 || status == 3) && user.IsInRole("Accept"))
                await articleService.ChangeStatus(textid, status);
            else if(status == 1 && user.IsInRole("Publish"))
            {
                var article = await articleService.GetAsync(textid);
                var masterForArticle = (await articleService.BrowseAsync(1, null, article.Id, null)).SingleOrDefault();
                var tasks = new List<Task>
                {
                    Task.Factory.StartNew(() => articleService.ChangeStatus(textid, status)),
                };
                if (masterForArticle.Master != null)
                    tasks.Add(Task.Factory.StartNew(() => articleService.ChangeStatus(masterForArticle.Master.Id, 21))); // change actual master status to archieved
                Task.WaitAll(tasks.ToArray());
            }
            else if(status==61 && user.IsInRole("Accept"))
            {
                await articleService.ChangeStatus(textid, status);
                await articleService.SetSupervisor(textid, userId);

            }
            return RedirectToPage("/Articles");
        }

        public async Task<IActionResult> OnPostAsync(int textid, string reason)
        {
            if (user.IsInRole("Accept"))
            {
                int rejectedStatus = 3;
                await articleService.ChangeStatus(textid, rejectedStatus, reason);
            }
            return RedirectToPage("/Articles");
        }
        
    }
}