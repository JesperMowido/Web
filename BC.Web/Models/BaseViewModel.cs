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
                    _pageTitle = "Admin - Mowido";

                    return _pageTitle;
            }
            set { _pageTitle = value; }
        }

        public string Header { get; set; }
    }
}