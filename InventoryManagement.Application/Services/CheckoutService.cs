using InventoryManagement.Application.Interfaces;
using InventoryManagement.Models.Entities;
using InventoryManagement.Repo.Interfaces;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace InventoryManagement.Application.Services
{
    public class CheckoutService : ICheckoutService
    {
        private readonly ICheckoutRepository _checkoutRepository;
        public CheckoutService(ICheckoutRepository checkoutRepository)
        {
            _checkoutRepository = checkoutRepository;
        }
        public async Task AddToCheckoutAsync(CheckoutItem checkoutItem)
        {
            await _checkoutRepository.AddToCheckoutAsync(checkoutItem);
        }

        public async Task ClearCheckoutAsync(int userId)
        {
            await _checkoutRepository.ClearCheckoutAsync(userId);
        }

        public async Task<List<CheckoutItem>> GetCheckoutItemsAsync(int userId)
        {
            return await _checkoutRepository.GetCheckoutItemsAsync(userId);
        }

        public async Task RemoveFromCheckoutAsync(int checkoutId)
        {
            await _checkoutRepository.RemoveFromCheckoutAsync(checkoutId);
        }

        public async Task<bool> UpdateCheckoutItemQuantityAsync(int checkoutId, int newQuantity, int productId)
        {
            return await _checkoutRepository.UpdateCheckoutItemQuantityAsync(checkoutId, newQuantity, productId);
        }
    }
}
