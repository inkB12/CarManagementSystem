using CarManagementSystem.DataAccess;
using CarManagementSystem.DataAccess.Repositories.Interfaces;
using CarManagementSystem.Services.Interfaces;

namespace CarManagementSystem.Services.Services
{
    public class FeedbackService : IFeedbackService
    {
        private readonly IFeedbackRepository _repo;

        public FeedbackService(IFeedbackRepository repo)
        {
            _repo = repo;
        }

        public async Task<List<Feedback>> GetAllAsync(int? userId = null)
        {
            if (userId.HasValue)
            {
                return await _repo.GetAllAsync(f => f.UserId == userId.Value);
            }
            return await _repo.GetAllAsync();
        }

        public Task<Feedback?> GetByIdAsync(int id)
        {
            return _repo.GetByIdAsync(id);
        }

        public async Task<(bool ok, string message, Feedback? data)> CreateAsync(Feedback feedback)
        {
            feedback.Datetime = DateTime.Now;
            var saved = await _repo.CreateAsync(feedback);
            return (true, "Created", saved);
        }

        public async Task<(bool ok, string message, Feedback? data)> UpdateAsync(Feedback feedback)
        {
            var existed = await _repo.GetByIdAsync(feedback.Id);
            if (existed == null) return (false, "Not found", null);

            existed.FeedbackType = feedback.FeedbackType?.Trim();
            existed.Content = feedback.Content;
            existed.UserId = feedback.UserId;
            // datetime giữ nguyên

            var updated = await _repo.UpdateAsync(existed);
            return (true, "Updated", updated);
        }

        public async Task<(bool ok, string message)> DeleteAsync(int id)
        {
            var ok = await _repo.DeleteAsync(id);
            return ok ? (true, "Deleted") : (false, "Not found");
        }
    }
}
