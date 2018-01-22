using Core.Entities;
using Microsoft.Azure.Search;
using Microsoft.Azure.Search.Models;
using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace BC.Web.Models
{
    public class SearchViewModel : BaseViewModel
    {
        public int PlaceId { get; set; }
        public string PlaceName { get; set; }
        public List<SearchProductItem> Products { get; set; }
        public string MapObjects { get; set; }
        public int ProductCount { get; set; }
        public string Url { get; set; }
        public List<PlaceItem> Countries { get; set; }
        public int PageSize { get; set; }
        public int PageIndex { get; set; }
        public int PageStart { get; set; }
        public int PageStop { get; set; }
        public string PlaceDescription { get; set; }
        public string PlaceDescTitle { get; set; }
        public string SearchSort { get; set; }
        public string SearchFilter { get; set; }
    }

    public class ListAllPlacesViewModel : BaseViewModel
    {
        public List<PlaceItem> Countries { get; set; }
        public List<PlaceItem> Areas { get; set; }
        public List<PlaceItem> Cities { get; set; }
        public List<PlaceItem> Districts { get; set; }
    }

    public class PlaceItem
    {
        public int Id { get; set; }
        public string DisplayName { get; set; }
        public string Url { get; set; }
    }

    public class SearchProductItem
    {
        public int Id { get; set; }
        public string Address { get; set; }
        public string PlaceHeader { get; set; }
        public string Description { get; set; }
        public string ExternalLink { get; set; }
        public string Price { get; set; }
        public int? LivingSpace { get; set; }
        public int? NumberOfRooms { get; set; }
        public string ThumbnailImg { get; set; }
        public string Created { get; set; }
        public string Lat { get; set; }
        public string Long { get; set; }
        public string Url { get; set; }
        public string ThumbnailUrl { get; set; }
        public int CustomerId { get; set; }

        private bool showMap = false;
        public bool ShowMap
        {
            get { return showMap; }
            set { showMap = value; }
        }
    }

    
}