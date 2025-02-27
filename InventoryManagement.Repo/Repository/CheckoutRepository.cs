using Dapper;
using InventoryManagement.Models.Entities;
using InventoryManagement.Repo.Data;
using InventoryManagement.Repo.Interfaces;
using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Data;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace InventoryManagement.Repo.Repository
{
    public class CheckoutRepository : ICheckoutRepository
    {
        private readonly DapperDbContext _dbContext;
        private readonly IProductRepository _productRepository;

        public CheckoutRepository(DapperDbContext dbContext, IProductRepository productRepository)
        {
            _dbContext = dbContext;
            _productRepository = productRepository;
        }

        public async Task AddToCheckoutAsync(CheckoutItem checkoutItem)
        {
            using var connection = _dbContext.CreateConnection();

            var query = @"INSERT INTO Checkout (UserId, ProductId, Quantity, Price) 
                  VALUES (@UserId, @ProductId, @Quantity, @Price)";

            await connection.ExecuteAsync(query, checkoutItem);
        }

        public async Task ClearCheckoutAsync(int userId)
        {
            using var connection = _dbContext.CreateConnection();

            var query = "DELETE FROM Checkout WHERE UserId = @UserId";
            await connection.ExecuteAsync(query, new { UserId = userId });
        }

        public async Task<List<CheckoutItem>> GetCheckoutItemsAsync(int userId)
        {
            using var connection = _dbContext.CreateConnection();
            var query = @"SELECT c.*, p.ProductName, p.StockQuantity
                      FROM Checkout c JOIN Products p ON c.ProductId = p.ProductId
                      WHERE c.UserId = @UserId";

            return (await connection.QueryAsync<CheckoutItem>(query, new { UserId = userId })).ToList();
        }

        public async Task<List<CheckoutItem>> GetCheckoutItemsAsync(int userId, IDbTransaction transaction)
        {
            var query = @"SELECT c.*, p.ProductName, p.StockQuantity
                      FROM Checkout c JOIN Products p ON c.ProductId = p.ProductId
                      WHERE c.UserId = @UserId";

            return (await transaction.Connection.QueryAsync<CheckoutItem>(query, new { UserId = userId }, transaction)).ToList();
        }

        public async Task RemoveFromCheckoutAsync(int checkoutId)
        {
            using var connection = _dbContext.CreateConnection();

            var query = "DELETE FROM Checkout WHERE CheckoutId = @CheckoutId";
            await connection.ExecuteAsync(query, new { CheckoutId = checkoutId });
        }

        public async Task<bool> UpdateCheckoutItemQuantityAsync(int checkoutId, int newQuantity, int productId)
        {
            using var connection = _dbContext.CreateConnection();

            // Get available stock
            var stockAvailable = await _productRepository.GetStockQuantity(productId);

            // Ensure new quantity is valid
            if (newQuantity < 1 || newQuantity > stockAvailable)
            {
                return false;
            }

            // Update checkout quantity
            var query = "UPDATE Checkout SET Quantity = @NewQuantity WHERE CheckoutId = @CheckoutId";
            await connection.ExecuteAsync(query, new { CheckoutId = checkoutId, NewQuantity = newQuantity });

            return true;
        }
    }
}
