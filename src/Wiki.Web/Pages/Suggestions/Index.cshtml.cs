using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;
using Wiki.Infrastructure.Services;
using Wiki.Web.ViewModels;

namespace Wiki.Web.Pages.Suggestions
{
    public class IndexModel : PageModel
    {
        private readonly ISuggestionService suggestionService;
        private readonly IArticleService articleService;
        private readonly IUserService userService;

        [BindProperty]
        public List<Suggestion> Suggestions { get; set; }



        public IndexModel(ISuggestionService suggestionService, IArticleService articleService, IUserService userService)
        {
            this.suggestionService = suggestionService;
            this.articleService = articleService;
            this.userService = userService;
        }
        public async Task OnGetAsync()
        {
            var suggestions = await suggestionService.BrowseAsync();
            Suggestions = new List<Suggestion>();
            foreach(var suggestion in suggestions)
            {
                var art = new Article();
                if (suggestion.Text != null)
                    art.Title = suggestion.Text.Title;

                var author = new Author();
                if (suggestion.Author != null)
                    author.Email = suggestion.Author.Email;

                Suggestions.Add(new Suggestion
                {
                    Article = art,
                    Author = author,
                    Content = suggestion.Content,
                    Id = suggestion.Id,
                    Status = suggestion.Served
                });
            }
        }
    }
}