using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;
using Microsoft.AspNetCore.Mvc.Rendering;
using Oracle.ManagedDataAccess.Client;
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
        [BindProperty]
        public Filter Filter { get; set;
        }
        public AddModel(IArticleService articleService)
        {
            Article = new Article();
            this.articleService = articleService;
        }

        public async Task OnGet()
        {
            await SetupFilter();
        }

        public async Task<IActionResult> OnPostAsync(string[] selectedTags, string selectedCategory)
        {
            //OracleCommand cmd = new OracleCommand()
            await articleService.AddAsync(Article.Title, Article.Content, selectedTags, selectedCategory);
            
            return Page();
        }

        private async Task SetupFilter()
        {
            var filter = await articleService.GetFilterInfo();
            //Filter = new Filter
            //{
                
            //    Categories = new SelectList(filter.ElementAt(0)),
            //    //Tags = new Dictionary<string, bool>(),
            //    Statuses = filter.ElementAt(2)
            //};
            //foreach (var tag in filter.ElementAt(1))
            //{
            //    Filter.Tags.Add(tag, false);
            //}

        }
    }
}
