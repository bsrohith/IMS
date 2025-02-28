using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using InventoryManagement.Models.Entities;
using InventoryManagement.Models.ViewModel;

namespace InventoryManagement.Repo.Interfaces
{
    public interface IUserRepository
    {
        Task<bool> UsernameExistsAsync(string username);
        Task<bool> EmailExistsAsync(string email);
        Task CreateUserAsync(Users user, Suppliers supplier = null);
        Task<IEnumerable<Users>> GetAllUsers();
        Task<Users> GetUserById(int id);
        Task<Users> GetUserByUsernameAsync(string username);

        Task<Users>  GetUserByEmailAndPasswordAsync(string email, string passwordHash);
        Task<Suppliers> GetSupplierDataAsync(int userId);
        Task<(bool Success, string Message)> UpdateUserProfileAsync(int userId, UserProfileEdit model);
    }
}
