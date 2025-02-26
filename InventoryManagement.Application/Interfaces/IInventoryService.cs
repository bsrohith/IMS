using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using InventoryManagement.Models.Entities;
using InventoryManagement.Models.ViewModel;

namespace InventoryManagement.Application.Interfaces
{
    public interface IInventoryService
    {
        Task<IEnumerable<InventoryTransactions>> GetInventoryTransactions();
        Task<bool> AddInventoryTransaction(InventoryTransactions transaction);
        Task<List<InventoryTransactionViewModel>> GetInventoryTransactionDetails();
        Task<InventoryTransactionItemViewModel> GetInventoryTransactionById(int inventoryid);
        Task AddInventoryTransaction(InventoryTransactionQueryViewModel inventoryTransactionQueryViewModel);
        Task UpdateInventoryTransaction(InventoryTransactionQueryViewModel inventoryTransactionQueryViewModel);
    }
}
