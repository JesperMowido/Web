using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace BC.Web.Models
{
    public class BaseViewModel
    {
        private string _pageTitle;

        public string PageTitle
        {
            get
            {
                if (string.IsNullOrEmpty(_pageTitle))
                    _pageTitle = Resources.Resources.Header_Default + " - Mowido";

                    return _pageTitle;
            }
            set { _pageTitle = value; }
        }

        public string Header { get; set; }

        private string _host;

        public string Host
        {
            get
            {
                if (string.IsNullOrEmpty(_host))
                    _host = HttpContext.Current.Request.Url.Host;
                return _host;
            }
            set { _host = value; }
        }

        public List<BreadCrumbItem> BreadCrumbs { get; set; }
    }

    public class BreadCrumbItem
    {
        public string Name { get; set; }
        public string Url { get; set; }
        public string FullName { get; set; }
    }
}