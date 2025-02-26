using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using InventoryManagement.Models.Entities;
using InventoryManagement.Models.ViewModel;

namespace InventoryManagement.Application.Interfaces
{
    public interface IOrderService
    {
        Task<IEnumerable<Orders>> GetAllOrders();
        Task<Orders> GetOrderById(int id);
        Task<int> CreateOrder(Orders order);
        Task<bool> UpdateOrderStatus(int orderId, string status);

        Task<IEnumerable<OrderView>> GetOrdersForCurrentUserAsync();
    }
}
