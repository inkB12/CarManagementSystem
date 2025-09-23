using CarManagementSystem.Services.Interfaces;
using CarManagementSystem.WebMVC.Models;
using Microsoft.AspNetCore.Mvc;

namespace CarManagementSystem.WebMVC.Controllers
{
    public class AuthController : Controller
    {
        private readonly IUserService _users;

        public AuthController(IUserService users)
        {
            _users = users;
        }

        [HttpGet]
        public IActionResult Login() => View(new LoginViewModel());

        [HttpPost]
        public async Task<IActionResult> Login(LoginViewModel vm)
        {
            if (!ModelState.IsValid) return View(vm);

            var response = await _users.LoginAsync(vm.Email, vm.Password);
            if (!response.ok)
            {
                ModelState.AddModelError(string.Empty, response.message);
                return View(vm);
            }

            TempData["msg"] = "Login success";
            return RedirectToAction("Index","Home");
        }

        [HttpGet]
        public IActionResult Register() => View(new RegisterViewModel());

        [HttpPost]
        public async Task<IActionResult> Register(RegisterViewModel vm)
        {
            if (!ModelState.IsValid) return View(vm);

            var res = await _users.RegisterAsync(vm.Email, vm.Password, vm.FullName, vm.Phone, "Customer");
            if (!res.ok)
            {
                ModelState.AddModelError(string.Empty, res.message);
                return View(vm);
            }

            TempData["msg"] = "Register success. Please login.";
            return RedirectToAction("Login");
        }

        public IActionResult Logout()
        {
            // TODO: clear cookie/session nếu bạn implement auth
            TempData["msg"] = "Logged out";
            return RedirectToAction("Login");
        }
    }
}
