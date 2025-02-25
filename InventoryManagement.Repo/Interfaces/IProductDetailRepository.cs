using InventoryManagement.Models.Entities;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace InventoryManagement.Repo.Interfaces
{
    public interface IProductDetailRepository
    {
        Task AddProductDetailAsync(ProductDetails productDetail);
        Task UpdateProductDetailAsync(ProductDetails productDetail);
        Task DeleteProductDetailAsync(int id);

    }
}
