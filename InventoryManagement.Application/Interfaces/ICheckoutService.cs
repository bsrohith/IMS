using InventoryManagement.Models.Entities;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace InventoryManagement.Application.Interfaces
{
    public interface ICheckoutService
    {
        Task AddToCheckoutAsync(CheckoutItem checkoutItem);
        Task<List<CheckoutItem>> GetCheckoutItemsAsync(int userId);
        Task RemoveFromCheckoutAsync(int checkoutId);
        Task ClearCheckoutAsync(int userId);
        Task<bool> UpdateCheckoutItemQuantityAsync(int checkoutId, int newQuantity, int productId);
    }
}
