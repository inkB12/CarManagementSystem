using CarManagementSystem.BusinessObjects;
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

        // GET: /Users/Edit/5
        [HttpGet]
        public async Task<IActionResult> Edit(int id)
        {
            //user cố tình sửa id trên link thì cho nó về home
            var sessionUserId = HttpContext.Session.GetInt32("UserId");
            if (sessionUserId == null || sessionUserId != id)
            {
                return RedirectToAction("Index", "Home");
            }

            var user = await _users.GetByIdAsync(id);
            if (user == null) return NotFound();

            var vm = new UpdateUserViewModel
            {
                Id = user.Id,
                FullName = user.FullName,
                Email = user.Email,
                Phone = user.Phone
            };

            return View(vm);
        }

        // POST: /Users/Edit/{id}
        [HttpPost]
        public async Task<IActionResult> Edit(UpdateUserViewModel vm)
        {
            //user cố tình sửa id trên link thì cho nó về home
            var sessionUserId = HttpContext.Session.GetInt32("UserId");
            if (sessionUserId == null || sessionUserId != vm.Id)
            {
                return RedirectToAction("Index", "Home");
            }

            if (!ModelState.IsValid) return View(vm);

            var user = await _users.GetByIdAsync(vm.Id);
            if (user == null) return NotFound();

            user.FullName = vm.FullName;
            user.Email = vm.Email;
            user.Phone = vm.Phone;

            if (!string.IsNullOrWhiteSpace(vm.Password))
                user.Password = vm.Password; 

            var response = await _users.UpdateAsync(user);
            if (!response.ok)
            {
                ModelState.AddModelError(string.Empty, response.message);
                return View(vm);
            }

            // lưu vào session
            HttpContext.Session.SetInt32("UserId", response.data.Id);
            HttpContext.Session.SetString("UserEmail", response.data.Email);
            HttpContext.Session.SetString("UserRole", response.data.Role);
            HttpContext.Session.SetString("UserFullName", response.data.FullName);

            TempData["msg"] = "Cập nhật thành công!";
            return RedirectToAction("Index", "Home");
        }
    }
}
