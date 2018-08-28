using System;
using System.Collections.Generic;
using System.Data;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;
using Microsoft.AspNetCore.Mvc.Rendering;
using Newtonsoft.Json;
using Wiki.Infrastructure.DTO;
using Wiki.Infrastructure.Services;
using Wiki.Web.ViewModels;
using X.PagedList;

namespace Wiki.Web.Pages
{
    public class ArticlesModel : PageModel
    {
        private readonly IArticleService articleService;
        private readonly IUserService userService;
        private readonly IHttpContextAccessor httpContextAccessor;

        
        public List<ViewModels.Article> Articles { get; set; }
        [BindProperty]
        public StaticPagedList<ViewModels.Article> ArticlesPaged { get; set; }

        [BindProperty]
        public Filter Filter { get; set; }
        [BindProperty]
        public BrowseFilter BrowseFilter { get; set; }
        

        public bool CanRead { get; set; }
        private readonly int userId;

        public ArticlesModel(IArticleService articleService, IUserService userService, IHttpContextAccessor httpContextAccessor)
        {
            
            //this.commandDispatcher = commandDispatcher;    
            this.articleService = articleService;
            this.userService = userService;
            this.httpContextAccessor = httpContextAccessor;
            CanRead = httpContextAccessor.HttpContext.User.IsInRole("Read");
            var claims = (httpContextAccessor.HttpContext.User.Claims.FirstOrDefault(x => x.Type == "http://schemas.xmlsoap.org/ws/2005/05/identity/claims/nameidentifier"));
            if (claims != null)
                userId = Convert.ToInt32(claims.Value);
        }


        public async Task<IActionResult> OnGetAsync(int mine)
        {
            SetupFilter();
            int? selectedStatus = null;
            int? selectedUser = null;
            Filter = new Filter
            {
                Categories = new SelectList(new int[] { }),
                SelectedTags = new List<int>(),
                Statuses = new SelectList(new int[] { }),
                Tags2 = new List<TagFilter>()

            };
            
            if (mine == 1)
            {
                selectedUser = userId;
                CanRead = true;
            }
            else if (!CanRead)
                selectedStatus = 1;
            var res = await articleService.BrowseAsync(selectedStatus, selectedUser, null);
            //var res2 = (await articleService.BrowseAsync(title, selectedTags, selectedCategory, selectedStatus)).ToPagedList(1, 5);

            Articles = new List<ViewModels.Article>();
            
            //List<ArticleDto> test = new List<ArticleDto>();
            //var a1 = new ArticleDto();
            //a1.Category = new ArticleCategoryDto
            //{
            //    Category = "cat1",
            //    Id = 1
            //};
            //var t1 = new TextDetailsDto
            //{
            //    Status = new TextStatusDto
            //    {
            //        Id = 1,
            //        Status = "stat1"
            //    },
            //    Title = "article1"
            //};
            //var tags1 = new List<TextTagDto>();
            //tags1.Add(new TextTagDto
            //{
            //    Id = 1,
            //    Tag = "tag1"
            //});
            //t1.Tags = tags1;
            //a1.Master = t1;
            //a1.Texts = new List<TextDto>() {t1};

            //var a2 = new ArticleDto();
            //a2.Category = new ArticleCategoryDto
            //{
            //    Category = "cat2",
            //    Id = 2
            //};
            //var t2 = new TextDetailsDto
            //{
            //    Status = new TextStatusDto
            //    {
            //        Id = 2,
            //        Status = "stat2"
            //    },
            //    Title = "article2"
            //};
            //var tags2 = new List<TextTagDto>();
            //tags2.Add(new TextTagDto
            //{
            //    Id = 2,
            //    Tag = "tag2"
            //});
            //t2.Tags = tags2;
            //a2.Master = t2;
            //a2.Texts = new List<TextDto>() { t2 };

            //test.Add(a1);
            //test.Add(a2);
            //var res = test;

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
                    article.Author = new Author
                    {
                        Id = text.Author.Id,
                        Email = text.Author.Email
                    };
                    if (text.Supervisor != null)
                    {
                        article.Supervisor = new User
                        {
                            Id = text.Supervisor.Id,
                            Email = text.Supervisor.Email
                        };
                    }
                    Articles.Add(article);
                }
            }

            BrowseFilter.Titles = JsonConvert.SerializeObject(Articles.Select(x => x.Title));
            //BrowseFilter.Users = JsonConvert.SerializeObject(res.SelectMany(x => x.Texts).Select(y => y.Author.Email).Distinct());\
            BrowseFilter.Users = JsonConvert.SerializeObject(Articles.Select(x => x.Author.Email).Distinct());


            ArticlesPaged = new StaticPagedList<Article>(Articles, 1, 4, Articles.Count());
            return Page();
        }
        
        private async void SetupFilter()
        {
            var filter = await articleService.GetFilterInfo();
            BrowseFilter = new BrowseFilter();
            BrowseFilter.Categories = JsonConvert.SerializeObject(filter.Categories.Select(x => x.Category));
            BrowseFilter.Statuses = JsonConvert.SerializeObject(filter.Statuses.Select(x => x.Status));

            
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

            //Filter = new Filter
            //{
            //    Title = title,
            //    Categories = new SelectList(categories, "Id", "Category"),
            //    Statuses = new SelectList(statuses, "Id", "Status")
            //};

            //Filter.Tags2 = new List<TagFilter>();
            //foreach (var tag in filter.Tags)
            //{
            //    Filter.Tags2.Add(new TagFilter
            //    {
            //        Id = tag.Id,
            //        Tag = tag.Tag,
            //        Checked = false
            //    });
            //}
            //if (selectedCategory != 0)
            //{
            //    Filter.Categories.Where(x => x.Value == selectedCategory.ToString()).Single().Selected = true;
            //}
            //foreach(var tag in selectedTags)
            //{
            //    Filter.Tags2.Where(x => x.Id == tag).Single().Checked = true;
            //}
        }
    }
}
