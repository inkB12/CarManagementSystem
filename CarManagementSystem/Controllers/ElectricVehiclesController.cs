// WebMVC/Controllers/ElectricVehiclesController.cs
using CarManagementSystem.Services.Interfaces;
using CarManagementSystem.WebMVC.Models;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Rendering;

namespace CarManagementSystem.WebMVC.Controllers
{
    public class ElectricVehiclesController : Controller
    {
        private readonly IElectricVehicleService _vehicleSvc;
        private readonly ICarCompanyService _companySvc;
        private readonly IVehicleCategoryService _categorySvc;

        public ElectricVehiclesController(
            IElectricVehicleService vehicleSvc,
            ICarCompanyService companySvc,
            IVehicleCategoryService categorySvc)
        {
            _vehicleSvc = vehicleSvc;
            _companySvc = companySvc;
            _categorySvc = categorySvc;
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

            var vm = new ElectricVehicleDetailVM
            {
                Id = v.Id,
                Model = v.Model,
                Version = v.Version,
                Price = v.Price,
                ImageUrl = string.IsNullOrWhiteSpace(v.ImageUrl) ? null : v.ImageUrl,
                Color = v.Color,
                CompanyName = v.CarCompany?.CatalogName ?? v.CarCompany?.ToString() ?? "N/A",
                CategoryName = v.VehicleCategory?.CategoryName ?? "N/A",
                Specification = v.Specification,
                SpecLines = (v.Specification ?? "")
                            .Split(new[] { '\n', '\r' }, StringSplitOptions.RemoveEmptyEntries)
                            .Select(s => s.Trim('•', '-', ' ')).Where(s => !string.IsNullOrWhiteSpace(s)).ToList()
            };

            return View(vm);
            }
        }
}
