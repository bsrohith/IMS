using InventoryManagement.Common;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace InventoryManagement.Models.Entities
{
    public class Orders
    {
        public int OrderId { get; set; }
        public int UserId { get; set; }  // Foreign Key
        public DateTime OrderDate { get; set; }
        public decimal TotalAmount { get; set; }
        public string OrderStatus { get; set; } = CommonStrings.OrderCreated;
    }
}
