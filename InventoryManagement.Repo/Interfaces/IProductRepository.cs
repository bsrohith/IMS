using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using InventoryManagement.Models.Entities;
using InventoryManagement.Models.ViewModel;
namespace InventoryManagement.Repo.Interfaces
{
    public interface IProductRepository
    {
        Task<IEnumerable<Products>> GetAllProductsAsync();
        Task<Products> GetProductByIdAsync(int id);
        Task<int> AddProductAsync(Products product);
        Task UpdateProductAsync(Products product);
        Task DeleteProductAsync(int id);
        Task<IEnumerable<ProductViewModel>> GetAllProductsWithDetailAsync();
        Task<ProductViewModel> GetProductViewModeByIdAsync(int id);
        Task<int> GetStockQuantity(int productId);
        Task UpdateProductStockQuantity(int productid, int quantity);
        Task<IEnumerable<ProductViewModel>> GetAllProductsWithDetailWithUserAsync(int userId);

        Task<List<Suppliers>> GetSupplierByUserIdAsync(int userId);

        

    }

}
