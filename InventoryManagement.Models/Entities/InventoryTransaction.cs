using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace InventoryManagement.Models.Entities
{
    public class InventoryTransaction
    {
        public int Id { get; set; }
        public int ProductId { get; set; } // Foreign Key
        public string TransactionType { get; set; } // "Added", "Sold", "Updated"
        public int Quantity { get; set; }
        public DateTime TransactionDate { get; set; }
    }
}
