using System;
using System.Collections;
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
   public class UserRepository : IUserRepository
    {
        private readonly DapperDbContext _dbContext;

        public UserRepository(DapperDbContext dbContext)
        {
            _dbContext = dbContext;
        }

        public async Task<bool> UsernameExistsAsync(string username)
        {
            using var connection = _dbContext.CreateConnection();
            return await connection.QueryFirstOrDefaultAsync<bool>("SELECT 1 FROM Users WHERE Username = @Username", new { Username = username });
        }

        public async Task<bool> EmailExistsAsync(string email)
        {
            using var connection = _dbContext.CreateConnection();
            return await connection.QueryFirstOrDefaultAsync<bool>("SELECT 1 FROM Users WHERE Email = @Email", new { Email = email });
        }

        //public async Task CreateUserAsync(Users user)
        //{
        //    using var connection = _dbContext.CreateConnection();
        //    var query = "INSERT INTO Users (Username, Email, PasswordHash, UserRole, CreatedAt, UserAddress, City) VALUES (@Username, @Email, @PasswordHash, @UserRole, @CreatedAt, @UserAddress, @City);";

        //    await connection.ExecuteAsync(query, user);
        //}

        public async Task CreateUserAsync(Users user, Suppliers supplier = null)
        {
            using var connection = _dbContext.CreateConnection();
            connection.Open();

            using var transaction = connection.BeginTransaction();

            try
            {
                // Insert into Users table and get the new UserId
                var insertUserQuery = @"
            INSERT INTO Users (Username, Email, PasswordHash, UserRole, CreatedAt, UserAddress, City) 
            VALUES (@Username, @Email, @PasswordHash, @UserRole, @CreatedAt, @UserAddress, @City);
            SELECT CAST(SCOPE_IDENTITY() as int);";

                var userId = await connection.ExecuteScalarAsync<int>(insertUserQuery, user, transaction);

                // If this is a supplier, insert the supplier data too
                if (supplier != null && user.UserRole == "Supplier")
                {
                    supplier.UserId = userId;

                    var insertSupplierQuery = @"
                INSERT INTO Suppliers (UserId, SupplierName, ContactEmail, PhoneNumber)
                VALUES (@UserId, @SupplierName, @ContactEmail, @PhoneNumber);";

                    await connection.ExecuteAsync(insertSupplierQuery, supplier, transaction);
                }

                transaction.Commit();
            }
            catch (Exception)
            {
                transaction.Rollback();
                throw;
            }
        }

        public async Task<IEnumerable<Users>> GetAllUsers()
        {
            using var connection = _dbContext.CreateConnection();
            return await connection.QueryAsync<Users>("SELECT * FROM USERS");
        }

        public async Task<Users> GetUserById(int id)
        {
            using var connection = _dbContext.CreateConnection();
            return await connection.QueryFirstOrDefaultAsync<Users>("SELECT * FROM USERS WHERE UserId = @UserId", new { UserId = id });
        }

        public async Task<Users> GetUserByUsernameAsync(string username)
        {
            using var connection = _dbContext.CreateConnection();
            return await connection.QueryFirstOrDefaultAsync<Users>("SELECT * FROM Users WHERE Username = @Username", new { Username = username });
        }


        public async Task<Users> GetUserByEmailAndPasswordAsync(string email, string passwordHash)
        {
            using var connection = _dbContext.CreateConnection();
            return await connection.QueryFirstOrDefaultAsync<Users>("SELECT * FROM Users WHERE Email = @Email AND PasswordHash = @PasswordHash",
                new { Email = email, PasswordHash = passwordHash });
        }

    }
}
