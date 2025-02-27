using InventoryManagement.Application.Interfaces;
using InventoryManagement.Models.Entities;
using InventoryManagement.Models.ViewModel;
using InventoryManagement.Repo.Interfaces;

namespace InventoryManagement.Application.Services
{
    public class ProductService : IProductService
    {
        private readonly IProductRepository _productRepository;

        public ProductService(IProductRepository productRepository)
        {
            _productRepository = productRepository;
        }

        public async Task<IEnumerable<Products>> GetAllProductsAsync() => await _productRepository.GetAllProductsAsync();
        public async Task<Products> GetProductByIdAsync(int id) => await _productRepository.GetProductByIdAsync(id);
        public async Task<int> AddProductAsync(Products product) => await _productRepository.AddProductAsync(product);
        public async Task UpdateProductAsync(Products product) => await _productRepository.UpdateProductAsync(product);
        public async Task DeleteProductAsync(int id) => await _productRepository.DeleteProductAsync(id);

        public async Task<List<ProductViewModel>> GetAllProductsWithDetailAsync()
        {
           return (List<ProductViewModel>)await _productRepository.GetAllProductsWithDetailAsync();
        }

        public async Task<ProductViewModel> GetProductViewModeByIdAsync(int id)
        {
            return await _productRepository.GetProductViewModeByIdAsync(id);
        }

        public async Task UpdateProductStockQuantity(int productid, int quantity)
        {
            await _productRepository.UpdateProductStockQuantity(productid, quantity);
        }

        public async Task<List<ProductViewModel>> GetAllProductsWithDetailWithUserAsync(int userId)
        {
           return (List<ProductViewModel>)await _productRepository.GetAllProductsWithDetailWithUserAsync(userId);
        }

        public async Task<Suppliers> GetSupplierByUserIdAsync(int userId)
        {
            var suppliers = await _productRepository.GetSupplierByUserIdAsync(userId);
            // Return the first supplier that matches the userId or null if none is found
            return suppliers.FirstOrDefault();
        }

    }
}
