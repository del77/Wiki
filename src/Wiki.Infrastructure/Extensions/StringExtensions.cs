using System;
using System.Collections.Generic;
using System.Text;

namespace Wiki.Infrastructure.Extensions
{
    public static class StringExtensions
    {
        public static bool Empty(this string value)
        {
            if (string.IsNullOrWhiteSpace(value))
                return true;
            return false;
        }

    }
}
