using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Wiki.Web.Extensions
{
    public static class IEnumerableStringExtensions
    {
        public static string AsString(this IEnumerable<string> data)
        {
            var ret = new StringBuilder();
            var count = data.Count();
            for(int i=0;i<count;i++)
            {
                ret.Append(data.ElementAt(i));
                if (i != count - 1)
                    ret.Append(", ");
            }
            return ret.ToString();
        }
    }
}
