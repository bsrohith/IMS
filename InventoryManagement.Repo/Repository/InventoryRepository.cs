using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using InventoryManagement.Models.Entities;
using InventoryManagement.Repo.Interfaces;

namespace InventoryManagement.Repo.Repository
{
    public class InventoryRepository : IInventoryRepository
    {
        private readonly DapperDbContext _dbContext;

        public InventoryRepository(DapperDbContext dbContext)
        {
            _dbContext = dbContext;
        }

        public async Task<IEnumerable<InventoryTransactions>> GetInventoryTransactions()
        {
            using var connection = _dbContext.CreateConnection();
            return await connection.QueryAsync<InventoryTransactions>("SELECT * FROM InventoryTransactions");
        }

        public async Task<bool> AddInventoryTransaction(InventoryTransactions transaction)
        {
            using var connection = _dbContext.CreateConnection();
            var sql = @"INSERT INTO InventoryTransactions (ProductId, TransactionType, Quantity, TransactionDate)
                        VALUES (@ProductId, @TransactionType, @Quantity, GETDATE())";
            return await connection.ExecuteAsync(sql, transaction) > 0;
        }
    }
}
