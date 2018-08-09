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
        public SelectList Categories { get; set; }
        public string SelectedCategory { get; set; }
        public List<TagFilter> Tags { get; set; }
        public SelectList Statuses { get; set; }
    }

    public class TagFilter
    {
        public int Id { get; set; }
        public string Tag { get; set; }
        public bool Checked { get; set; }
    }

    public class CategoryFilter
    {
        public int Id { get; set; }
        public string Category { get; set; }
        public bool Selected { get; set; }
    }

    public class StatusFilter
    {
        public int Id { get; set; }
        public string Status { get; set; }
        public bool Selected { get; set; }
    }
}
