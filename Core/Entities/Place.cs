using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Core.Entities
{
    public class Place
    {
        public int Id { get; set; }
        public int? ParentId { get; set; }
        public int PlaceTypeId { get; set; }
        public string Name { get; set; }
        public string Lat { get; set; }
        public string Long { get; set; }

        public virtual PlaceType PlaceType { get; set; }
    }
}
