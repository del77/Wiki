using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;
using Microsoft.AspNetCore.Mvc.Rendering;
using Wiki.Infrastructure.Services;
using Wiki.Web.ViewModels;

namespace Wiki.Web.Pages
{
    public class ArticlesModel : PageModel
    {
        private readonly IArticleService articleService;

        [BindProperty]
        public IEnumerable<SelectListItem> Cats { get; set; } = new List<SelectListItem>()
        {
            new SelectListItem("", ""),
            new SelectListItem("a", "A"),
            new SelectListItem("b", "B")
        };
        
        [BindProperty]
        public List<ViewModels.Article> Articles { get; set; }
        public ArticlesModel(IArticleService articleService)
        {
            //this.commandDispatcher = commandDispatcher;    
            this.articleService = articleService;
        }

        [BindProperty]
        public Filter Filter { get; set; }

        public async Task OnGetAsync(string title, IEnumerable<string> selectedTags, string selectedCategory)
        {
            Filter = new Filter
            {
                Categories = Cats,
                SelectedCategory = "dupaxD",
                Tags = new List<KeyValuePair<bool, string>>
                {
                    new KeyValuePair<bool, string>(false, "a"),
                    new KeyValuePair<bool, string>(true, "b"),
                    new KeyValuePair<bool, string>(true, "c"),
                }
            };

            var res = await articleService.BrowseAsync(title);
            Articles = new List<ViewModels.Article>();
            foreach (var item in res)
            {
                foreach (var text in item.Texts)
                {
                    Articles.Add(new ViewModels.Article
                    {
                        Title = text.Value.Title,
                        Category = item.Category,
                        Status = text.Value.Status
                    });
                }
            }
        }
    }
}
