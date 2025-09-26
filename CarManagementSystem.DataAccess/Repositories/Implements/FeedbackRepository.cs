using CarManagementSystem.BusinessObjects;
using CarManagementSystem.DataAccess.Repositories.Interfaces;
using Microsoft.EntityFrameworkCore;
using System.Linq;
using System.Linq.Expressions;


namespace CarManagementSystem.DataAccess.Repositories.Services
{
    public class FeedbackRepository : IFeedbackRepository
    {
        private readonly CarManagementDbContext _db;

        public FeedbackRepository(CarManagementDbContext db)
        {
            _db = db;
        }

        public async Task<Feedback> GetByIdAsync(int id) =>
            await _db.Feedbacks.FirstOrDefaultAsync(x => x.Id == id);

        public async Task<List<Feedback>> GetAllAsync(Expression<Func<Feedback, bool>>? predicate = null)
        {
            IQueryable<Feedback> q = _db.Feedbacks.AsNoTracking();
            if (predicate != null) q = q.Where(predicate);
            return await q.OrderByDescending(x => x.Datetime).ToListAsync();
        }

        public async Task<Feedback> CreateAsync(Feedback feedback)
        {
            _db.Feedbacks.Add(feedback);
            await _db.SaveChangesAsync();
            return feedback;
        }

        public async Task<Feedback> UpdateAsync(Feedback feedback)
        {
            _db.Feedbacks.Update(feedback);
            await _db.SaveChangesAsync();
            return feedback;
        }

        public async Task<bool> DeleteAsync(int id)
        {
            var f = await _db.Feedbacks.FindAsync(id);
            if (f == null) return false;

            _db.Feedbacks.Remove(f);
            await _db.SaveChangesAsync();
            return true;
        }

        public Task<int> SaveChangesAsync() => _db.SaveChangesAsync();
    }
}
