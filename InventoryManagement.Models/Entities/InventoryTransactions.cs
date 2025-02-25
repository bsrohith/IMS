﻿using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace InventoryManagement.Models.Entities
{
    public class InventoryTransactions
    {
        public int InventoryTransactionsId { get; set; }
        public int ProductId { get; set; } // Foreign Key
        public int TotalQuantity { get; set; }
        public DateTime LastModifiedDate { get; set; }
    }
}
