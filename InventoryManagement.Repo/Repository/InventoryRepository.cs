using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Dapper;
using InventoryManagement.Models.Entities;
using InventoryManagement.Models.ViewModel;
using InventoryManagement.Repo.Data;
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
        public async Task<IEnumerable<InventoryTransactionViewModel>> GetInventoryTransactionDetails()
        {
            using var connection = _dbContext.CreateConnection();

            var sql = @" SELECT  IT.InventoryTransactionsId, IT.ProductId, IT.TotalQuantity,
                            CAST(IT.LastModifiedDate AS DATE) AS LastModifiedDate, -- Only date part
                            P.ProductName, C.CategoryName AS Category
                        FROM InventoryTransactions IT
                        JOIN Products P ON IT.ProductId = P.ProductId
                        JOIN Categories C ON P.CategoryId = C.CategoryId;";

            return await connection.QueryAsync<InventoryTransactionViewModel>(sql);
        }

        public async Task<InventoryTransactionItemViewModel> GetInventoryTransactionById(int inventoryid)
        {
            using var connection = _dbContext.CreateConnection();
            var sql = @"SELECT it.InventoryTransactionsId,it.ProductId, it.TotalQuantity,
                        CONVERT(DATE, it.LastModifiedDate) AS LastModifiedDate, p.ProductName, c.CategoryName AS Category,
                        ISNULL(pd.ProductDescription, '') AS ProductDescription,
                        ISNULL(pd.ProductPhoto, '') AS ProductPhoto, iti.InventoryTranactionItemsId, iti.TransactionType,
                        iti.TransactionDate,iti.Quantity,ISNULL(iti.TransactionsItemLog, '') AS TransactionsItemLog, u.UserName
                        FROM InventoryTransactions it
                        JOIN InventoryTransactionsItems iti ON it.InventoryTransactionsId = iti.InventoryTransactionsId
                        JOIN Products p ON it.ProductId = p.ProductId
                        LEFT JOIN ProductDetails pd ON p.ProductId = pd.ProductId
                        JOIN Categories c ON p.CategoryId = c.CategoryId
                        JOIN Users u ON iti.UserId = u.UserId
                        WHERE it.InventoryTransactionsId = @InventoryTransactionsId
                        ORDER BY it.LastModifiedDate DESC, iti.TransactionDate DESC;";

            var transactionDictionary = new Dictionary<int, InventoryTransactionItemViewModel>();

            var result = await connection.QueryAsync<InventoryTransactionItemViewModel, TransactionItemDetail, InventoryTransactionItemViewModel>(
                sql, (transaction, item) =>
                {
                    if (!transactionDictionary.TryGetValue(transaction.InventoryTransactionsId, out var transactionEntry))
                    {
                        transactionEntry = transaction;
                        transactionEntry.TransactionItems = new List<TransactionItemDetail>();
                        transactionDictionary.Add(transaction.InventoryTransactionsId, transactionEntry);
                    }

                    transactionEntry.TransactionItems.Add(item);
                    return transactionEntry;
                },
                new { InventoryTransactionsId = inventoryid },
                splitOn: "InventoryTranactionItemsId"
            );

            return transactionDictionary.Values.FirstOrDefault();
        }
    }
}
