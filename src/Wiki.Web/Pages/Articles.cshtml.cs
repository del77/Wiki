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
        private readonly ICategoryService categoryService;
        private readonly IStatusService statusService;

        public List<ViewModels.Article> Articles { get; set; }

        [BindProperty]
        public BrowseFilter BrowseFilter { get; set; }
        

        public bool CanRead { get; set; }
        private readonly int userId;

        public ArticlesModel(IArticleService articleService, IUserService userService, IHttpContextAccessor httpContextAccessor, ICategoryService categoryService, IStatusService statusService)
        {
            
            //this.commandDispatcher = commandDispatcher;    
            this.articleService = articleService;
            this.userService = userService;
            this.httpContextAccessor = httpContextAccessor;
            this.categoryService = categoryService;
            this.statusService = statusService;
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
            int? selectedSupervisor = null;

            
            if (mine == 1)
            {
                selectedUser = userId;
                CanRead = true;
            }
            else if(mine == 2)
            {
                if(!User.IsInRole("Accept"))
                {
                    // no access
                }
                selectedSupervisor = userId;
                CanRead = true;
            }
            else if (!CanRead)
                selectedStatus = 1;
            var res = await articleService.BrowseAsync(selectedStatus, selectedUser, null, selectedSupervisor);

            Articles = new List<ViewModels.Article>();
         
            foreach (var item in res)
            {
                foreach (var text in item.Texts)
                {
                    var article = new Article
                    {
                        Title = text.Title,
                        Category = new CategoryFilter
                        {
                            Id = item.Category.Id,
                            Category = item.Category.Category
                        }
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
            BrowseFilter.Users = JsonConvert.SerializeObject(Articles.Select(x => x.Author.Email).Distinct());

            return Page();
        }
        
        private async void SetupFilter()
        {
            var statusess = await statusService.GetAllAsync();
            var categoriess = await categoryService.GetAllAsync();
            BrowseFilter = new BrowseFilter
            {
                Categories = JsonConvert.SerializeObject(categoriess.Select(x => x.Category)),
                Statuses = JsonConvert.SerializeObject(statusess.Select(x => x.Status))
            };
        }
    }
}
