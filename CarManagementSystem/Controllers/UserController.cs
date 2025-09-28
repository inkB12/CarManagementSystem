using CarManagementSystem.BusinessObjects;
using CarManagementSystem.Services.Interfaces;
using CarManagementSystem.Services.Services;
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

        //-----------------Admin Controller-----------------//
        private bool IsAdmin()
        {
            var role = HttpContext.Session.GetString("UserRole");
            return string.Equals(role, "Admin", StringComparison.OrdinalIgnoreCase);
        }

        public async Task<IActionResult> Index()
        {
            if (!IsAdmin()) return RedirectToAction("Index", "Home");
            var list = await _users.GetAllAsync(onlyActive: false);
            return View(list);
        }

        //Redirect sang trang tạo user
        [HttpGet]
        public IActionResult Create()
        {
            if (!IsAdmin()) return RedirectToAction("Index", "Home");
            return View(new AdminCreateUserViewModel());
        }

        //Admin POST: /Users/Create
        [HttpPost]
        public async Task<IActionResult> Create(AdminCreateUserViewModel vm)
        {
            if (!IsAdmin()) return RedirectToAction("Index", "Home");
            if (!ModelState.IsValid) return View(vm);

            var res = await _users.RegisterAsync(
                email: vm.Email,
                password: vm.Password,
                fullName: vm.FullName,
                phone: vm.Phone,
                role: vm.Role
            );

            if (!res.ok)
            {
                ModelState.AddModelError(string.Empty, res.message);
                return View(vm);
            }

            if (res.data is { } created && created.IsActive != vm.IsActive)
            {
                created.IsActive = vm.IsActive;

                await _users.UpdateAsync(created);
            }

            TempData["msg"] = "Tạo người dùng thành công.";
            return RedirectToAction(nameof(Index));
        }

        //Admin Redirect GET: /Users/AdminEdit/5  
        [HttpGet]
        public async Task<IActionResult> EditAdmin(int id)
        {
            if (!IsAdmin()) return RedirectToAction("Index", "Home");

            var user = await _users.GetByIdAsync(id);
            if (user == null) return NotFound();

            var vm = new AdminUpdateUserViewModel
            {
                Id = user.Id,
                FullName = user.FullName,
                Email = user.Email,
                Phone = user.Phone,
                Role = user.Role,
                IsActive = user.IsActive
            };
            return View(vm);
        }

        //Admin POST: /Users/Edit/{id} 
        [HttpPost]
        public async Task<IActionResult> EditAdmin(AdminUpdateUserViewModel vm)
        {
            if (!IsAdmin()) return RedirectToAction("Index", "Home");
            if (!ModelState.IsValid) return View(vm);

            var user = await _users.GetByIdAsync(vm.Id);
            if (user == null) return NotFound();

            user.FullName = vm.FullName.Trim();
            user.Email = vm.Email.Trim();
            user.Phone = vm.Phone;
            user.Role = vm.Role;
            user.IsActive = vm.IsActive;

            if (!string.IsNullOrWhiteSpace(vm.Password))
            {
                if (vm.Password.Length < 6)
                {
                    ModelState.AddModelError("Password", "Mật khẩu tối thiểu 6 ký tự.");
                    return View(vm);
                }

                user.Password = UserService.HashSHA256(vm.Password);
            }

            var res = await _users.UpdateAsync(user);
            if (!res.ok)
            {
                ModelState.AddModelError(string.Empty, res.message);
                return View(vm);
            }

            TempData["msg"] = "Cập nhật người dùng thành công.";
            return RedirectToAction(nameof(Index));
        }

        //Delete user (hard delete)
        [HttpGet]
        public async Task<IActionResult> Delete(int id)
        {
            if (!IsAdmin()) return RedirectToAction("Index", "Home");
            var user = await _users.GetByIdAsync(id);
            if (user == null) return NotFound();
            return View(user);
        }

        //Delete confirm 
        [HttpPost, ActionName("Delete")]
        public async Task<IActionResult> DeleteConfirmed(int id)
        {
            if (!IsAdmin()) return RedirectToAction("Index", "Home");
            var res = await _users.DeleteAsync(id);
            TempData["msg"] = res.ok ? "Đã xóa người dùng." : $"Xóa thất bại: {res.message}";
            return RedirectToAction(nameof(Index));
        }


        //------------------User Edit Profile------------------//
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
            {
                if (vm.Password.Length >= 6)
                {
                    user.Password = UserService.HashSHA256(vm.Password);
                }
                else
                {
                    ModelState.AddModelError("Password", "Wrong format password (min 6 characters).");
                    return View(vm);
                }
            }


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
