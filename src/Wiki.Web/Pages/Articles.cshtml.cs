using System;
using System.Collections.Generic;
using System.Data;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;
using Microsoft.AspNetCore.Mvc.Rendering;
using Wiki.Infrastructure.DTO;
using Wiki.Infrastructure.Services;
using Wiki.Web.ViewModels;
using X.PagedList;

namespace Wiki.Web.Pages
{
    public class ArticlesModel : PageModel
    {
        private readonly IArticleService articleService;
        private readonly IHttpContextAccessor httpContextAccessor;

        public List<ViewModels.Article> Articles { get; set; }
        [BindProperty]
        public StaticPagedList<ViewModels.Article> ArticlesPaged { get; set; }

        [BindProperty]
        public Filter Filter { get; set; }
        public bool CanRead { get; set; }

        public ArticlesModel(IArticleService articleService, IHttpContextAccessor httpContextAccessor)
        {
            //this.commandDispatcher = commandDispatcher;    
            this.articleService = articleService;
            this.httpContextAccessor = httpContextAccessor;
            CanRead = httpContextAccessor.HttpContext.User.IsInRole("Read");
        }


        public async Task<IActionResult> OnGetAsync(string title, IEnumerable<int> selectedTags, int selectedCategory, int selectedStatus)
        {
            SetupFilter(title, selectedTags, selectedCategory);
            if(!CanRead && selectedStatus != 0)
            {
                return Page();
            }
            if (!CanRead)
                selectedStatus = 1;
            var res = await articleService.BrowseAsync(title, selectedTags, selectedCategory, selectedStatus);
            var res2 = (await articleService.BrowseAsync(title, selectedTags, selectedCategory, selectedStatus)).ToPagedList(1, 5);

            Articles = new List<ViewModels.Article>();
            foreach (var item in res)
            {
                foreach (var text in item.Texts)
                {
                    var article = new Article();
                    article.Title = text.Title;
                    article.Category = new CategoryFilter
                    {
                        Id = item.Category.Id,
                        Category = item.Category.Category
                    };
                    var tags = new List<TagFilter>();
                    foreach(var tag in text.Tags)
                    {
                        tags.Add(new TagFilter
                        {
                            Id = tag.Id,
                            Tag = tag.Tag,
                            Checked = true
                        });
                    }
                    article.ArticleId = item.Id;
                    article.TextId = text.Id;
                    article.Tags = tags;
                    article.Status = new StatusFilter
                    {
                        Id = text.Status.Id,
                        Status = text.Status.Status,
                        Selected = false
                    };
                        

                    Articles.Add(article);
                }
            }
            ArticlesPaged = new StaticPagedList<Article>(Articles, 1, 4, Articles.Count());
            return Page();
        }

        private async void SetupFilter(string title, IEnumerable<int> selectedTags, int selectedCategory)
        {
            var filter = await articleService.GetFilterInfo();
            var categories = new List<CategoryFilter>();
            categories.Add(new CategoryFilter
            {
                Id = 0,
                Category = "All",
                Selected = true
            });
            foreach (var category in filter.Categories)
            {
                categories.Add(new CategoryFilter
                {
                    Id = category.Id,
                    Category = category.Category,
                    Selected = false
                });
            }

            var statuses = new List<StatusFilter>();
            statuses.Add(new StatusFilter
            {
                Id = 0,
                Status = "All",
                Selected = true
            });
            foreach(var status in filter.Statuses)
            {
                statuses.Add(new StatusFilter
                {
                    Id = status.Id,
                    Status = status.Status,
                    Selected = false
                });
            }

            Filter = new Filter
            {
                Title = title,
                Categories = new SelectList(categories, "Id", "Category"),
                Statuses = new SelectList(statuses, "Id", "Status")
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
            if (selectedCategory != 0)
            {
                Filter.Categories.Where(x => x.Value == selectedCategory.ToString()).Single().Selected = true;
            }
            foreach(var tag in selectedTags)
            {
                Filter.Tags.Where(x => x.Id == tag).Single().Checked = true;
            }
        }
    }
}
