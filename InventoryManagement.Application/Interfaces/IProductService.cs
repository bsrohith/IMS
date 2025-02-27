using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using InventoryManagement.Models.Entities;
using InventoryManagement.Models.ViewModel;

namespace InventoryManagement.Application.Interfaces
{
    public interface IProductService
    {
        Task<IEnumerable<Products>> GetAllProductsAsync();
        Task<Products> GetProductByIdAsync(int id);
        Task<int> AddProductAsync(Products product);
        Task UpdateProductAsync(Products product);
        Task DeleteProductAsync(int id);
        Task<List<ProductViewModel>> GetAllProductsWithDetailAsync();
        Task<ProductViewModel> GetProductViewModeByIdAsync(int id);
        Task UpdateProductStockQuantity(int productid, int quantity);
        Task<Suppliers> GetSupplierByUserIdAsync(int userId);

        Task<List<ProductViewModel>> GetAllProductsWithDetailWithUserAsync(int userId);


    }
}
