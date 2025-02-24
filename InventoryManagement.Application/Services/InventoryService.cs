using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using InventoryManagement.Application.Interfaces;
using InventoryManagement.Models.Entities;
using InventoryManagement.Repo.Interfaces;

namespace InventoryManagement.Application.Services
{
    public class InventoryService : IInventoryService
    {
        private readonly IInventoryRepository _inventoryRepository;

        public InventoryService(IInventoryRepository inventoryRepository)
        {
            _inventoryRepository = inventoryRepository;
        }

        public async Task<IEnumerable<InventoryTransactions>> GetInventoryTransactions()
            => await _inventoryRepository.GetInventoryTransactions();

        public async Task<bool> AddInventoryTransaction(InventoryTransactions transaction)
            => await _inventoryRepository.AddInventoryTransaction(transaction);
    }
}
