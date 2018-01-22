using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Web;
using System.Web.Mvc;

namespace Common.Extensions
{
    public static class PagerExtensions
    {
        public static MvcHtmlString Pager(this HtmlHelper htmlHelper, int pageSize, int currentPage, int totalItemCount, string baseUrl)
        {
            //remove empty sort for baseurl
            if (baseUrl.Contains("sort=&"))
            {
                baseUrl = baseUrl.Replace("sort=&", "");
            }
            var pager = new Pager(pageSize, currentPage, totalItemCount, baseUrl);
            return pager.RenderHtml();
        }
    }

    public class Pager
    {
        private readonly int pageSize;
        private readonly int currentPage;
        private readonly int totalItemCount;
        private readonly string baseUrl;

        public Pager(int pageSize, int currentPage, int totalItemCount, string baseUrl)
        {
            this.pageSize = pageSize;
            this.currentPage = currentPage;
            this.totalItemCount = totalItemCount;
            this.baseUrl = baseUrl;
        }

        public MvcHtmlString RenderHtml()
        {
            var pageCount = (int)Math.Floor(totalItemCount / (double)pageSize);
            const int nrOfPagesToDisplay = 10;

            var sb = new StringBuilder();

            //first page
            sb.Append(currentPage > 1 ? "<li>" + GenerateFirstPageLink(baseUrl, "&lt;&lt;") + "</li>" : string.Empty);

            // Previous
            sb.Append(currentPage >= 3 ? "<li>" + GeneratePageLink(baseUrl, "&lt;", (currentPage - 1), "prev") + "</li>" : string.Empty);

            var start = 1;
            var end = pageCount + 1;

            if (pageCount > nrOfPagesToDisplay)
            {
                var middle = (int)Math.Ceiling(nrOfPagesToDisplay / 2d) - 1;
                var below = (currentPage - middle);
                var above = (currentPage + middle);

                if (below < 4)
                {
                    above = nrOfPagesToDisplay;
                    below = 1;
                }
                else if (above > (pageCount - 4))
                {
                    above = pageCount;
                    below = (pageCount - nrOfPagesToDisplay + 1);
                }

                start = below;
                end = above;
            }

            if (start > 1)
            {
                sb.Append("<li>" + GeneratePageLink(baseUrl, "1", 1) + "</li>");
                if (start > 3)
                {
                    sb.Append("<li>" + GeneratePageLink(baseUrl, "2", 2) + "</li>");
                }
                if (start > 2)
                {
                    sb.Append("<li><a>...</a></li>");
                }
            }

            for (var i = start; i <= end; i++)
            {
                if (i == currentPage || (currentPage <= 0 && i == 0))
                {
                    sb.Append("<li class=\"active\">" + GenerateActivePageLink(i.ToString(), i) + "</li>");
                }
                else
                {
                    sb.Append("<li>" + GeneratePageLink(baseUrl, i.ToString(), i) + "</li>");
                }
            }
            if (end < pageCount)
            {
                if (end < pageCount - 1)
                {
                    sb.Append("<li><a>...</a></li>");
                }
                if (pageCount - 2 > end)
                {
                    sb.Append("<li>" + GeneratePageLink(baseUrl, (pageCount - 1).ToString(), pageCount - 1) + "</li>");
                }
                sb.Append("<li>" + GeneratePageLink(baseUrl, pageCount.ToString(), pageCount) + "</li>");
            }

            // Next
            sb.Append(currentPage < (pageCount + 1) ? "<li>" + GeneratePageLink(baseUrl, "&gt;", (currentPage + 1), "next") + "</li>" : string.Empty);
            //last page
            sb.Append(currentPage < (pageCount + 1) ? "<li>" + GeneratePageLink(baseUrl, "&gt;&gt;", pageCount + 1) + "</li>" : string.Empty);

            return new MvcHtmlString(sb.ToString());
        }
        private string GenerateFirstPageLink(string baseUrl, string content)
        {
            //remove sort and page from url if no sort is set
            string removePageParameter = GetPageParameterChar(baseUrl) + "page=";
            baseUrl = baseUrl.Replace(removePageParameter, "");

            return "<a href=\"" + baseUrl + "\" title=\"Första sidan\" >" + content + "</a>";
        }

        private string GenerateActivePageLink(string content, int index)
        {
            return "<a href=\"#\" title=\"Sida " + index + "\">" + content + "</a>";
        }

        private string GeneratePageLink(string baseUrl, string content, int index)
        {
            if (index == 1)
            {
                //remove sort and page from url if no sort is set
                baseUrl = baseUrl.Replace("?page=", "");
            }
            else
            {
                baseUrl = baseUrl + GetPageParameterChar(baseUrl) + "page=" + index;
            }

            return "<a href=\"" + baseUrl + "\" title=\"Sida " + index + "\" >" + content + "</a>";
        }

        private string GeneratePageLink(string baseUrl, string content, int index, string rel)
        {
            return "<a rel=\"" + rel + "\" href=\"" + baseUrl + GetPageParameterChar(baseUrl) + "page=" + index + "\" title=\"Sida " + index + "\" >" + content + "</a>";
        }

        private string GeneratePageLink(string baseUrl, string content, int index, bool isCurrent)
        {
            return "<a href=\"" + baseUrl + GetPageParameterChar(baseUrl) + "page=" + index + "\" title=\"Sida " + index + "\" class=\"" + (isCurrent ? "current" : string.Empty) + "\">" + content + "</a>";
        }

        private string GetPageParameterChar(string baseUrl)
        {
            return baseUrl.Contains("sort") || baseUrl.Contains("filter") ? "&" : "?";
        }
    }
}
