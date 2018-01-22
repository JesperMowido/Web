using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace BC.Search.Helper
{
    public static class SearchHelper
    {
        public static string GetCompleteSearchFilter(string filterQuery)
        {
            if (string.IsNullOrEmpty(filterQuery))
            {
                return string.Empty;
            }

            filterQuery = filterQuery.Trim();

            if (filterQuery.Contains("_"))
            {
                filterQuery = filterQuery.Replace("_", " and ");

            }

            return filterQuery.Replace("-", " ");
        }
    }
}
