using Dapper;
using InventoryManagement.Models.Entities;
using InventoryManagement.Repo.Data;
using InventoryManagement.Repo.Interfaces;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace InventoryManagement.Repo.Repository
{
    public class SupplierRepository : ISupplierRepository
    {
        private readonly DapperDbContext _dbContext;
        public SupplierRepository(DapperDbContext dbContext)
        {
            _dbContext = dbContext;
        }

        public async Task<IEnumerable<Suppliers>> GetAllSuppliers()
        {
            using var connection = _dbContext.CreateConnection();
            return await connection.QueryAsync<Suppliers>("SELECT * FROM Suppliers");
        }

        public async Task<Suppliers> GetSuppliersById(int id)
        {
            using var connection = _dbContext.CreateConnection();
            return await connection.QueryFirstOrDefaultAsync<Suppliers>(
                "SELECT * FROM Suppliers WHERE SupplierId = @SupplierId", new { SupplierId = id }) ?? new Suppliers();
        }

        public async Task<Suppliers> GetSupplierByUserIdAsync(int userId)
        {
            using var connection = _dbContext.CreateConnection();
            var query = "SELECT * FROM Suppliers WHERE UserId = @UserId";
            return await connection.QueryFirstOrDefaultAsync<Suppliers>(query, new { UserId = userId });
        }
    }


}
