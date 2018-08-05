using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;

namespace Wiki.Web.Pages.Articles
{
    public class AddModel : PageModel
    {
        [BindProperty]
        public ViewModels.Article Article { get; set; }
        public void OnGet()
        {

        }

        public void OnPost()
        {

        }
    }
}