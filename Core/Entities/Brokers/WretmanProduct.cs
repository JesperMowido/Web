﻿using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Core.Entities
{
    public class WretmanProduct
    {
        public int Id { get; set; }
        public string Price { get; set; }
        public string Place { get; set; }
        public string Spaces { get; set; }
        public string MonthlyPrice { get; set; }
        public string ImageUrl1 { get; set; }
        public string ImageUrl2 { get; set; }
        public string ImageUrl3 { get; set; }
        public string ImageUrl4 { get; set; }
        public string ImageUrl5 { get; set; }
        public string Description { get; set; }
        public string Address { get; set; }
        public string ExternalLink { get; set; }
    }
}
