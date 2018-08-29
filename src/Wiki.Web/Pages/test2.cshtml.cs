using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using System.IO;
using Microsoft.AspNetCore.Mvc.RazorPages;
using static System.Net.Mime.MediaTypeNames;

namespace Wiki.Web.Pages
{
    public class test2Model : PageModel
    {
        public byte[] xd { get; set; } = new byte[] { 1, 2, 3 };
        public void OnGet()
        {
            

        }
    }
}