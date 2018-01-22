using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace BC.Web.Models
{
    public class ProductViewModel : BaseViewModel
    {
        public string Address { get; set; }
        public string PlaceHeader { get; set; }
        public string Description { get; set; }
        public string ExternalLink { get; set; }
        public string ExternalPicLink { get; set; }
        public string Price { get; set; }
        public int? LivingSpace { get; set; }
        public int? PlotSize { get; set; }
        public int? NumberOfRooms { get; set; }
        public string MonthlyCharge { get; set; }
        public string SquareMeterPrice { get; set; }
        public int? YearBuilt { get; set; }
        public string SalesResponsibleName { get; set; }
        public string BrokerDescription { get; set; }
        public string TopImage { get; set; }
        public List<string> Images { get; set; }
        public string Created { get; set; }
        public string Lat { get; set; }
        public string Long { get; set; }
        public string Url { get; set; }
        public int CustomerId { get; set; }

        private bool showMap = false;
        public bool ShowMap
        {
            get { return showMap; }
            set { showMap = value; }
        }
    }
}