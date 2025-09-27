using CarManagementSystem.BusinessObjects;
using CarManagementSystem.Services.Interfaces;
using CarManagementSystem.WebMVC.Models;
using Microsoft.AspNetCore.Mvc;

namespace CarManagementSystem.WebMVC.Controllers
{
    public class CarCompaniesController : Controller
    {
        private readonly ICarCompanyService _service;

        public CarCompaniesController(ICarCompanyService service)
        {
            _service = service;
        }

        // GET: CarCompanies
        public async Task<IActionResult> Index()
        {
            var companies = await _service.GetAllAsync(false);
            var vm = companies.Select(c => new CarCompanyViewModel
            {
                Id = c.Id,
                CatalogName = c.CatalogName,
                Description = c.Description,
                IsActive = c.IsActive
            }).ToList();

            ViewData["Title"] = "Car Companies";
            return View(vm);
        }

        // GET: CarCompanies/Create
        public IActionResult Create()
        {
            return View();
        }

        // POST: CarCompanies/Create
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Create(CarCompanyViewModel vm)
        {
            if (!ModelState.IsValid) return View(vm);

            var entity = new CarCompany
            {
                CatalogName = vm.CatalogName,
                Description = vm.Description
                // ❌ KHÔNG set IsActive ở đây, service sẽ tự set = true
            };

            var (ok, message, _) = await _service.CreateAsync(entity);

            if (ok) TempData["SuccessMessage"] = message;
            else TempData["ErrorMessage"] = message;

            return RedirectToAction(nameof(Index));
        }

        // GET: CarCompanies/Edit/5
        public async Task<IActionResult> Edit(int id)
        {
            var entity = await _service.GetByIdAsync(id);
            if (entity == null) return NotFound();

            var vm = new CarCompanyViewModel
            {
                Id = entity.Id,
                CatalogName = entity.CatalogName,
                Description = entity.Description,
                IsActive = entity.IsActive
            };

            return View(vm);
        }

        // POST: CarCompanies/Edit
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Edit(CarCompanyViewModel vm)
        {
            if (!ModelState.IsValid) return View(vm);

            var entity = new CarCompany
            {
                Id = vm.Id,
                CatalogName = vm.CatalogName,
                Description = vm.Description
                // ❌ KHÔNG set IsActive, service sẽ bỏ qua
            };

            var (ok, message, _) = await _service.UpdateAsync(entity);

            if (ok) TempData["SuccessMessage"] = message;
            else TempData["ErrorMessage"] = message;

            return RedirectToAction(nameof(Index));
        }

        // GET: CarCompanies/Delete/5
        public async Task<IActionResult> Delete(int id)
        {
            var entity = await _service.GetByIdAsync(id);
            if (entity == null) return NotFound();

            var vm = new CarCompanyViewModel
            {
                Id = entity.Id,
                CatalogName = entity.CatalogName,
                Description = entity.Description,
                IsActive = entity.IsActive
            };

            return View(vm);
        }

        // POST: CarCompanies/DeleteConfirmed
        [HttpPost, ActionName("Delete")]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> DeleteConfirmed(int id)
        {
            var (ok, message) = await _service.DeleteAsync(id);

            if (ok) TempData["SuccessMessage"] = message;
            else TempData["ErrorMessage"] = message;

            return RedirectToAction(nameof(Index));
        }
    }
}
