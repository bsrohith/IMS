using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Dapper;
using InventoryManagement.Models.Entities;
using InventoryManagement.Repo.Data;
using InventoryManagement.Repo.Interfaces;
using InventoryManagementSystem.Models.ViewModel;

namespace InventoryManagement.Repo.Repository
{
    public class ProductRepository : IProductRepository
    {
        private readonly DapperDbContext _dbContext;

        public ProductRepository(DapperDbContext dbContext)
        {
            _dbContext = dbContext;
        }

        public async Task<IEnumerable<Products>> GetAllProductsAsync()
        {
            using var connection = _dbContext.CreateConnection();
            return await connection.QueryAsync<Products>("SELECT * FROM Products");
        }

        public async Task<Products> GetProductByIdAsync(int id)
        {
            using var connection = _dbContext.CreateConnection();
            return await connection.QueryFirstOrDefaultAsync<Products>("SELECT * FROM Products WHERE ProductId = @Id", new { ProductId = id });
        }

        public async Task AddProductAsync(Products product)
        {
            using var connection = _dbContext.CreateConnection();
            var query = "INSERT INTO Products (Name, Description, Price, StockQuantity, SellerId, CreatedAt) VALUES (@Name, @Description, @Price, @StockQuantity, @SellerId, @CreatedAt)";
            await connection.ExecuteAsync(query, product);
        }

        public async Task UpdateProductAsync(Products product)
        {
            using var connection = _dbContext.CreateConnection();
            var query = "UPDATE Products SET Name = @Name, Description = @Description, Price = @Price, StockQuantity = @StockQuantity WHERE Id = @Id";
            await connection.ExecuteAsync(query, product);
        }

        public async Task DeleteProductAsync(int id)
        {
            using var connection = _dbContext.CreateConnection();
            var query = "DELETE FROM Products WHERE Id = @Id";
            await connection.ExecuteAsync(query, new { Id = id });
        }

        public async Task<IEnumerable<ProductViewModel>> GetAllProductsWithDetailAsync()
        {
            using var connection = _dbContext.CreateConnection();
            return await connection.QueryAsync<ProductViewModel>("SELECT * FROM Products");
        }
    }
}
