using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace InventoryManagement.Models.ViewModel
{
    public class InventoryTransactionQueryViewModel
    {
        public int ProductId { get; set; }
        public int Quantity { get; set; }
        //just for SP parameter
        public int NewQuantity { get; set; }
        public int UserId { get; set; }
        public string TransactionType { get; set; } = string.Empty;
        public string TransactionLog { get; set; } = string.Empty;
    }
}
