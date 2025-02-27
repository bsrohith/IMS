using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using InventoryManagement.Models.Entities;

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
    }
}
