﻿using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;
using Microsoft.AspNetCore.Mvc.Rendering;
using Oracle.ManagedDataAccess.Client;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Threading.Tasks;
using Wiki.Infrastructure.DTO;
using Wiki.Infrastructure.Services;
using Wiki.Web.ViewModels;
using Microsoft.AspNetCore.Identity;

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
        public bool Editing { get; set; }
        
        public AddModel(IArticleService articleService, IHttpContextAccessor httpContextAccessor)
        {
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
                if (!httpContextAccessor.HttpContext.User.IsInRole("Read") && article.Master.Status.Id != 1)
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
                    }
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
                    Filter.Tags.Where(x => x.Id == tag.Id).Single().Checked = true;
                }
                Article.Tags = tags;
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

        public async Task OnPostAsync(int[] selectedTags, Article Article = null)
        {
            if (ModelState.IsValid)
            {
                if (Article.TextId != 0)
                {
                    var article = await articleService.GetAsync(Article.TextId);
                    Article.Version = article.Master.Version + 0.1;
                    Article.Category = new CategoryFilter
                    {
                        Id = article.Category.Id
                    };
                }
                else
                    Article.Version = 1.0;

                await articleService.AddAsync(Article.ArticleId, Article.Title, Article.Content, selectedTags, Article.Category.Id, userId, Article.Version);
            }
            
            await SetupFilter(selectedTags);
        }

        private async Task SetupFilter(int[] selectedTags)
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

            foreach(var tag in selectedTags)
            {
                Filter.Tags.Single(x => x.Id == tag).Checked = true;
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
        protected void SaveTags(object sender, EventArgs e)
        {
            Article.Tags = new List<TagFilter>(Filter.Tags.Where(x => x.Checked == true));
        }
    }
}
