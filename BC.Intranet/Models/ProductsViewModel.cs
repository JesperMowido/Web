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
        public int? ProductId { get; set; }

        [Required, Display(Name = "Responseble sales person")]
        public int SalesResponsibleId { get; set; }

        [Required, Display(Name = "Place")]
        public int PlaceId { get; set; }

        [Required, Display(Name = "Description")]
        public string Description { get; set; }

        [Display(Name = "Living space", Description = "Example: 44")]
        public int? LivingSpace { get; set; }

        [Display(Name = "Number of rooms")]
        public int? NumberOfRooms { get; set; }

        [Display(Name = "Price")]
        [DisplayFormat(DataFormatString = "{0:F2}", ApplyFormatInEditMode = true)]
        public decimal? Price { get; set; }

        [Display(Name = "Monthly charge")]
        [DisplayFormat(DataFormatString = "{0:F2}", ApplyFormatInEditMode = true)]
        public decimal? MonthlyCharge { get; set; }

        [Display(Name = "Square meter price")]
        [DisplayFormat(DataFormatString = "{0:F2}", ApplyFormatInEditMode = true)]
        public decimal? SquareMeterPrice { get; set; }

        [Display(Name ="Year built")]
        public DateTime? YearBuilt { get; set; }

        [Required, Display(Name = "External link")]
        public string ExternalLink { get; set; }

        [Display(Name = "External link for images")]
        public string ExternalPicLink { get; set; }

        [Required, Display(Name = "Address")]
        public string Address { get; set; }
        public string Lat { get; set; }
        public string Long { get; set; }

        [Required, Display(Name = "Approve property")]
        public bool IsApproved { get; set; }

        [Display(Name = "Ref Nr.")]
        public string RefNr { get; set; }

        [Display(Name = "Plot size")]
        public int? PlotSize { get; set; }

        [Display(Name = "Place")]
        public SelectList Places { get; set; }

        [Display(Name = "Responsible sales person")]
        public SelectList SalesPersons { get; set; }
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