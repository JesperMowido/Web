
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Microsoft.Azure.Search;
using Microsoft.Azure.Search.Models;
using Microsoft.Spatial;
using Newtonsoft.Json;

namespace BC.Search.SearchModel
{
    [SerializePropertyNamesAsCamelCase]
    public partial class SearchProduct
    {
        [System.ComponentModel.DataAnnotations.Key]
        [JsonProperty("ID")]
        public int Id { get; set; }

        [JsonProperty("FKProductTypeID")]
        public int ProductTypeId { get; set; }

        [JsonProperty("SalesResponsibleID")]
        public int SalesResponsibleId { get; set; }

        [JsonProperty("FKPlaceID"), IsSearchable, IsFilterable, IsSortable]
        public string PlaceId { get; set; }

        public string Description { get; set; }

        [IsSortable, IsFilterable]
        public int? LivingSpace { get; set; }
        public int? PlotSize { get; set; }

        [IsSortable, IsFilterable]
        public int? NumberOfRooms { get; set; }

        [IsSortable, IsFilterable]
        public string Price { get; set; }

        [IsSortable, IsFilterable]
        public string MonthlyCharge { get; set; }

        [IsSortable, IsFilterable]
        public string SquareMeterPrice { get; set; }
        public DateTime? YearBuilt { get; set; }
        public string ExternalLink { get; set; }

        [IsSortable]
        public DateTime InsertDate { get; set; }
        public string Address { get; set; }
        public string Lat { get; set; }
        public string Long { get; set; }
        public string ThumbnailUrl { get; set; }

        [IsSortable, IsFilterable]
        public double? PriceFilter { get; set; }
    }
}
