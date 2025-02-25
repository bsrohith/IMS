using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using InventoryManagement.Models.Entities;

namespace InventoryManagement.Application.Interfaces
{
    public interface IInventoryService
    {
        Task<IEnumerable<InventoryTransactions>> GetInventoryTransactions();
        Task<bool> AddInventoryTransaction(InventoryTransactions transaction);
    }
}
