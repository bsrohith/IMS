using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using InventoryManagement.Application.Interfaces;
using InventoryManagement.Models.Entities;
using InventoryManagement.Repo.Interfaces;

namespace InventoryManagement.Application.Services
{
    public class OrderService : IOrderService
    {
        private readonly IOrderRepository _orderRepository;

        public OrderService(IOrderRepository orderRepository)
        {
            _orderRepository = orderRepository;
        }

        public async Task<IEnumerable<Orders>> GetAllOrders() => await _orderRepository.GetAllOrders();

        public async Task<Orders> GetOrderById(int id) => await _orderRepository.GetOrderById(id);

        public async Task<int> CreateOrder(Orders order) => await _orderRepository.CreateOrder(order);

        public async Task<bool> UpdateOrderStatus(int orderId, string status)
            => await _orderRepository.UpdateOrderStatus(orderId, status);
    }
}
