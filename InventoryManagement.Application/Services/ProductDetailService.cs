using InventoryManagement.Application.Interfaces;
using InventoryManagement.Models.Entities;
using InventoryManagement.Repo.Interfaces;
using InventoryManagement.Repo.Repository;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace InventoryManagement.Application.Services
{
    public class ProductDetailService : IProductDetailService
    {
        private readonly IProductDetailRepository _repository;
        public ProductDetailService(IProductDetailRepository repository)
        {
            _repository = repository;
        }
        public async Task AddProductDetailAsync(ProductDetails productDetail)
        {
            await _repository.AddProductDetailAsync(productDetail);
        }

        public async Task UpdateProductDetailAsync(ProductDetails productDetail)
        {
            await _repository.UpdateProductDetailAsync(productDetail);
        }
        public async Task DeleteProductDetailAsync(int id) => await _repository.DeleteProductDetailAsync(id);

    }
}
