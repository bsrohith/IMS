using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using System.Security.Cryptography;
using System.Text;
using System.Threading.Tasks;
using Dapper;
using InventoryManagement.Models.Entities;
using InventoryManagement.Models.ViewModel;
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


        public async Task<(bool Success, string Message)> UpdateUserProfileAsync(int userId, UserProfileEdit model)
        {
            // Validation
            if (string.IsNullOrWhiteSpace(model.Username) || string.IsNullOrWhiteSpace(model.Email))
            {
                return (false, "Username and email are required.");
            }

            // Get current user for password verification
            var currentUser = await GetUserByIdAsync(userId);
            if (currentUser == null)
            {
                return (false, "User not found.");
            }

            // Verify current password
            if (!VerifyPassword(model.CurrentPassword, currentUser.PasswordHash))
            {
                return (false, "Current password is incorrect.");
            }

            // Check if username is taken by another user
            if (model.Username != currentUser.Username)
            {
                var existingUser = await GetUserByUsernameAsync(model.Username);
                if (existingUser != null && existingUser.UserId != userId)
                {
                    return (false, "Username is already taken.");
                }
            }

            // Check if email is taken by another user
            if (model.Email != currentUser.Email)
            {
                var existingUser = await GetUserByEmailAsync(model.Email);
                if (existingUser != null && existingUser.UserId != userId)
                {
                    return (false, "Email is already registered.");
                }
            }

            using var connection = _dbContext.CreateConnection();
            connection.Open();
            using var transaction = connection.BeginTransaction();

            try
            {
                // Prepare the SQL query and parameters
                string updateUserQuery;
                object parameters;

                // If password is being changed, include it in the update
                if (!string.IsNullOrEmpty(model.NewPassword))
                {
                    updateUserQuery = @"
                UPDATE Users 
                SET Username = @Username, 
                    Email = @Email, 
                    UserAddress = @UserAddress, 
                    City = @City,
                    PasswordHash = @PasswordHash
                WHERE UserId = @UserId";

                    parameters = new
                    {
                        UserId = userId,
                        model.Username,
                        model.Email,
                        model.UserAddress,
                        model.City,
                        PasswordHash = HashPassword(model.NewPassword)
                    };
                }
                else
                {
                    updateUserQuery = @"
                UPDATE Users 
                SET Username = @Username, 
                    Email = @Email, 
                    UserAddress = @UserAddress, 
                    City = @City
                WHERE UserId = @UserId";

                    parameters = new
                    {
                        UserId = userId,
                        model.Username,
                        model.Email,
                        model.UserAddress,
                        model.City
                    };
                }

                await connection.ExecuteAsync(updateUserQuery, parameters, transaction);

                // If user is a supplier, update supplier data
                if (currentUser.UserRole == "Supplier")
                {
                    var supplier = await GetSupplierDataAsync(userId);

                    if (supplier != null)
                    {
                        // Update existing supplier record
                        var updateSupplierQuery = @"
                    UPDATE Suppliers 
                    SET SupplierName = @SupplierName, 
                        ContactEmail = @ContactEmail, 
                        PhoneNumber = @PhoneNumber 
                    WHERE UserId = @UserId";

                        await connection.ExecuteAsync(updateSupplierQuery, new
                        {
                            UserId = userId,
                            SupplierName = model.SupplierName,
                            ContactEmail = model.Email,
                            PhoneNumber = model.PhoneNumber
                        }, transaction);
                    }
                    else
                    {
                        // Create new supplier record if it doesn't exist
                        var insertSupplierQuery = @"
                    INSERT INTO Suppliers (UserId, SupplierName, ContactEmail, PhoneNumber)
                    VALUES (@UserId, @SupplierName, @ContactEmail, @PhoneNumber)";

                        await connection.ExecuteAsync(insertSupplierQuery, new
                        {
                            UserId = userId,
                            SupplierName = model.SupplierName,
                            ContactEmail = model.Email,
                            PhoneNumber = model.PhoneNumber
                        }, transaction);
                    }
                }

                transaction.Commit();
                return (true, "Profile updated successfully.");
            }
            catch (Exception ex)
            {
                transaction.Rollback();
                Console.WriteLine($"Error updating profile: {ex.Message}");
                return (false, "An error occurred while updating your profile.");
            }
        }

        private string HashPassword(string password)
        {
            using (SHA256 sha256 = SHA256.Create())
            {
                byte[] bytes = sha256.ComputeHash(Encoding.UTF8.GetBytes(password));

                StringBuilder builder = new StringBuilder();
                for (int i = 0; i < bytes.Length; i++)
                {
                    builder.Append(bytes[i].ToString("x2"));
                }

                return builder.ToString();
            }
        }

        private async Task<Users> GetUserByIdAsync(int userId)
        {
            using var connection = _dbContext.CreateConnection();
            var query = "SELECT * FROM Users WHERE UserId = @UserId";
            return await connection.QueryFirstOrDefaultAsync<Users>(query, new { UserId = userId });
        }

   

        private async Task<Users> GetUserByEmailAsync(string email)
        {
            using var connection = _dbContext.CreateConnection();
            var query = "SELECT * FROM Users WHERE Email = @Email";
            return await connection.QueryFirstOrDefaultAsync<Users>(query, new { Email = email });
        }

        private bool VerifyPassword(string password, string passwordHash)
        {
            // Implement your password verification logic here
            // For example, using BCrypt:
            // return BCrypt.Net.BCrypt.Verify(password, passwordHash);

            // Or if you're using a simple hash like SHA256:
            // return HashPassword(password) == passwordHash;

            // This is a placeholder - implement actual verification based on how you hash passwords
            return HashPassword(password) == passwordHash;
        }

        public async Task<Suppliers> GetSupplierDataAsync(int userId)
        {
            using var connection = _dbContext.CreateConnection();
            var query = "SELECT * FROM Suppliers WHERE UserId = @UserId";
            var supplier = await connection.QueryFirstOrDefaultAsync<Suppliers>(query, new { UserId = userId });
            return supplier;
        }

    }
}
