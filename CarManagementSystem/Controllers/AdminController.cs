
using CarManagementSystem.Services.Interfaces;
using CarManagementSystem.WebMVC.Models;
using Microsoft.AspNetCore.Mvc;

namespace CarManagementSystem.WebMVC.Controllers
{
    public class AdminController : Controller
    {
        private readonly ICarCompanyService _carCompanyService;
        private readonly IVehicleCategoryService _categoryService;
        private readonly IElectricVehicleService _vehicleService;


        public AdminController(
            ICarCompanyService carCompanyService,
            IVehicleCategoryService categoryService,
            IElectricVehicleService vehicleService)
        {
            _carCompanyService = carCompanyService;
            _categoryService = categoryService;
            _vehicleService = vehicleService;
        }

        private bool IsAdmin()
        {
            var role = HttpContext.Session.GetString("UserRole");
            return string.Equals(role, "Admin", StringComparison.OrdinalIgnoreCase);
        }
        public async Task<IActionResult> Index()
        {
            if (!IsAdmin()) return RedirectToAction("Index", "Home");
            var vm = new DashboardViewModel
            {
                TotalCarCompanies = (await _carCompanyService.GetAllAsync()).Count,
                TotalCategories = (await _categoryService.GetAllAsync()).Count,
                TotalVehicles = (await _vehicleService.GetAllAsync()).Count
            };

            ViewData["Title"] = "Dashboard";
            return View(vm);
        }
    }
}
