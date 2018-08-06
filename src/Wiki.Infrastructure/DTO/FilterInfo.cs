using System;
using System.Collections.Generic;
using System.Text;

namespace Wiki.Infrastructure.DTO
{
    public class FilterInfo
    {
        public IEnumerable<string> Categories { get; set; }
        public IEnumerable<string> Tags { get; set; }
        public IEnumerable<string> Statuses { get; set; }
    }
}
