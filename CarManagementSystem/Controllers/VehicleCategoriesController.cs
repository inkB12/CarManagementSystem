using CarManagementSystem.BusinessObjects;
using CarManagementSystem.Services.Interfaces;
using CarManagementSystem.WebMVC.Models;
using Microsoft.AspNetCore.Mvc;

namespace CarManagementSystem.WebMVC.Controllers
{
    public class VehicleCategoriesController : Controller
    {
        private readonly IVehicleCategoryService _service;

        public VehicleCategoriesController(IVehicleCategoryService service)
        {
            _service = service;
        }

        private bool IsAdmin()
        {
            var role = HttpContext.Session.GetString("UserRole");
            return string.Equals(role, "Admin", StringComparison.OrdinalIgnoreCase);
        }

        // GET: VehicleCategories
        public async Task<IActionResult> Index()
        {
            if (!IsAdmin()) return RedirectToAction("Index", "Home");
            var categories = await _service.GetAllAsync(false);
            var vm = categories.Select(c => new VehicleCategoryViewModel
            {
                Id = c.Id,
                CategoryName = c.CategoryName,
                IsActive = c.IsActive
            }).ToList();

            ViewData["Title"] = "Vehicle Categories";
            return View(vm);
        }

        // GET: VehicleCategories/Create
        public IActionResult Create()
        {
            return View();
        }

        // POST: VehicleCategories/Create
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Create(VehicleCategoryViewModel vm)
        {
            if (!IsAdmin()) return RedirectToAction("Index", "Home");
            if (!ModelState.IsValid) return View(vm);

            var entity = new VehicleCategory
            {
                CategoryName = vm.CategoryName
                // ❌ không set IsActive, service sẽ auto = true
            };

            var (ok, message, _) = await _service.CreateAsync(entity);

            if (ok) TempData["SuccessMessage"] = message;
            else TempData["ErrorMessage"] = message;

            return RedirectToAction(nameof(Index));
        }



        // GET: VehicleCategories/Edit/5
        public async Task<IActionResult> Edit(int id)
        {
            if (!IsAdmin()) return RedirectToAction("Index", "Home");
            var entity = await _service.GetByIdAsync(id);
            if (entity == null) return NotFound();

            var vm = new VehicleCategoryViewModel
            {
                Id = entity.Id,
                CategoryName = entity.CategoryName,
                IsActive = entity.IsActive
            };

            return View(vm);
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Edit(VehicleCategoryViewModel vm)
        {
            if (!IsAdmin()) return RedirectToAction("Index", "Home");
            if (!ModelState.IsValid) return View(vm);

            var entity = new VehicleCategory
            {
                Id = vm.Id,
                CategoryName = vm.CategoryName
                // ❌ không set IsActive
            };

            var (ok, message, _) = await _service.UpdateAsync(entity);

            if (ok) TempData["SuccessMessage"] = message;
            else TempData["ErrorMessage"] = message;

            return RedirectToAction(nameof(Index));
        }


        // GET: VehicleCategories/Delete/5
        public async Task<IActionResult> Delete(int id)
        {
            if (!IsAdmin()) return RedirectToAction("Index", "Home");
            var entity = await _service.GetByIdAsync(id);
            if (entity == null) return NotFound();

            var vm = new VehicleCategoryViewModel
            {
                Id = entity.Id,
                CategoryName = entity.CategoryName,
                IsActive = entity.IsActive
            };

            return View(vm);
        }

        // POST: VehicleCategories/DeleteConfirmed
        [HttpPost, ActionName("Delete")]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> DeleteConfirmed(int id)
        {
            if (!IsAdmin()) return RedirectToAction("Index", "Home");
            var (ok, message) = await _service.DeleteAsync(id);

            if (ok) TempData["SuccessMessage"] = message;
            else TempData["ErrorMessage"] = message;

            return RedirectToAction(nameof(Index));
        }
    }
}
