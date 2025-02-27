using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace InventoryManagement.Models.Entities
{
    public class UserRegister
    {
        public string? Username { get; set; }
        public string? Email { get; set; }
        public string? UserAddress { get; set; }
        public string? UserRole { get; set; }
        public string? City { get; set; }
        public string? Password { get; set; }
        public string? ConfirmPassword { get; set; }
        public string? SupplierName { get; set; } = string.Empty;
        public string? PhoneNumber { get; set; } = string.Empty;
    }
}
