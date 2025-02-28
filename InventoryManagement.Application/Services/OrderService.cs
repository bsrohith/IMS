﻿using System;
using System.Collections.Generic;
using System.Data;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using InventoryManagement.Application.Interfaces;
using InventoryManagement.Models.Entities;
using InventoryManagement.Models.ViewModel;
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

        public async Task<int> CreateOrder(Orders order, IDbTransaction transaction) => await _orderRepository.CreateOrder(order, transaction);

        public async Task<bool> UpdateOrderStatus(int orderId, string status)
            => await _orderRepository.UpdateOrderStatus(orderId, status);


        public async Task<IEnumerable<OrderView>> GetOrdersForCurrentUserAsync()
        {
            // Get current user from UserService
            var currentUser = await _userService.GetCurrentUserAsync();

            // If user is not authenticated, return empty list
            if (currentUser == null)
            {
                return new List<OrderView>();
            }

            // Use the user ID from the current user
            return await _orderRepository.GetOrdersByUserIdAsync(currentUser.UserId);
        }

        public async Task<List<CheckoutItem>> ConfirmOrderAsync(int userid)
        {
            return await _orderRepository.ConfirmOrderAsync(userid);
        }

        public async Task<List<OrderItemViewModel>> GetOrderItemsByOrderId(int orderId)
        {
            return (await _orderRepository.GetOrderItemsByOrderId(orderId)).ToList<OrderItemViewModel>();
        }

        public async Task<bool> CancelOrderItemAsync(int orderItemId)
        {
            return await _orderRepository.CancelOrderItemAsync(orderItemId);
        }

        public async Task<List<OrderItemSellerViewModel>> GetOrdersForSellerAsync(int sellerId)
        {
            return (await _orderRepository.GetOrdersForSellerAsync(sellerId)).ToList<OrderItemSellerViewModel>();
        }

        public async Task<bool> UpdateOrderItemStatusAsync(int orderItemId, string newStatus)
        {
            return await _orderRepository.UpdateOrderItemStatusAsync(orderItemId, newStatus);
        }
    }
}
