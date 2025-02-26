using System.Security.Cryptography;
using System.Text;
using InventoryManagement.Application.Interfaces;
using InventoryManagement.Models.Entities;
using InventoryManagement.Repo.Interfaces;


namespace InventoryManagement.Application.Services
{
    public class UserService : IUserService
    {
        private readonly IUserRepository _userRepository;

        public UserService(IUserRepository userRepository)
        {
            _userRepository = userRepository;
        }

        public async Task<(bool Success, string Message)> RegisterUserAsync(UserRegister model)
        {
            // Validation
            if (string.IsNullOrWhiteSpace(model.Username) || string.IsNullOrWhiteSpace(model.Email) ||
                string.IsNullOrWhiteSpace(model.Password))
            {
                return (false, "Username, email, and password are required.");
            }

            if (model.Password != model.ConfirmPassword)
            {
                return (false, "Passwords do not match.");
            }

            if (model.Password.Length < 8)
            {
                return (false, "Password must be at least 8 characters long.");
            }

            // Check for existing username
            if (await _userRepository.UsernameExistsAsync(model.Username))
            {
                return (false, "Username is already taken.");
            }

            // Check for existing email
            if (await _userRepository.EmailExistsAsync(model.Email))
            {
                return (false, "Email is already registered.");
            }

            // Create user object
            var user = new Users
            {
                Username = model.Username,
                Email = model.Email,
                UserAddress = model.UserAddress,
                City = model.City,
                PasswordHash = HashPassword(model.Password),
                UserRole = "User", // Default role
                CreatedAt = DateTime.Now
            };

            // Save user to database
            await _userRepository.CreateUserAsync(user);

            return (true, "Registration successful. You can now log in.");
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

        public async Task<List<Users>> GetAllUsers()
        {
            return (List<Users>)await _userRepository.GetAllUsers();
        }

        public async Task<Users> GetUserById(int id)
        {
            return await _userRepository.GetUserById(id);
        }

    }
}
