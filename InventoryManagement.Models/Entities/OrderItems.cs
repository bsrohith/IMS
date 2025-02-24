using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace InventoryManagement.Models.Entities
{
    public class OrderItems
    {
        public int Id { get; set; }
        public int OrderId { get; set; }  // Foreign Key
        public int ProductId { get; set; }  // Foreign Key
        public int Quantity { get; set; }
        public decimal UnitPrice { get; set; }
        public decimal TotalPrice => Quantity * UnitPrice;
    }
}
