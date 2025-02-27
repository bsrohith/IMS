using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace InventoryManagement.Models.ViewModel
{
    public class OrderItemSellerViewModel
    {
        public int OrderItemId { get; set; }
        public int OrderId { get; set; }
        public int ProductId { get; set; }
        public string ProductName { get; set; } = string.Empty;
        public string ProductDescription { get; set; } = string.Empty;
        public string ProductPhoto { get; set; } = string.Empty;
        public int Quantity { get; set; }
        public decimal UnitPrice { get; set; }
        public decimal TotalPrice => Quantity * UnitPrice;
        public string OrderItemStatus { get; set; }
        public int BuyerId { get; set; }
        public string BuyerName { get; set; } = string.Empty;
        public string BuyerEmail { get; set; } = string.Empty;
        public int SellerId { get; set; }
        public string SellerName { get; set; } = string.Empty;
        public string SellerEmail { get; set; } = string.Empty;
        public DateTime OrderDate { get; set; }
    }
}
