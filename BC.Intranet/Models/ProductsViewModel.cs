using Core.Entities;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Web;
using System.Web.Mvc;

namespace BC.Intranet.Models
{
    public class ProductsViewModel { }

    public class ProductListViewModel : BaseViewModel
    {
        public List<Product> Products { get; set; }
    }

    public class ProductViewModel : BaseViewModel
    {
        [Required, Display(Name = "Product Type")]
        public int ProductTypeId { get; set; }

        [Required, Display(Name = "Responseble sales person")]
        public int SalesResponsibleId { get; set; }

        [Required, Display(Name = "Place")]
        public int PlaceId { get; set; }

        [Required, Display(Name = "Description")]
        public string Description { get; set; }
        public int? LivingSpace { get; set; }
        public int? NumberOfRooms { get; set; }
        public decimal? Price { get; set; }
        public decimal? MonthlyCharge { get; set; }
        public decimal? SquareMeterPrice { get; set; }
        public DateTime? YearBuilt { get; set; }

        [Required, Display(Name = "External link")]
        public string ExternalLink { get; set; }

        [Required, Display(Name = "Address")]
        public string Address { get; set; }
        public string Lat { get; set; }
        public string Long { get; set; }

        [Display(Name = "Approve property")]
        public bool IsApproved { get; set; }

        [Display(Name = "Area or...")]
        public SelectList PlaceAreas { get; set; }

        [Display(Name = "...City")]
        public SelectList PlaceCities { get; set; }

        [Display(Name = "Responsible sales person")]
        public SelectList SalesPersons { get; set; }

        [Display(Name = "Product type")]
        public SelectList ProductTypes { get; set; }

    }

    public class ProductTypeViewModel : BaseViewModel
    {
        [Required, Display(Name = "Name of Product Type")]
        public int ProductTypeId { get; set; }
    }

    public class ProductImageViewModel : BaseViewModel
    {
        public int ProductId { get; set; }
        public List<string> Blobs { get; set; }
    }
}