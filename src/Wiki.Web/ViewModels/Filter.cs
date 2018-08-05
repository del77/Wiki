using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc.Rendering;

namespace Wiki.Web.ViewModels
{
    public class Filter
    {
        public string Title { get; set; }
        public IEnumerable<SelectListItem> Categories { get; set; }
        public string SelectedCategory { get; set; }
        public IEnumerable<KeyValuePair<bool, string>> Tags { get; set; }
    }
}
