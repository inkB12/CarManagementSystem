using CarManagementSystem.BusinessObjects;
using CarManagementSystem.Services.Interfaces;
using CarManagementSystem.WebMVC.Models;
using Microsoft.AspNetCore.Mvc;

namespace CarManagementSystem.WebMVC.Controllers
{
    public class FeedbackController : Controller
    {
        private readonly IFeedbackService _feedbackService;
        private readonly IUserService _userService;

        public FeedbackController(IFeedbackService feedbackService, IUserService userService)
        {
            _feedbackService = feedbackService;
            _userService = userService;
        }

        public async Task<IActionResult> Index()
        {
            var list = await _feedbackService.GetAllAsync();
            var vm = list.Select(f => new FeedbackViewModel
            {
                Id = f.Id,
                FeedbackType = f.FeedbackType,
                Content = f.Content,
                Datetime = f.Datetime,
                UserName = f.User?.FullName ?? $"User #{f.UserId}"
            }).ToList();

            return View(vm);
        }

        public IActionResult Create()
        {
            return View(new FeedbackViewModel());
        }

        [HttpPost, ValidateAntiForgeryToken]
        public async Task<IActionResult> Create(FeedbackViewModel vm)
        {
            if (!ModelState.IsValid) return View(vm);

            var entity = new Feedback
            {
                FeedbackType = vm.FeedbackType,
                Content = vm.Content,
                UserId = SessionExtensions.GetInt32(HttpContext.Session, "UserId") ?? 0,
            };

            var result = await _feedbackService.CreateAsync(entity);
            if (result.ok) return RedirectToAction(nameof(Index));

            ModelState.AddModelError("", result.message);
            return View(vm);
        }

        private bool IsStaffOrAdmin()
        {
            var role = HttpContext.Session.GetString("UserRole");
            return string.Equals(role, "Staff", StringComparison.OrdinalIgnoreCase)
                || string.Equals(role, "Admin", StringComparison.OrdinalIgnoreCase);
        }

        public async Task<IActionResult> IndexAdmin(string? q = null)
        {
            if (!IsStaffOrAdmin()) return RedirectToAction("Index", "Home");

            // Lấy tất cả feedback 
            var all = await _feedbackService.GetAllAsync(null);

            //map Tên vs Email + search
            var userIds = all.Select(f => f.UserId).Distinct().ToList();
            var users = await _userService.GetAllAsync(onlyActive: false);
            var userDict = users
                .Where(u => userIds.Contains(u.Id))
                .ToDictionary(u => u.Id, u => new { u.FullName, u.Email });

            // Lọc theo q là tên hoặc email
            if (!string.IsNullOrWhiteSpace(q))
            {
                var norm = q.Trim().ToLowerInvariant();
                all = all.Where(f =>
                {
                    if (!userDict.TryGetValue(f.UserId, out var info)) return false;
                    var name = info.FullName?.ToLowerInvariant() ?? "";
                    var email = info.Email?.ToLowerInvariant() ?? "";
                    return name.Contains(norm) || email.Contains(norm);
                }).ToList();
            }

            var vms = all
                .OrderByDescending(f => f.Datetime)
                .Select(f =>
                {
                    userDict.TryGetValue(f.UserId, out var info);
                    return new AdminFeedbackViewModel
                    {
                        Id = f.Id,
                        UserId = f.UserId,
                        FeedbackType = f.FeedbackType,
                        Content = f.Content,
                        Datetime = f.Datetime,
                        UserFullName = info?.FullName,
                        UserEmail = info?.Email
                    };
                })
                .ToList();

            ViewBag.Query = q;
            return View(vms);
        }
         
        [HttpGet]
        public async Task<IActionResult> Delete(int id)
        {
            if (!IsStaffOrAdmin()) return RedirectToAction("Index", "Home");

            var f = await _feedbackService.GetByIdAsync(id);
            if (f == null) return NotFound();

            // Lấy user info
            var users = await _userService.GetAllAsync(onlyActive: false);
            var info = users.FirstOrDefault(u => u.Id == f.UserId);

            var vm = new AdminFeedbackViewModel
            {
                Id = f.Id,
                UserId = f.UserId,
                FeedbackType = f.FeedbackType,
                Content = f.Content,
                Datetime = f.Datetime,
                UserFullName = info?.FullName,
                UserEmail = info?.Email
            };
            return View(vm); // View: Delete.cshtml
        }

        [HttpPost, ActionName("Delete")]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> DeleteConfirmed(int id)
        {
            if (!IsStaffOrAdmin()) return RedirectToAction("Index", "Home");

            var res = await _feedbackService.DeleteAsync(id); // HARD DELETE
            TempData["msg"] = res.ok ? "Đã xóa feedback." : $"Xóa thất bại: {res.message}";
            return RedirectToAction("IndexAdmin","Feedback");
        }
    }
}
