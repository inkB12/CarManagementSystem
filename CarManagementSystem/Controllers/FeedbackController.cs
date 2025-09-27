using CarManagementSystem.BusinessObjects;
using CarManagementSystem.Services.Interfaces;
using CarManagementSystem.WebMVC.Models;
using Microsoft.AspNetCore.Mvc;

namespace CarManagementSystem.WebMVC.Controllers
{
    public class FeedbackController : Controller
    {
        private readonly IFeedbackService _feedbackService;

        public FeedbackController(IFeedbackService feedbackService)
        {
            _feedbackService = feedbackService;
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
                UserId = 1 // TODO: thay bằng user đăng nhập
            };

            var result = await _feedbackService.CreateAsync(entity);
            if (result.ok) return RedirectToAction(nameof(Index));

            ModelState.AddModelError("", result.message);
            return View(vm);
        }
    }
}
