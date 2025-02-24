using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace InventoryManagement.Models.Entities
{
    public class Product
    {
        public int ProductId { get; set; }
        public string Name { get; set; } = string.Empty;
        public decimal Price { get; set; }
        public int StockQuantity { get; set; }
        public int CategoryId { get; set; }  // Foreign Key
        public int SupplierId { get; set; }  // Foreign Key
        public int SellerId { get; set; }    // Foreign Key (Seller who owns the product)
        public DateTime CreatedAt { get; set; }
    }
}
