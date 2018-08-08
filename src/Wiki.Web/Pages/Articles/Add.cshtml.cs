using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;
using Microsoft.AspNetCore.Mvc.Rendering;
using Oracle.ManagedDataAccess.Client;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Threading.Tasks;
using Wiki.Infrastructure.DTO;
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

        public async Task OnPostAsync(int[] selectedTags)
        {
            if (ModelState.IsValid)
            {
                //OracleCommand cmd = new OracleCommand()
                var article = new ArticleDetailsDto();
                //article.Category = new ArticleCategoryDto { Id = article.Category.Id };

                //var tags = new List<TextTagDto>();
                //foreach(var tag in selectedTags)
                //{
                //    tags.Add(new TextTagDto
                //    {
                //        Id = tag
                //    });
                //}

                //article.Text = new TextDetailsDto
                //{
                //    //Author
                //    Content = Article.Content,
                //    Tags = tags,
                //    Title = Article.Title
                //};

                await articleService.AddAsync(Article.Title, Article.Content, selectedTags, Article.Category.Id, 3);
            }
            await SetupFilter();
        }

        private async Task SetupFilter()
        {
            var filter = await articleService.GetFilterInfo();
            var categories = new List<CategoryFilter>();
            //categories.Add(new CategoryFilter
            //{
            //    Id = 0,
            //    Category = "All",
            //    Selected = true
            //}); todo select cateogry
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
                //Title = title,
                Categories = new SelectList(categories, "Id", "Category"),
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
            //if (selectedCategory != 0)
            //{
            //    Filter.Categories.Where(x => x.Value == selectedCategory.ToString()).Single().Selected = true;
            //}
            //foreach (var tag in selectedTags)
            //{
            //    Filter.Tags.Where(x => x.Id == tag).Single().Checked = true;
            //}
            

        }
    }
}
