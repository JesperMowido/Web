using Core.Entities;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace BC.Intranet.Models
{
    public class ImportViewModels : BaseViewModel
    {
    }

    public class SkandiaImportViewModel : BaseViewModel
    {
        public List<ProductItem> Products { get; set; }
        public int NumberOfProducts { get; set; }
    }

    public class WretmanImportViewModel : BaseViewModel
    {
        public List<ProductItem> Products { get; set; }
        public int NumberOfProducts { get; set; }
    }

    public class PortugalImportViewModel : BaseViewModel
    {
        public List<ProductItem> Products { get; set; }
        public int NumberOfProducts { get; set; }
    }

    public class NordicFranceViewModel : BaseViewModel
    {
        public List<ProductItem> Products { get; set; }
        public int NumberOfProducts { get; set; }
    }

    public class CCHomesViewModel : BaseViewModel
    {
        public List<ProductItem> Products { get; set; }
        public int NumberOfProducts { get; set; }
    }

    public class ProductItem
    {
        public int Id { get; set; }
        public int ProductTypeId { get; set; }
        public int SalesResponsibleId { get; set; }
        public int PlaceId { get; set; }
        public string Description { get; set; }
        public int? LivingSpace { get; set; }
        public int? PlotSize { get; set; }
        public int? NumberOfRooms { get; set; }
        public decimal? Price { get; set; }
        public decimal? MonthlyCharge { get; set; }
        public decimal? SquareMeterPrice { get; set; }
        public DateTime? YearBuilt { get; set; }
        public string ExternalLink { get; set; }
        public string ExternalPicLink { get; set; }
        public DateTime InsertDate { get; set; }
        public string Address { get; set; }
        public string Lat { get; set; }
        public string Long { get; set; }
        public bool IsApproved { get; set; }
        public string RefNr { get; set; }
        public string ThumbnailUrl { get; set; }

        public string DisplayPlace { get; set; }
        public string ImageUrl1 { get; set; }
        public string ImageUrl2 { get; set; }
        public string ImageUrl3 { get; set; }
        public string ImageUrl4 { get; set; }
        public string ImageUrl5 { get; set; }
    }
}