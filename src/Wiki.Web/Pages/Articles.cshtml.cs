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
        public List<ViewModels.Article> Articles { get; set; }
        [BindProperty]
        public Filter Filter { get; set; }

        public ArticlesModel(IArticleService articleService)
        {
            //this.commandDispatcher = commandDispatcher;    
            this.articleService = articleService;
        }


        public async Task OnGetAsync(string title, IEnumerable<string> selectedTags, string selectedCategory)
        {
            SetupFilter(title, selectedTags, selectedCategory);
            
            var res = await articleService.BrowseAsync(title, selectedTags, selectedCategory);
            Articles = new List<ViewModels.Article>();
            foreach (var item in res)
            {
                foreach (var text in item.Texts)
                {
                    Articles.Add(new ViewModels.Article
                    {
                        Title = text.Title,
                        Category = item.Category,
                        Status = text.Status
                    });
                }
            }
        }

        private async void SetupFilter(string title, IEnumerable<string> selectedTags, string selectedCategory)
        {
            var filter = await articleService.GetFilterInfo();
            Filter = new Filter
            {
                Title = title,
                Categories = new SelectList(filter.ElementAt(0)),
                Tags = new Dictionary<string, bool>(),
                Statuses = filter.ElementAt(2)
            };
            foreach (var tag in filter.ElementAt(1))
            {
                Filter.Tags.Add(tag, false);
            }
            foreach (var tag in selectedTags)
            {
                Filter.Tags[tag] = true;
            }
            if (selectedCategory != null)
            {
                Filter.Categories.Where(x => x.Text == selectedCategory).Single().Selected = true;
            }
        }
    }
}
