using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace InventoryManagement.Models.ViewModel
{
    public class InventoryTransactionItemViewModel
    {
        public int InventoryTransactionsId { get; set; }
        public int ProductId { get; set; }
        public int TotalQuantity { get; set; }
        public DateTime LastModifiedDate { get; set; }
        public string ProductName { get; set; }
        public string Category { get; set; }
        public string ProductDescription { get; set; } = string.Empty;
        public List<TransactionItemDetail> TransactionItems { get; set; }
    }

    public class TransactionItemDetail
    {
        public string TransactionType { get; set; }
        public DateTime TransactionDate { get; set; }
        public int Quantity { get; set; }
        public string TransactionsItemLog { get; set; } = string.Empty;
        public string UserName { get; set; }
    }
}
