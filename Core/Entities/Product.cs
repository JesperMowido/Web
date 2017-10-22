using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Core.Entities
{
    public class Product
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

        public virtual ProductType ProductType { get; set; }
        public virtual User SalesResponsible { get; set; }
        public virtual Place Place { get; set; }

    }
}
