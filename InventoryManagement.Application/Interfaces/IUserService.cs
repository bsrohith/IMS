using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using InventoryManagement.Models.Entities;
using InventoryManagement.Models.ViewModel;

namespace InventoryManagement.Application.Interfaces
{
    public interface IUserService
    {
        Task<(bool Success, string Message)> RegisterUserAsync(UserRegister register);
        Task<List<Users>> GetAllUsers();
        Task<Users> GetUserById(int id);

        Task<Users?> Verify(string email, string password);

        Task<Users> GetCurrentUserAsync();
        Task<Suppliers> GetSupplierDataAsync(int userId);
        Task<(bool Success, string Message)> UpdateUserProfileAsync(int userId, UserProfileEdit model);
    }
}
