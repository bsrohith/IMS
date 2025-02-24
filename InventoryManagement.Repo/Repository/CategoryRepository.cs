using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Dapper;
using InventoryManagement.Models.Entities;
using InventoryManagement.Repo.Data;
using InventoryManagement.Repo.Interfaces;

namespace InventoryManagement.Repo.Repository
{
    public class CategoryRepository : ICategoryRepository
    {
        private readonly DapperDbContext _dbContext;

        public CategoryRepository(DapperDbContext dbContext)
        {
            _dbContext = dbContext;
        }

        public async Task<IEnumerable<Categories>> GetAllCategories()
        {
            using var connection = _dbContext.CreateConnection();
            return await connection.QueryAsync<Categories>("SELECT * FROM Categories");
        }
    }
}
