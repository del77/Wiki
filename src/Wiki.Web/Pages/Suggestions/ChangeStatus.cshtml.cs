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
    public class ChangeStatus : PageModel
    {
        private readonly ISuggestionService suggestionService;

        [BindProperty]
        public Suggestion Suggestion { get; set; }

        public ChangeStatus(ISuggestionService suggestionService)
        {
            this.suggestionService = suggestionService;
        }

        public async Task<IActionResult> OnGetAsync(int? id)
        {
            var suggestion = await suggestionService.GetAsync((int)id);
 

            var art = new Article();
            if (suggestion.Text != null)
                art.Title = suggestion.Text.Title;

            var author = new Author();
            if (suggestion.Author != null)
                author.Email = suggestion.Author.Email;

            Suggestion = new Suggestion
            {
                Article = art,
                Author = author,
                Content = suggestion.Content,
                Id = suggestion.Id,
                Status = suggestion.Served
            };
            return Page();
        }

        public async Task<IActionResult> OnPostAsync(int? id)
        {
            await suggestionService.MakeServed((int)id);

            return Page();
        }
    }
}