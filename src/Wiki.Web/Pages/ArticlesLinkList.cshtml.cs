using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;
using Newtonsoft.Json;
using Wiki.Infrastructure.Services;

namespace Wiki.Web.Pages
{
    public class ArticlesLinkListModel : PageModel
    {
        private readonly IArticleService articleService;

        public ArticlesLinkListModel(IArticleService articleService)
        {
            this.articleService = articleService;
        }
        public async Task<IActionResult> OnGet()
        {
            var articles = await articleService.BrowseAsync(1, null, null, null);
            List<ArtInfo> arts = new List<ArtInfo>();
            foreach (var art in articles.Where(x => x.Master != null))
            {
                arts.Add(new ArtInfo { title = art.Master.Title, value = "/Articles/Get?textid=" + art.Master.Id });
            }
            var json = JsonConvert.SerializeObject(arts);
            return Content(json);
        }

        class ArtInfo
        {
            public string title;
            public string value;
        }
    }
}