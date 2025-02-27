using System;
using System.Collections.Generic;
using System.Data;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Dapper;
using InventoryManagement.Common;
using InventoryManagement.Models.Entities;
using InventoryManagement.Models.ViewModel;
using InventoryManagement.Repo.Data;
using InventoryManagement.Repo.Interfaces;
using Microsoft.Data.SqlClient;
using static Microsoft.EntityFrameworkCore.DbLoggerCategory.Database;

namespace InventoryManagement.Repo.Repository
{
    public class OrderRepository : IOrderRepository
    {
        private readonly DapperDbContext _dbContext;
        private readonly ICheckoutRepository _checkoutRepository;
        private readonly IInventoryRepository _inventoryRepository;
        private readonly IProductRepository _productRepository;

        public OrderRepository(DapperDbContext dbContext, ICheckoutRepository checkoutRepository, IInventoryRepository inventoryRepository, IProductRepository productRepository)
        {
            _dbContext = dbContext;
            _checkoutRepository = checkoutRepository;
            _inventoryRepository = inventoryRepository;
            _productRepository = productRepository;
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

        public async Task<int> CreateOrder(Orders order, IDbTransaction transaction)
        {
            using var connection = _dbContext.CreateConnection();
            var sql = @"INSERT INTO Orders (UserId, OrderDate, TotalAmount, OrderStatus)
                        VALUES (@UserId, GETDATE(), @TotalAmount, @OrderStatus);
                        SELECT CAST(SCOPE_IDENTITY() as int)";
            return await connection.ExecuteScalarAsync<int>(sql, order);
        }

        public async Task<bool> UpdateOrderStatus(int orderId, string status)
        {
            using var connection = _dbContext.CreateConnection();
            var sql = "UPDATE Orders SET Status = @Status WHERE Id = @Id";
            return await connection.ExecuteAsync(sql, new { Id = orderId, Status = status }) > 0;
        }


        public async Task<IEnumerable<OrderView>> GetOrdersByUserIdAsync(int userId)
        {
            const string sql = @"
                SELECT o.OrderId, o.UserId, o.OrderDate, o.TotalAmount, o.OrderStatus,
                       u.Username, u.Email
                FROM Orders o
                INNER JOIN Users u ON o.UserId = u.UserId
                WHERE o.UserId = @UserId
                ORDER BY o.OrderDate DESC";

            using var connection = _dbContext.CreateConnection();
            return await connection.QueryAsync<OrderView>(sql, new { UserId = userId });
        }

        public async Task<List<CheckoutItem>> ConfirmOrderAsync(int userid)
        {
            using var connection = _dbContext.CreateConnection();
            connection.Open();
            using var transaction = connection.BeginTransaction();
            try
            {
                var checkoutItems = await _checkoutRepository.GetCheckoutItemsAsync(userid, transaction);
                if (!checkoutItems.Any())
                {
                    transaction.Rollback();
                    return new List<CheckoutItem>(0);
                }

                var order = new Orders()
                {
                    UserId = userid,
                    TotalAmount = checkoutItems.Sum(chk => chk.Quantity * chk.Price),
                    OrderStatus = CommonStrings.OrderStatusCreated,
                };

                var orderId = await CreateOrder(order, transaction);

                await CreateOrderItems(checkoutItems, orderId, transaction);

                transaction.Commit();
                return (checkoutItems);
            }
            catch (Exception ex)
            {
                transaction.Rollback();
                return (new List<CheckoutItem>(0));
            }
        }

        public async Task CreateOrderItems(List<CheckoutItem> checkoutItems, int orderId, IDbTransaction transaction)
        {
            foreach (var item in checkoutItems)
            {
                var query = "INSERT INTO OrderItems (OrderId, ProductId, Quantity, UnitPrice,OrderItemStatus) VALUES (@OrderId, @ProductId, @Quantity, @UnitPrice,@OrderItemStatus)";
                await transaction.Connection.ExecuteAsync(query, new
                {
                    OrderId = orderId,
                    ProductId = item.ProductId,
                    Quantity = item.Quantity,
                    UnitPrice = item.Price,
                    TotalPrice = item.Quantity * item.Price,
                    OrderItemStatus = CommonStrings.OrderItemStatusCreated
                }, transaction);
            }
        }

        public async Task<IEnumerable<OrderItemViewModel>> GetOrderItemsByOrderId(int orderId)
        {
            using var connection = _dbContext.CreateConnection();
            var query = @"SELECT oi.Id AS OrderItemId, oi.OrderId, oi.ProductId, oi.Quantity, 
                           oi.UnitPrice, (oi.Quantity * oi.UnitPrice) AS TotalPrice, 
                           COALESCE(oi.OrderItemStatus, '') AS OrderStatus,
                           p.ProductName, pd.ProductDescription, pd.ProductPhoto, 
                           COALESCE(s.SupplierName, '') AS SellerName, 
	                       COALESCE(s.ContactEmail, '') AS SellerEmail
                        FROM OrderItems oi
                        LEFT JOIN Products p ON oi.ProductId = p.ProductId
                        LEFT JOIN ProductDetails pd ON p.ProductId = pd.ProductId
                        LEFT JOIN Suppliers s ON p.SupplierId =s.SupplierId
                        WHERE oi.OrderId = @OrderId";
            return await connection.QueryAsync<OrderItemViewModel>(query, new { OrderId = orderId });
        }

        public async Task<bool> CancelOrderItemAsync(int orderItemId)
        {
            using var connection = _dbContext.CreateConnection();
            var query = @"UPDATE OrderItems SET OrderItemStatus = '" + CommonStrings.OrderItemStatusCancelled + "' WHERE Id = @OrderItemId";
            var rowsAffected = await connection.ExecuteAsync(query, new { OrderItemId = orderItemId });
            return rowsAffected > 0;
        }

        public async Task<IEnumerable<OrderItemSellerViewModel>> GetOrdersForSellerAsync(int sellerId)
        {
            using var connection = _dbContext.CreateConnection();
            var query = @"SELECT  o.OrderId, o.UserId, o.OrderDate,
                            oi.Id AS OrderItemId, oi.ProductId, oi.Quantity, oi.UnitPrice, oi.OrderItemStatus,
                            p.ProductName, pd.ProductDescription, pd.ProductPhoto,
                            u.Username AS BuyerName, u.Email AS BuyerEmail,
			                u.UserAddress, u.City
                        FROM OrderItems oi
                        JOIN Orders o ON oi.OrderId = o.OrderId
                        JOIN Products p ON oi.ProductId = p.ProductId
                        JOIN Suppliers s ON p.SellerId = s.UserId
                        LEFT JOIN ProductDetails pd ON p.ProductId = pd.ProductId
                        JOIN Users u ON o.UserId = u.UserId
                        WHERE s.UserId = @SellerId
                        ORDER BY o.OrderDate DESC";

            return await connection.QueryAsync<OrderItemSellerViewModel>(query, new { SellerId = sellerId });
        }

        public async Task<bool> UpdateOrderItemStatusAsync(int orderItemId, string newStatus)
        {
            using var connection = _dbContext.CreateConnection();
            var query = "UPDATE OrderItems SET OrderItemStatus = @NewStatus WHERE Id = @OrderItemId";

            var result = await connection.ExecuteAsync(query, new { OrderItemId = orderItemId, NewStatus = newStatus });
            return result > 0;
        }
    }
}
