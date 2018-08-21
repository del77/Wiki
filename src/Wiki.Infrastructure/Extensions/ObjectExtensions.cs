using System;
using System.Collections.Generic;
using System.Text;

namespace Wiki.Infrastructure.Extensions
{
    public static class ObjectExtensions
    {
        public static string NullString(this object obj)
        {
            return obj?.ToString() ?? "null";
        }
    }
}
