using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;
using Wiki.Infrastructure.Services;

namespace Wiki.Web.Pages.Articles
{
    public class ModifyModel : PageModel
    {
        private readonly IArticleService articleSerivce;
        private readonly IHttpContextAccessor httpContextAccessor;

        public ModifyModel(IArticleService articleSerivce, IHttpContextAccessor httpContextAccessor)
        {
            this.articleSerivce = articleSerivce;
            this.httpContextAccessor = httpContextAccessor;
        }
        public async void OnGetAsync(int textid, int status)
        {
            if (httpContextAccessor.HttpContext.User.IsInRole("Accept"))
                await articleSerivce.ChangeStatus(textid, status);
        }

        public async void OnPostAsync(int textid, string reason)
        {
            if (httpContextAccessor.HttpContext.User.IsInRole("Accept"))
            {
                int rejectedStatus = 3;
                await articleSerivce.ChangeStatus(textid, rejectedStatus, reason);
            }
        }
    }
}