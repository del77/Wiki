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

        public ModifyModel(IArticleService articleSerivce, IHttpContextAccessor httpContextAccessor)
        {
            this.articleService = articleSerivce;
            
            user = httpContextAccessor.HttpContext.User;
        }
        public async Task OnGetAsync(int textid, int status)
        {
            if ( (status == 2 || status == 3) && user.IsInRole("Accept"))
                await articleService.ChangeStatus(textid, status);
            if(status == 1 && user.IsInRole("Publish"))
            {
                var article = await articleService.GetAsync(textid);
                var masterForArticle = (await articleService.BrowseAsync(null, new int[0], 0, 1)).Where(x => x.Id == article.Id).SingleOrDefault();
                var tasks = new List<Task>();
                
                tasks.Add(Task.Factory.StartNew(() => articleService.ChangeStatus(textid, status)));
                tasks.Add(Task.Factory.StartNew(() => articleService.ChangeStatus(masterForArticle.Master.Id, 22))); // change actual master status to accepted
                Task.WaitAll(tasks.ToArray());
            }
        }

        public async Task OnPostAsync(int textid, string reason)
        {
            if (user.IsInRole("Accept"))
            {
                int rejectedStatus = 3;
                await articleService.ChangeStatus(textid, rejectedStatus, reason);
            }
        }
        
    }
}