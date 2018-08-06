using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;
using Microsoft.AspNetCore.Mvc.Rendering;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Wiki.Infrastructure.Services;
using Wiki.Web.ViewModels;

namespace Wiki.Web.Pages.Articles
{
    public class AddModel : PageModel
    {
        private readonly IArticleService articleService;

        [BindProperty]
        public ViewModels.Article Article { get; set; }

        public AddModel(IArticleService articleService)
        {
            Article = new Article();
            this.articleService = articleService;
        }

        public async Task OnGet()
        {
            await SetupFilter();
        }

        public void OnPost()
        {

        }

        private async Task SetupFilter()
        {
            var filter = await articleService.GetFilterInfo();
            Article.Filter = new Filter
            {
                
                Categories = new SelectList(filter.ElementAt(0)),
                Tags = new Dictionary<string, bool>(),
                Statuses = filter.ElementAt(2)
            };
            foreach (var tag in filter.ElementAt(1))
            {
                Article.Filter.Tags.Add(tag, false);
            }

        }
    }
}
