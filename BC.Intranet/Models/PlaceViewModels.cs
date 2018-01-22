using Core.Entities;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Web;
using System.Web.Mvc;

namespace BC.Intranet.Models
{
    public class PlaceViewModels { }

    public class PlaceListViewModel : BaseViewModel
    {
        public List<Place> Places { get; set; }
    }

    public class PlaceViewModel : BaseViewModel
    {
        public int Id { get; set; }

        [Required]
        [Display(Name = "Place name")]
        public string Name { get; set; }

        [Display(Name = "Swedish place name")]
        public string NameSV { get; set; }

        [Required, Display(Name = "Place type")]
        public int PlaceTypeId { get; set; }

        [Display(Name = "Parent place")]
        public int? ParentId { get; set; }

        [Display(Name = "Place types")]
        public SelectList PlaceTypes { get; set; }

        [Display(Name = "Parent place")]
        public SelectList ParentPlaces { get; set; }
    }
}