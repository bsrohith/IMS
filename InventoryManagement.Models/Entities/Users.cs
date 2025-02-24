using InventoryManagement.Common;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace InventoryManagement.Models.Entities
{
    public class Users
    {
        public int UserId { get; set; }
        public string Username { get; set; }
        public string Email { get; set; }
        public string Password { get; set; }
        public string UserAddress { get; set; } = string.Empty;
        public string City { get; set; } = string.Empty;
        public string UserRole { get; set; } = CommonStrings.User;
        public DateTime CreatedAt { get; set; }
    }
}
