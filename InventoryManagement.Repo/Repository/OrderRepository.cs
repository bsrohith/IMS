using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Dapper;
using InventoryManagement.Models.Entities;
using InventoryManagement.Repo.Data;
using InventoryManagement.Repo.Interfaces;
using Microsoft.Data.SqlClient;

namespace InventoryManagement.Repo.Repository
{
    public class OrderRepository : IOrderRepository
    {
        private readonly DapperDbContext _dbContext;

        public OrderRepository(DapperDbContext dbContext)
        {
            _dbContext = dbContext;
        }

        public async Task<IEnumerable<Orders>> GetAllOrders()
        {
            using var connection = _dbContext.CreateConnection();
            return await connection.QueryAsync<Orders>("SELECT * FROM Orders");
        }

        public async Task<Orders> GetOrderById(int id)
        {
            using var connection = _dbContext.CreateConnection();
            return await connection.QueryFirstOrDefaultAsync<Orders>(
                "SELECT * FROM Orders WHERE OrderId = @OrderId", new { Id = id });
        }

        public async Task<int> CreateOrder(Orders order)
        {
            using var connection = _dbContext.CreateConnection();
            var sql = @"INSERT INTO Orders (UserId, OrderDate, TotalAmount, Status)
                        VALUES (@UserId, GETDATE(), @TotalAmount, 'Pending');
                        SELECT CAST(SCOPE_IDENTITY() as int)";
            return await connection.ExecuteScalarAsync<int>(sql, order);
        }

        public async Task<bool> UpdateOrderStatus(int orderId, string status)
        {
            using var connection = _dbContext.CreateConnection();
            var sql = "UPDATE Orders SET Status = @Status WHERE Id = @Id";
            return await connection.ExecuteAsync(sql, new { Id = orderId, Status = status }) > 0;
        }


        public async Task<IEnumerable<Orders>> GetOrdersByUserIdAsync(int userId)
        {
            const string sql = @"
                SELECT o.OrderId, o.UserId, o.OrderDate, o.TotalAmount, o.OrderStatus,
                       u.Username, u.Email
                FROM Orders o
                INNER JOIN Users u ON o.UserId = u.UserId
                WHERE o.UserId = @UserId
                ORDER BY o.OrderDate DESC";

            using var connection = _dbContext.CreateConnection(); 
            return await connection.QueryAsync<Orders>(sql, new { UserId = userId });
        }
    }
}
