using CarManagementSystem.DataAccess;
using CarManagementSystem.DataAccess.Repositories.Interfaces;
using CarManagementSystem.Services.Interfaces;


namespace CarManagementSystem.Services.Services
{
    public class PromotionService : IPromotionService
    {
        private readonly IPromotionRepository _repo;

        public PromotionService(IPromotionRepository repo)
        {
            _repo = repo;
        }

        public async Task<List<Promotion>> GetAllAsync(bool onlyActive = true)
        {
            if (onlyActive)
            {
                return await _repo.GetAllAsync(p => p.Status == "Active");
            }
            return await _repo.GetAllAsync();
        }

        public Task<Promotion?> GetByIdAsync(int id)
        {
            return _repo.GetByIdAsync(id);
        }

        public async Task<(bool ok, string message, Promotion? data)> CreateAsync(Promotion promotion)
        {
            // Code và Status mặc định
            promotion.Code = promotion.Code?.Trim();
            if (string.IsNullOrWhiteSpace(promotion.Status))
            {
                promotion.Status = "Active";
            }

            var saved = await _repo.CreateAsync(promotion);
            return (true, "Created", saved);
        }

        public async Task<(bool ok, string message, Promotion? data)> UpdateAsync(Promotion promotion)
        {
            var existed = await _repo.GetByIdAsync(promotion.Id);
            if (existed == null) return (false, "Not found", null);

            existed.Code = promotion.Code?.Trim();
            existed.Description = promotion.Description;
            existed.Discount = promotion.Discount;
            existed.Status = promotion.Status;

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
