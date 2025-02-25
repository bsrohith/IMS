using Dapper;
using InventoryManagement.Models.Entities;
using InventoryManagement.Repo.Data;
using InventoryManagement.Repo.Interfaces;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace InventoryManagement.Repo.Repository
{
    public class ProductDetailRepository : IProductDetailRepository
    {
        private readonly DapperDbContext _dbContext;

        public ProductDetailRepository(DapperDbContext dapperDbContext)
        {
            _dbContext = dapperDbContext;
        }

        public async Task AddProductDetailAsync(ProductDetails productDetail)
        {
            using var connection = _dbContext.CreateConnection();
            var query = "INSERT INTO ProductDetails (ProductId, ProductDescription, ProductPhoto) VALUES (@ProductId, @ProductDescription, @ProductPhoto)";
            await connection.ExecuteAsync(query, productDetail);
        }

        public async Task UpdateProductDetailAsync(ProductDetails productDetail)
        {
            using var connection = _dbContext.CreateConnection();
            var query = "UPDATE ProductDetails SET ProductDescription=@ProductDescription, ProductPhoto=@ProductPhoto WHERE ProductDetailId=@ProductDetailId";
            await connection.ExecuteAsync(query, productDetail);
        }

        public async Task DeleteProductDetailAsync(int id)
        {
            using var connection = _dbContext.CreateConnection();
            var query = "DELETE FROM ProductDetails WHERE ProductId = @ProductId";
            await connection.ExecuteAsync(query, new { ProductId = id });
        }
    }
}
