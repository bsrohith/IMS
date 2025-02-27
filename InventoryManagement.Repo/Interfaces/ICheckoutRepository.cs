using InventoryManagement.Models.Entities;
using System;
using System.Collections.Generic;
using System.Data;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace InventoryManagement.Repo.Interfaces
{
    public interface ICheckoutRepository
    {
        Task AddToCheckoutAsync(CheckoutItem checkoutItem);
        Task<List<CheckoutItem>> GetCheckoutItemsAsync(int userId);
        Task RemoveFromCheckoutAsync(int checkoutId);
        Task ClearCheckoutAsync(int userId);
        Task<bool> UpdateCheckoutItemQuantityAsync(int checkoutId, int newQuantity, int productId);
        Task<List<CheckoutItem>> GetCheckoutItemsAsync(int userId, IDbTransaction transaction);
    }
}
