using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace InventoryManagement.Models.ViewModel
{
    public class InventoryTransactionViewModel
    {
        public int InventoryTransactionsId { get; set; }
        public int ProductId { get; set; }
        public int TotalQuantity { get; set; }
        public DateTime LastModifiedDate { get; set; }
        public string ProductName { get;set; }
        public string Category { get; set; }
    }
}
