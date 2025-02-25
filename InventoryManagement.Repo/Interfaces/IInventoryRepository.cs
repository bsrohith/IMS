using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using InventoryManagement.Models.Entities;
using InventoryManagement.Models.ViewModel;

namespace InventoryManagement.Repo.Interfaces
{
    public interface IInventoryRepository
    {
        Task<IEnumerable<InventoryTransactions>> GetInventoryTransactions();
        Task<bool> AddInventoryTransaction(InventoryTransactions transaction);
        Task<IEnumerable<InventoryTransactionViewModel>> GetInventoryTransactionDetails();
        Task<InventoryTransactionItemViewModel> GetInventoryTransactionById(int inventoryid);
    }
}
