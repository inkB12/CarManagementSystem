using CarManagementSystem.Services.Interfaces;
using CarManagementSystem.WebMVC.Models;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Rendering;
using CarManagementSystem.BusinessObjects;
using CarManagementSystem.Services.Interfaces;
using CarManagementSystem.WebMVC.Models;
using CloudinaryDotNet;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Rendering;
using CloudinaryDotNet;
using CloudinaryDotNet.Actions;
using CarManagementSystem.Services.Services;

namespace CarManagementSystem.WebMVC.Controllers
{
    public class ElectricVehiclesController : Controller
    {
        private readonly IElectricVehicleService _vehicleSvc;
        private readonly ICarCompanyService _companySvc;
        private readonly IVehicleCategoryService _categorySvc;
        private readonly Cloudinary _cloudinary;   // ✅ thêm Cloudinary

        public ElectricVehiclesController(
            IElectricVehicleService service,
            ICarCompanyService carCompanyService,
            IVehicleCategoryService categoryService,
            Cloudinary cloudinary) // ✅ inject từ DI
        {
            _vehicleSvc = service;
            _companySvc = carCompanyService;
            _categorySvc = categoryService;
            _cloudinary = cloudinary;
        }

        public async Task<IActionResult> Index(
            int page = 1, int pageSize = 6,
            int? companyId = null, int? categoryId = null,
            string? q = null)
        {
            // 1) Lấy tất cả xe (repo đã Include Company/Category)
            var all = await _vehicleSvc.GetAllAsync(true);

            // 2) Áp bộ lọc
            if (companyId.HasValue)
                all = all.Where(v => v.CarCompanyId == companyId.Value).ToList();

            if (categoryId.HasValue)
                all = all.Where(v => v.VehicleCategoryId == categoryId.Value).ToList();

            if (!string.IsNullOrWhiteSpace(q))
            {
                var s = q.Trim().ToLower();
                all = all.Where(v =>
                        (v.Model?.ToLower().Contains(s) ?? false) ||
                        (v.Version?.ToLower().Contains(s) ?? false) ||
                        (v.Specification?.ToLower().Contains(s) ?? false) ||
                        (v.CarCompany?.CatalogName?.ToLower().Contains(s) ?? false) ||
                        (v.VehicleCategory?.CategoryName?.ToLower().Contains(s) ?? false))
                     .ToList();
            }

            // 3) Phân trang (6 sp/trang)
            var totalCount = all.Count;
            var totalPages = (int)Math.Ceiling(totalCount / (double)pageSize);
            page = Math.Max(1, Math.Min(page, Math.Max(totalPages, 1)));
            var items = all
                .OrderBy(v => v.Price) // hoặc .OrderByDescending(v => v.Id)
                .Skip((page - 1) * pageSize)
                .Take(pageSize)
                .ToList();

            // 4) Nạp dữ liệu filter
            var companies = await _companySvc.GetAllAsync(true);
            var categories = await _categorySvc.GetAllAsync(true);

            var vm = new ElectricVehicleListViewModel
            {
                Vehicles = items,
                Q = q,
                CompanyId = companyId,
                CategoryId = categoryId,
                CurrentPage = page,
                PageSize = pageSize,
                TotalPages = totalPages,
                TotalCount = totalCount,
                CompanyOptions = companies
                    .Select(c => new SelectListItem { Value = c.Id.ToString(), Text = c.CatalogName ?? c.ToString(), Selected = companyId == c.Id })
                    .ToList(),
                CategoryOptions = categories
                    .Select(cat => new SelectListItem { Value = cat.Id.ToString(), Text = cat.CategoryName ?? cat.ToString(), Selected = categoryId == cat.Id })
                    .ToList()
            };

            return View(vm);
        }

        public async Task<IActionResult> Details(int id)
        {
            var v = await _vehicleSvc.GetByIdAsync(id);
            if (v == null) return NotFound();

            var vm = new ElectricVehicleDetailVM()
            {
                ImageUrl = string.IsNullOrWhiteSpace(v.ImageUrl) ? null : v.ImageUrl,
                Color = v.Color,
                Price = v.Price,
                CompanyName = v.CarCompany?.CatalogName ?? v.CarCompany?.ToString() ?? "N/A",
                CategoryName = v.VehicleCategory?.CategoryName ?? "N/A",
                Specification = v.Specification,
                SpecLines = (v.Specification ?? "")
                            .Split(new[] { '\n', '\r' }, StringSplitOptions.RemoveEmptyEntries)
                            .Select(s => s.Trim('•', '-', ' ')).Where(s => !string.IsNullOrWhiteSpace(s)).ToList()
            };

            return View(vm);
        }


        // GET: ElectricVehicles
        public async Task<IActionResult> IndexAdmin()
        {
            var vehicles = await _vehicleSvc.GetAllAsync(false);

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

            var (ok, message, _) = await _vehicleSvc.CreateAsync(entity);
            TempData[ok ? "SuccessMessage" : "ErrorMessage"] = message;

            return RedirectToAction(nameof(Index));
        }

        // GET: ElectricVehicles/Edit/5
        public async Task<IActionResult> Edit(int id)
        {
            var entity = await _vehicleSvc.GetByIdAsync(id);
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

            var (ok, message, _) = await _vehicleSvc.UpdateAsync(entity);
            TempData[ok ? "SuccessMessage" : "ErrorMessage"] = message;

            return RedirectToAction(nameof(Index));
        }

        // GET: ElectricVehicles/Delete/5
        public async Task<IActionResult> Delete(int id)
        {
            var entity = await _vehicleSvc.GetByIdAsync(id);
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
            var (ok, message) = await _vehicleSvc.DeleteAsync(id);
            TempData[ok ? "SuccessMessage" : "ErrorMessage"] = message;
            return RedirectToAction(nameof(Index));
        }

        // chỉ load những CarCompany và VehicleCategory còn active
        private async Task LoadDropdowns()
        {
            var companies = await _companySvc.GetAllAsync(true);
            var categories = await _categorySvc.GetAllAsync(true);

            ViewBag.CarCompanies = new SelectList(companies, "Id", "CatalogName");
            ViewBag.Categories = new SelectList(categories, "Id", "CategoryName");
        }
    }
}
