using CarManagementSystem.DataAccess;
using CarManagementSystem.DataAccess.Repositories.Interfaces;
using CarManagementSystem.Services.Interfaces;
using System.Security.Cryptography;
using System.Text;

namespace CarManagementSystem.Services.Services
{
    public class UserService : IUserService
    {
        private readonly IUserRepository _repo;

        public UserService(IUserRepository repo)
        {
            _repo = repo;
        }

        //Get all truyền vào onlyActive = true để chỉ lấy user đang active, nếu ko thì lấy hết
        public async Task<List<User>> GetAllAsync(bool onlyActive = true)
        {
            if (onlyActive)
            {
                return await _repo.GetAllAsync(u => u.IsActive == true);
            }    
            return await _repo.GetAllAsync();
        }

        public Task<User?> GetByIdAsync(int id)
        {
            return _repo.GetByIdAsync(id);
        }
        public async Task<(bool ok, string message, User? data)> RegisterAsync(string email, string password, string fullName, string? phone, string role = "Customer")
        {
            var existed = await _repo.GetByEmailAsync(email);
            if (existed != null) return (false, "Email already exists", null);

            var user = new User
            {
                Email = email.Trim(),
                FullName = fullName?.Trim() ?? "",
                Phone = phone,
                Password = HashSHA256(password), 
                Role = role,
                IsActive = true
            };

            user = await _repo.CreateAsync(user);
            return (true, "Register success", user);
        }

        //Login 
        public async Task<(bool ok, string message, User? data)> LoginAsync(string email, string password)
        {
            var user = await _repo.GetByEmailAsync(email);
            if (user == null) return (false, "Email not found", null);
            if (user.IsActive == false) return (false, "User is disabled", null);

            if (HashSHA256(password) != user.Password)
                return (false, "Wrong Email or Password", null);

            return (true, "Login success", user);
        }

        //Create Account
        public async Task<(bool ok, string message, User? data)> CreateAsync(User user)
        {
            user.Password = HashSHA256(user.Password);

            user.IsActive = true;
            var saved = await _repo.CreateAsync(user);
            return (true, "Created", saved);
        }

        public async Task<(bool ok, string message, User? data)> UpdateAsync(User user)
        {
            //chưa xử lý logic
            var updated = await _repo.UpdateAsync(user);
            return (true, "Updated", updated);
        }

        public async Task<(bool ok, string message)> DeleteAsync(int id)
        {
            var ok = await _repo.DeleteAsync(id);
            return ok ? (true, "Deleted") : (false, "Not found");
        }

        public static string HashSHA256(string input)
        {
            using var sha = SHA256.Create();
            var bytes = Encoding.UTF8.GetBytes(input);
            return Convert.ToBase64String(sha.ComputeHash(bytes));
        }
    }
}
