using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using InventoryManagement.Application.Interfaces;
using InventoryManagement.Models.Entities;
using InventoryManagement.Repo.Interfaces;
using Microsoft.AspNetCore.Http;

namespace InventoryManagement.Application.Services
{
    public class OrderService : IOrderService
    {
        private readonly IOrderRepository _orderRepository;
        private readonly IUserService _userService;

        public OrderService(IOrderRepository orderRepository, IUserService userService)
        {
            _orderRepository = orderRepository;
            _userService = userService;
        }

        public async Task<IEnumerable<Orders>> GetAllOrders() => await _orderRepository.GetAllOrders();

        public async Task<Orders> GetOrderById(int id) => await _orderRepository.GetOrderById(id);

        public async Task<int> CreateOrder(Orders order) => await _orderRepository.CreateOrder(order);

        public async Task<bool> UpdateOrderStatus(int orderId, string status)
            => await _orderRepository.UpdateOrderStatus(orderId, status);


        public async Task<IEnumerable<Orders>> GetOrdersForCurrentUserAsync()
        {
            // Get current user from UserService
            var currentUser = await _userService.GetCurrentUserAsync();

            // If user is not authenticated, return empty list
            if (currentUser == null)
            {
                return new List<Orders>();
            }

            // Use the user ID from the current user
            return await _orderRepository.GetOrdersByUserIdAsync(currentUser.UserId);
        }
    }
}
