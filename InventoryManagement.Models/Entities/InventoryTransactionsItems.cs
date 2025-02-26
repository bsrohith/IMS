using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace InventoryManagement.Models.Entities
{
    public class InventoryTransactionsItems
    {
        public int InventoryTranactionItemsId { get; set; }
        public string TransactionType { get; set; } // "Added", "Sold", "Updated" 
        public DateTime TransactionDate { get; set; }
        public int Quantity { get; set; }
        public string TransactionsItemLog { get; set; } = string.Empty;
        public int InventoryTransactionsId { get; set; }// Foreign Key
        public int UserId { get; set; } //Foreign key
    }
}
