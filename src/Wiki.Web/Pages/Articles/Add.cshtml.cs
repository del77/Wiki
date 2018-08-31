using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;
using Microsoft.AspNetCore.Mvc.Rendering;
using Oracle.ManagedDataAccess.Client;
using System;
using System.IO;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Threading.Tasks;
using Wiki.Infrastructure.DTO;
using Wiki.Infrastructure.Services;
using Wiki.Web.ViewModels;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Hosting;
using System.Drawing;

namespace Wiki.Web.Pages.Articles
{
    public class AddModel : PageModel
    {
        private readonly IArticleService articleService;
        private readonly IHttpContextAccessor httpContextAccessor;

        [BindProperty]
        public ViewModels.Article Article { get; set; }
        [BindProperty]
        public Filter Filter { get; set; }
        private readonly int userId;
        private readonly IHostingEnvironment _environment;
        private readonly ITagService tagService;
        private readonly ICategoryService categoryService;

        public readonly ImageProperties imageProperties = new ImageProperties();

        public bool Editing { get; set; }
        [BindProperty]
        public IFormFile Upload { get; set; }

        public AddModel(IArticleService articleService, IHttpContextAccessor httpContextAccessor, IHostingEnvironment _environment, ITagService tagService, ICategoryService categoryService)
        {
            this._environment = _environment;
            this.tagService = tagService;
            this.categoryService = categoryService;
            Article = new Article();
            this.articleService = articleService;
            this.httpContextAccessor = httpContextAccessor;
            userId = Convert.ToInt32(httpContextAccessor.HttpContext.User.Claims.FirstOrDefault(x => x.Type == "http://schemas.xmlsoap.org/ws/2005/05/identity/claims/nameidentifier").Value);
        }

        public async Task<IActionResult> OnGet(int textid)
        {
            await SetupFilter(new int[0]);
            if (textid != 0)
            {
                var article = await articleService.GetAsync(textid);
                if (!httpContextAccessor.HttpContext.User.IsInRole("Read") && article.Master.Status.Id != 1 && article.Master.Author.Id != userId)
                    return Page();
                Article = new ViewModels.Article
                {
                    ArticleId = article.Id,
                    TextId = article.Master.Id,
                    Category = new ViewModels.CategoryFilter
                    {
                        Id = article.Category.Id,
                        Category = article.Category.Category
                    },
                    Content = article.Master.Content,
                    Status = new ViewModels.StatusFilter
                    {
                        Id = article.Master.Status.Id,
                        Status = article.Master.Status.Status
                    },
                    Title = article.Master.Title,
                    Author = new Author
                    {
                        Id = article.Master.Author.Id,
                        Email = article.Master.Author.Email
                    },
                    Avatar = String.Format("data:image/gif;base64,{0}", Convert.ToBase64String(article.Master.Avatar))
                };
                var tags = new List<TagFilter>();
                foreach (var tag in article.Master.Tags)
                {
                    tags.Add(new TagFilter
                    {
                        Id = tag.Id,
                        Tag = tag.Tag,
                        Checked = true
                    });
                    Filter.Tags.Where(x => x.Value == tag.Id.ToString()).Single().Selected = true;
                }
                Article.Tags = tags;
                //Article.Tags.Single(x => x.Id == 2).Checked = true;
                Article.Content = article.Master.Content;
                Article.Title = article.Master.Title;
                Editing = true;
            }
            else
            {
                Editing = false;
            }
            return Page();
        }

        public async Task OnPostAsync(int[] selectedTags, int submit, IFormFile avatar, Article Article = null)
        {


            if (ModelState.IsValid)
            {
                byte[] image = new byte[] { };
                if (avatar != null)
                {
                    using (var memoryStream = new MemoryStream())
                    {
                        avatar.CopyTo(memoryStream);
                        image = memoryStream.ToArray();
                        Image icon = new Bitmap(memoryStream);
                        if (icon.Width > imageProperties.Width || icon.Height > imageProperties.Height)
                            throw new Exception("Image dimensions are wrong");
                    }
                }
                
                if (Article.TextId != 0)
                {
                    var editedArticle = await articleService.GetAsync(Article.TextId);
                    var master = (await articleService.BrowseAsync(1, null, editedArticle.Id, null)).SingleOrDefault();
                    if (master.Master != null)
                    {
                        var masterDetails = await articleService.GetAsync(master.Master.Id);
                        Article.Version = masterDetails.Master.Version + 0.1;
                    }
                    else
                    Article.Version = 1.0;
                    Article.Comment = editedArticle.Master.TextComment;
                    Article.ArticleId = editedArticle.Id;
                    Article.Category = new CategoryFilter
                    {
                        Id = editedArticle.Category.Id
                    };

                    if (submit == 0 && editedArticle.Master.Status.Status == "NotSubmitted")
                        await articleService.UpdateVersion(Article.ArticleId, Article.TextId, Article.Title, Article.Content, 41, selectedTags, Article.Category.Id, userId, Article.Version, Article.Comment);
                    else if (submit == 0 && editedArticle.Master.Status.Status != "NotSubmitted")
                        await articleService.AddAsync(Article.ArticleId, Article.Title, Article.Content, 41, selectedTags, Article.Category.Id, userId, Article.Version, image);
                    else if (submit == 1 && editedArticle.Master.Status.Status == "NotSubmitted")
                    {
                        await articleService.UpdateVersion(Article.ArticleId, Article.TextId, Article.Title, Article.Content, 2, selectedTags, Article.Category.Id, userId, Article.Version, Article.Comment);
                    }
                    else if (submit == 1 && editedArticle.Master.Status.Status != "NotSubmitted")
                        await articleService.AddAsync(Article.ArticleId, Article.Title, Article.Content, 2, selectedTags, Article.Category.Id, userId, Article.Version, image);
                }
                else
                {
                    Article.Version = 1.0;
                    if (submit == 0)
                        await articleService.AddAsync(Article.ArticleId, Article.Title, Article.Content, 41, selectedTags, Article.Category.Id, userId, Article.Version, image);
                    else if (submit == 1)
                    {
                        await articleService.AddAsync(Article.ArticleId, Article.Title, Article.Content, 2, selectedTags, Article.Category.Id, userId, Article.Version, image);
                    }
                }
                //return RedirectToPage("/Articles");
            }

            await SetupFilter(selectedTags);
            //return Page();
        }

        private async Task SetupFilter(int[] selectedTags)
        {

            var tagss = await tagService.GetAllAsync();
            var categoriess = await categoryService.GetAllAsync();
            var categories = new List<CategoryFilter>();

            foreach (var category in categoriess)
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
                Categories = new SelectList(categories, "Id", "Category"),
            };
            var tags = new List<SelectListItem>();

            //Filter.Tags2 = new List<TagFilter>();
            foreach (var tag in tagss)
            {
                tags.Add(new SelectListItem
                {
                    Text = tag.Tag,
                    Value = tag.Id.ToString(),
                    Selected = false
                });
            }
            foreach(var tag in selectedTags)
            {
                tags.Single(x => x.Value == tag.ToString()).Selected = true;
            }
            Filter.Tags = new SelectList(tags, "Value", "Text");
        }
    }
}

public class ImageProperties
{
    public int Width = 300;
    public int Height = 300;
}