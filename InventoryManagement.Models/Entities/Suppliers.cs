using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace InventoryManagement.Models.Entities
{
    public class Suppliers
    {
        public int SupplierId { get; set; }
        public string SupplierName { get; set; } = string.Empty;
        public string ContactEmail { get; set; } = string.Empty;
        public string PhoneNumber { get; set; } = string.Empty;

        public int UserId { get; set; }
    }
}
