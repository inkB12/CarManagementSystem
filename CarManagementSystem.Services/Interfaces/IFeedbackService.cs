

using CarManagementSystem.DataAccess;

namespace CarManagementSystem.Services.Interfaces
{
    public interface IFeedbackService
    {
        Task<List<Feedback>> GetAllAsync(int? userId = null);
        Task<Feedback?> GetByIdAsync(int id);

        Task<(bool ok, string message, Feedback? data)> CreateAsync(Feedback feedback);
        Task<(bool ok, string message, Feedback? data)> UpdateAsync(Feedback feedback);
        Task<(bool ok, string message)> DeleteAsync(int id);
    }
}
