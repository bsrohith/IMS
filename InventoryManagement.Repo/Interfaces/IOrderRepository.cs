using System;
using System.Collections.Generic;
using System.Data;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using InventoryManagement.Models.Entities;
using InventoryManagement.Models.ViewModel;

namespace InventoryManagement.Repo.Interfaces
{
    public interface IOrderRepository
    {
        Task<IEnumerable<Orders>> GetAllOrders();
        Task<Orders> GetOrderById(int id);
        Task<int> CreateOrder(Orders order, IDbTransaction transaction);
        Task<bool> UpdateOrderStatus(int orderId, string status);

        Task<IEnumerable<OrderView>> GetOrdersByUserIdAsync(int userId);
        Task CreateOrderItems(List<CheckoutItem> checkoutItems, int orderId, IDbTransaction transaction);
        Task<List<CheckoutItem>> ConfirmOrderAsync(int userid);
    }
   
}
