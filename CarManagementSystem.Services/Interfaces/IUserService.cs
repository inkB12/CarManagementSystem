using CarManagementSystem.BusinessObjects;
using CarManagementSystem.DataAccess;

namespace CarManagementSystem.Services.Interfaces
{
    public interface IUserService
    {
        Task<List<User>> GetAllAsync(bool onlyActive = true);
        Task<User?> GetByIdAsync(int id);

        Task<(bool ok, string message, User? data)> RegisterAsync(string email, string password, string fullName, string? phone, string role = "Customer");
        Task<(bool ok, string message, User? data)> LoginAsync(string email, string password);

        Task<(bool ok, string message, User? data)> CreateAsync(User user);
        Task<(bool ok, string message, User? data)> UpdateAsync(User user);
        Task<(bool ok, string message)> DeleteAsync(int id);
    }
}
