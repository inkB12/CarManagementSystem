using CarManagementSystem.DataAccess;
using CarManagementSystem.Services.Interfaces;
using CarManagementSystem.WebMVC.Models;
using Microsoft.AspNetCore.Mvc;

namespace CarManagementSystem.WebMVC.Controllers
{
    public class UserController : Controller
    {
        private readonly IUserService _users;

        public UserController(IUserService users)
        {
            _users = users;
        }

        public async Task<IActionResult> Index()
        {
            var data = await _users.GetAllAsync(onlyActive: false);
            return View(data);
        }

        [HttpGet]
        public IActionResult Create() => View(new UserCreateViewModel());

        [ValidateAntiForgeryToken]
        [HttpPost]
        public async Task<IActionResult> Create(UserCreateViewModel vm)
        {
            if (!ModelState.IsValid) return View(vm);

            var user = new User
            {
                Email = vm.Email.Trim(),
                FullName = vm.FullName.Trim(),
                Phone = vm.Phone,
                Password = vm.Password, // service sẽ hash nếu là plain
                Role = vm.Role,
                IsActive = vm.IsActive
            };

            var res = await _users.CreateAsync(user);
            if (!res.ok)
            {
                ModelState.AddModelError(string.Empty, res.message);
                return View(vm);
            }

            TempData["msg"] = "Created";
            return RedirectToAction(nameof(Index));
        }

        [HttpGet]
        public async Task<IActionResult> Edit(int id)
        {
            var u = await _users.GetByIdAsync(id);
            if (u == null) return NotFound();

            var vm = new UserEditViewModel
            {
                Id = u.Id,
                Email = u.Email,
                FullName = u.FullName,
                Phone = u.Phone,
                Role = u.Role,
                IsActive = u.IsActive
            };
            return View(vm);
        }

        [ValidateAntiForgeryToken]
        [HttpPost]
        public async Task<IActionResult> Edit(UserEditViewModel vm)
        {
            if (!ModelState.IsValid) return View(vm);

            var u = await _users.GetByIdAsync(vm.Id);
            if (u == null) return NotFound();

            u.Email = vm.Email.Trim();
            u.FullName = vm.FullName.Trim();
            u.Phone = vm.Phone;
            u.Role = vm.Role;
            u.IsActive = vm.IsActive;
            u.Password = string.IsNullOrWhiteSpace(vm.Password) ? "" : vm.Password; // service sẽ giữ hash cũ nếu trống

            var res = await _users.UpdateAsync(u);
            if (!res.ok)
            {
                ModelState.AddModelError(string.Empty, res.message);
                return View(vm);
            }

            TempData["msg"] = "Updated";
            return RedirectToAction(nameof(Index));
        }

        [ValidateAntiForgeryToken]
        [HttpPost]
        public async Task<IActionResult> Delete(int id)
        {
            var res = await _users.DeleteAsync(id);
            if (!res.ok) TempData["error"] = res.message;
            else TempData["msg"] = "Deleted";

            return RedirectToAction(nameof(Index));
        }
    }
}
