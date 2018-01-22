using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace BC.Web.Models
{
    public class HomeViewModel : BaseViewModel
    {
        public List<TopPlaceItem> Countries { get; set; }
        public List<LatestProduct> LatestFirst { get; set; }
        public List<LatestProduct> LatestSecond { get; set; }
    }

    public class TopPlaceItem
    {
        public int Id { get; set; }
        public string PlaceName { get; set; }
        public List<TopPlaceItem> Cities { get; set; }
    }

    public class LatestProduct
    {
        public int Id { get; set; }
        public string PlaceName { get; set; }
        public string Address { get; set; }
        public string CreatedDate { get; set; }
        public string Description { get; set; }
        public string ImageUrl { get; set; }
    }
}