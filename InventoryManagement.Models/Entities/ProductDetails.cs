﻿using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace InventoryManagement.Models.Entities
{
    public class ProductDetails
    {
        public int ProductDetailId { get; set; }
        public string ProductDescription { get; set; } = string.Empty;
        public string ProductPhoto { get; set; } = string.Empty;
        public int ProductId { get; set; }
    }
}
