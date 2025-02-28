using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace InventoryManagement.Models.ViewModel
{
    // Model for profile editing
    public class UserProfileEdit
    {
        [Required(ErrorMessage = "Username is required")]
        public string Username { get; set; }

        [Required(ErrorMessage = "Email is required")]
        [EmailAddress(ErrorMessage = "Invalid email format")]
        public string Email { get; set; }

        public string UserAddress { get; set; }

        public string City { get; set; }

        public string UserRole { get; set; }

        public string SupplierName { get; set; }

        public string PhoneNumber { get; set; }

        public string NewPassword { get; set; }

        public string ConfirmNewPassword { get; set; }

        [Required(ErrorMessage = "Current password is required to save changes")]
        public string CurrentPassword { get; set; }
    }
}

