using CarManagementSystem.BusinessObjects;
using CarManagementSystem.Services.Interfaces;
using CarManagementSystem.WebMVC.Models;
using CloudinaryDotNet;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Rendering;
using CloudinaryDotNet;
using CloudinaryDotNet.Actions;

namespace CarManagementSystem.WebMVC.Controllers
{
    public class ElectricVehiclesController : Controller
    {
        private readonly IElectricVehicleService _service;
        private readonly ICarCompanyService _carCompanyService;
        private readonly IVehicleCategoryService _categoryService;
        private readonly Cloudinary _cloudinary;   // ✅ thêm Cloudinary

        public ElectricVehiclesController(
            IElectricVehicleService service,
            ICarCompanyService carCompanyService,
            IVehicleCategoryService categoryService,
            Cloudinary cloudinary) // ✅ inject từ DI
        {
            _service = service;
            _carCompanyService = carCompanyService;
            _categoryService = categoryService;
            _cloudinary = cloudinary;
        }

        // GET: ElectricVehicles
        public async Task<IActionResult> Index()
        {
            var vehicles = await _service.GetAllAsync(false);

            var vm = vehicles.Select(v => new VehicleViewModel
            {
                Id = v.Id,
                Model = v.Model,
                Version = v.Version,
                Price = v.Price,
                Specification = v.Specification,
                Color = v.Color,
                ImageUrl = v.ImageUrl,
                CarCompanyId = v.CarCompanyId,
                CarCompanyName = v.CarCompany?.CatalogName,
                VehicleCategoryId = v.VehicleCategoryId,
                VehicleCategoryName = v.VehicleCategory?.CategoryName,
                IsActive = v.IsActive
            }).ToList();

            ViewData["Title"] = "Vehicles";
            return View(vm);
        }

        // GET: ElectricVehicles/Create
        public async Task<IActionResult> Create()
        {
            await LoadDropdowns();
            return View();
        }

        // POST: ElectricVehicles/Create
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Create(VehicleViewModel vm)
        {
            if (!ModelState.IsValid)
            {
                await LoadDropdowns();
                return View(vm);
            }

            string? imageUrl = null;
            if (vm.ImageFile != null && vm.ImageFile.Length > 0)
            {
                using var stream = vm.ImageFile.OpenReadStream();
                var uploadParams = new ImageUploadParams
                {
                    File = new FileDescription(vm.ImageFile.FileName, stream),
                    Folder = "electric_vehicles"
                };
                var uploadResult = await _cloudinary.UploadAsync(uploadParams);
                imageUrl = uploadResult.SecureUrl?.ToString();
            }

            var entity = new ElectricVehicle
            {
                Model = vm.Model,
                Version = vm.Version,
                Price = vm.Price,
                Specification = vm.Specification,
                Color = vm.Color,
                ImageUrl = imageUrl,
                CarCompanyId = vm.CarCompanyId,
                VehicleCategoryId = vm.VehicleCategoryId
                // ❌ không set IsActive, service sẽ tự set = true
            };

            var (ok, message, _) = await _service.CreateAsync(entity);
            TempData[ok ? "SuccessMessage" : "ErrorMessage"] = message;

            return RedirectToAction(nameof(Index));
        }

        // GET: ElectricVehicles/Edit/5
        public async Task<IActionResult> Edit(int id)
        {
            var entity = await _service.GetByIdAsync(id);
            if (entity == null) return NotFound();

            var vm = new VehicleViewModel
            {
                Id = entity.Id,
                Model = entity.Model,
                Version = entity.Version,
                Price = entity.Price,
                Specification = entity.Specification,
                Color = entity.Color,
                ImageUrl = entity.ImageUrl,
                CarCompanyId = entity.CarCompanyId,
                VehicleCategoryId = entity.VehicleCategoryId,
                IsActive = entity.IsActive
            };

            await LoadDropdowns();
            return View(vm);
        }

        // POST: ElectricVehicles/Edit
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Edit(VehicleViewModel vm)
        {
            if (!ModelState.IsValid)
            {
                await LoadDropdowns();
                return View(vm);
            }

            string? imageUrl = vm.ImageUrl;
            if (vm.ImageFile != null && vm.ImageFile.Length > 0)
            {
                using var stream = vm.ImageFile.OpenReadStream();
                var uploadParams = new ImageUploadParams
                {
                    File = new FileDescription(vm.ImageFile.FileName, stream),
                    Folder = "electric_vehicles"
                };
                var uploadResult = await _cloudinary.UploadAsync(uploadParams);
                imageUrl = uploadResult.SecureUrl?.ToString();
            }

            var entity = new ElectricVehicle
            {
                Id = vm.Id,
                Model = vm.Model,
                Version = vm.Version,
                Price = vm.Price,
                Specification = vm.Specification,
                Color = vm.Color,
                ImageUrl = imageUrl,
                CarCompanyId = vm.CarCompanyId,
                VehicleCategoryId = vm.VehicleCategoryId
                // ❌ không set IsActive
            };

            var (ok, message, _) = await _service.UpdateAsync(entity);
            TempData[ok ? "SuccessMessage" : "ErrorMessage"] = message;

            return RedirectToAction(nameof(Index));
        }

        // GET: ElectricVehicles/Delete/5
        public async Task<IActionResult> Delete(int id)
        {
            var entity = await _service.GetByIdAsync(id);
            if (entity == null) return NotFound();

            var vm = new VehicleViewModel
            {
                Id = entity.Id,
                Model = entity.Model,
                CarCompanyName = entity.CarCompany?.CatalogName,
                VehicleCategoryName = entity.VehicleCategory?.CategoryName,
                Price = entity.Price
            };

            return View(vm);
        }

        [HttpPost, ActionName("Delete")]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> DeleteConfirmed(int id)
        {
            var (ok, message) = await _service.DeleteAsync(id);
            TempData[ok ? "SuccessMessage" : "ErrorMessage"] = message;
            return RedirectToAction(nameof(Index));
        }

        // chỉ load những CarCompany và VehicleCategory còn active
        private async Task LoadDropdowns()
        {
            var companies = await _carCompanyService.GetAllAsync(true);
            var categories = await _categoryService.GetAllAsync(true);

            ViewBag.CarCompanies = new SelectList(companies, "Id", "CatalogName");
            ViewBag.Categories = new SelectList(categories, "Id", "CategoryName");
        }
    }
}
