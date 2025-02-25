using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using InventoryManagement.Models.Entities;
using InventoryManagementSystem.Models.ViewModel;
namespace InventoryManagement.Repo.Interfaces
{
    public interface IProductRepository
    {
        Task<IEnumerable<Products>> GetAllProductsAsync();
        Task<Products> GetProductByIdAsync(int id);
        Task AddProductAsync(Products product);
        Task UpdateProductAsync(Products product);
        Task DeleteProductAsync(int id);
        Task<IEnumerable<ProductViewModel>> GetAllProductsWithDetailAsync();
    }
    
}
