using BC.Search.Helper;
using Core.Entities;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Common.Helpers
{
    public static class UrlBuilder
    {
        public static string PlaceUrl(int? id, string slug, string startSlug)
        {
            return PlaceUrl(id, slug, startSlug, string.Empty, string.Empty);
        }
        public static string PlaceUrl(int? id, string slug, string startSlug, string sort, string filter)
        {
            if (!id.HasValue || string.IsNullOrEmpty(slug))
                return string.Empty;

            slug = slug.Trim().Replace(" ", "-").Replace("/", "-").Replace(".", "-").Replace("å", "a").Replace("ä", "a").Replace("ö", "o").Replace("!", "-").ToLower();
            startSlug = startSlug.Trim().Replace(" ", "-").Replace("/", "-").ToLower();

            string sortindex = string.Empty;

            if (!string.IsNullOrEmpty(sort))
            {
                sortindex = "?sort=" + sort;
            }

            string filterIndex = string.Empty;

            if (!string.IsNullOrEmpty(filter))
            {
                string filterChar = string.IsNullOrEmpty(sortindex) ? "?filter=" : "&filter=";

                filterIndex = filterChar + filter;
            }

            string url = string.Format("/{0}/{1}/{2}/{3}{4}", startSlug, id, slug, sortindex, filterIndex);
            return url;
        }

        public static string ProductUrl(int? id, string slug, string startSlug, string placeSlug)
        {
            if (!id.HasValue || string.IsNullOrEmpty(slug))
                return string.Empty;

            slug = slug.Trim().Replace(" ", "-").Replace("/", "-").Replace(".", "-").Replace("å", "a").Replace("ä", "a").Replace("ö", "o").Replace("!", "-").ToLower();
            placeSlug = placeSlug.Trim().Replace(" ", "-").Replace("/", "-").Replace(".", "-").ToLower();

            string url = string.Format("/{0}/{3}/{1}/{2}", startSlug, id, slug, placeSlug);
            return url;
        }
    }
}
