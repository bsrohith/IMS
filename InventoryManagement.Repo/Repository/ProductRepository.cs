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
using Microsoft.Data.SqlClient;
using Microsoft.Extensions.Configuration;

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
            return await connection.QueryFirstOrDefaultAsync<Products>("SELECT * FROM Products WHERE ProductId = @Id", new { Id = id });
        }

        public async Task<int> AddProductAsync(Products product)
        {
            using var connection = _dbContext.CreateConnection();
            var query = @" INSERT INTO Products (ProductName, Price, StockQuantity, CategoryId, SupplierId, CreatedAt) 
                           VALUES (@ProductName, @Price, @StockQuantity, @CategoryId, @SupplierId, @CreatedAt);
                           SELECT CAST(SCOPE_IDENTITY() as int);";
            return await connection.QuerySingleAsync<int>(query, product);
        }

        public async Task UpdateProductAsync(Products product)
        {
            using var connection = _dbContext.CreateConnection();
            var query = "UPDATE Products SET ProductName=@ProductName, Price=@Price, StockQuantity=@StockQuantity, CategoryId=@CategoryId, SupplierId=@SupplierId WHERE ProductId=@ProductId";
            await connection.ExecuteAsync(query, product);
        }

        public async Task DeleteProductAsync(int id)
        {
            using var connection = _dbContext.CreateConnection();
            var query = "DELETE FROM Products WHERE ProductId = @ProductId";
            await connection.ExecuteAsync(query, new { ProductId = id });
        }

        public async Task<IEnumerable<ProductViewModel>> GetAllProductsWithDetailAsync()
        {
            using var connection = _dbContext.CreateConnection();
            var query = @"SELECT p.ProductId, p.ProductName, p.Price, p.StockQuantity, 
                               p.CategoryId, p.SupplierId, p.CreatedAt, 
                               pd.ProductDescription, pd.ProductPhoto
                        FROM Products p
                        LEFT JOIN ProductDetails pd ON p.ProductId = pd.ProductId";
            return await connection.QueryAsync<ProductViewModel>(query);

        }

        public async Task<IEnumerable<ProductViewModel>> GetAllProductsWithDetailWithUserAsync(int userId)
        {
            using var connection = _dbContext.CreateConnection();

            var query = @"SELECT p.ProductId, p.ProductName, p.Price, p.StockQuantity, 
                         p.CategoryId, p.SupplierId, p.CreatedAt, 
                         pd.ProductDescription, pd.ProductPhoto
                  FROM Products p
                  LEFT JOIN ProductDetails pd ON p.ProductId = pd.ProductId
                  WHERE p.UserId = @UserId"; // Fetch only the user's products

            return await connection.QueryAsync<ProductViewModel>(query, new { UserId = userId });
        }


        public async Task<ProductViewModel> GetProductViewModeByIdAsync(int id)
        {
            using var connection = _dbContext.CreateConnection();
            var query = @"SELECT p.ProductId, p.ProductName, p.Price, p.StockQuantity, 
                               p.CategoryId, p.SupplierId, p.CreatedAt, 
                               pd.ProductDescription, pd.ProductPhoto
                        FROM Products p
                        LEFT JOIN ProductDetails pd ON p.ProductId = pd.ProductId
                        WHERE p.ProductId = @ProductId";

            return await connection.QueryFirstOrDefaultAsync<ProductViewModel>(query, new { ProductId = id });
        }

        public async Task<int> GetStockQuantity(int productId)
        {
            using var connection = _dbContext.CreateConnection();
            // Get stock quantity for the product
            return await connection.ExecuteScalarAsync<int>(
                 "SELECT StockQuantity FROM Products WHERE ProductId = @ProductId",
                 new { ProductId = productId });
        }

        public async Task UpdateProductStockQuantity(int productid,int quantity)
        {
            using var connection = _dbContext.CreateConnection();
            var query = "UPDATE Products SET StockQuantity=@StockQuantity WHERE ProductId=@ProductId";
            await connection.ExecuteAsync(query, new { StockQuantity  = quantity, ProductId = productid });
        }
    }
}
