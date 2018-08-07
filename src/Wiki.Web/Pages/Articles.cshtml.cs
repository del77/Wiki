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
                        Category = item.Category.Category,
                        Status = text.Status.Status
                    });
                }
            }
        }

        private async void SetupFilter(string title, IEnumerable<string> selectedTags, string selectedCategory)
        {
            var filter = await articleService.GetFilterInfo();
            var categories = new List<CategoryFilter>();
            foreach (var category in filter.Categories)
            {
                categories.Add(new CategoryFilter
                {
                    Id = category.Id,
                    Category = category.Category,
                    Selected = false
                });
            }

            Filter = new Filter
            {
                Title = title,
                Categories = new SelectList(categories, "Id", "Category"),
                //Tags = filter.Tags,
                //Statuses = filter.S
            };

            Filter.Tags = new List<TagFilter>();
            foreach (var tag in filter.Tags)
            {
                Filter.Tags.Add(new TagFilter
                {
                    Id = tag.Id,
                    Tag = tag.Tag,
                    Checked = false
                });
            }
            //foreach (var tag in selectedTags)
            //{
            //    Filter.Tags[tag] = true;
            //}
            if (selectedCategory != null)
            {
                Filter.Categories.Where(x => x.Text == selectedCategory).Single().Selected = true;
            }
        }
    }
}
