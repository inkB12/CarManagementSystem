using System;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using CarManagementSystem.Services.Interfaces;
using CarManagementSystem.WebMVC.Models;
using CarManagementSystem.BusinessObjects;

namespace CarManagementSystem.WebMVC.Controllers
{
    public class PromotionsController : Controller
    {
        private readonly IPromotionService _svc;
        public PromotionsController(IPromotionService svc) => _svc = svc;

        // LIST
        [HttpGet("admin/promotions", Name = "AdminPromotionIndex")]
        public async Task<IActionResult> Index(int page = 1, int pageSize = 10, string? q = null, string? status = null)
        {
            var all = await _svc.GetAllAsync(false);

            if (!string.IsNullOrWhiteSpace(q))
            {
                var s = q.Trim().ToLower();
                all = all.Where(x =>
                    (x.Code?.ToLower().Contains(s) ?? false) ||
                    (x.Description?.ToLower().Contains(s) ?? false)).ToList();
            }

            if (!string.IsNullOrWhiteSpace(status))
                all = all.Where(x => string.Equals(x.Status, status, StringComparison.OrdinalIgnoreCase)).ToList();

            var totalCount = all.Count;
            var totalPages = (int)Math.Ceiling(totalCount / (double)pageSize);
            page = Math.Max(1, Math.Min(page, Math.Max(totalPages, 1)));

            var items = all.OrderByDescending(x => x.Id)
                           .Skip((page - 1) * pageSize)
                           .Take(pageSize)
                           .ToList();

            var vm = new PromotionListViewModels
            {
                Q = q,
                Status = status,
                CurrentPage = page,
                PageSize = pageSize,
                TotalPages = totalPages,
                TotalCount = totalCount,
                Items = items
            };

            var role = HttpContext.Session.GetString("UserRole") ?? "Admin";
            ViewBag.Layout = GetLayout(role);
            ViewBag.Role = role;

            return View(vm);
        }

        // CREATE
        [HttpGet("admin/promotions/create", Name = "AdminPromotionCreateGet")]
        public IActionResult Create() => View(new PromotionFormViewModel { Status = "Active" });

        [HttpPost("admin/promotions/create", Name = "AdminPromotionCreatePost")]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Create(PromotionFormViewModel vm)
        {
            if (!ModelState.IsValid) return View(vm);

            var entity = new Promotion
            {
                Code = vm.Code?.Trim() ?? "",
                Description = vm.Description,
                Discount = vm.Discount,
                Status = string.IsNullOrWhiteSpace(vm.Status) ? "Active" : vm.Status
            };

            var (ok, msg, _) = await _svc.CreateAsync(entity);
            TempData[ok ? "SuccessMessage" : "ErrorMessage"] = msg;
            return RedirectToRoute("AdminPromotionIndex");
        }

        // EDIT
        [HttpGet("admin/promotions/edit/{id:int}", Name = "AdminPromotionEditGet")]
        public async Task<IActionResult> Edit(int id)
        {
            var p = await _svc.GetByIdAsync(id);
            if (p == null) return NotFound();

            var vm = new PromotionFormViewModel
            {
                Id = p.Id,
                Code = p.Code,
                Description = p.Description,
                Discount = p.Discount,
                Status = p.Status
            };
            return View(vm);
        }

        // POST KHÔNG có {id} trên URL – nhận Id từ hidden field
        [HttpPost("admin/promotions/edit", Name = "AdminPromotionEditPost")]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Edit(PromotionFormViewModel vm)
        {
            if (!ModelState.IsValid) return View(vm);

            var entity = new Promotion
            {
                Id = vm.Id,
                Code = vm.Code?.Trim() ?? "",
                Description = vm.Description,
                Discount = vm.Discount,
                Status = vm.Status
            };

            var (ok, msg, _) = await _svc.UpdateAsync(entity);
            TempData[ok ? "SuccessMessage" : "ErrorMessage"] = msg;
            return RedirectToRoute("AdminPromotionIndex");
        }

        // DELETE
        [HttpGet("admin/promotions/delete/{id:int}", Name = "AdminPromotionDeleteGet")]
        public async Task<IActionResult> Delete(int id)
        {
            var p = await _svc.GetByIdAsync(id);
            if (p == null) return NotFound();

            var vm = new PromotionFormViewModel
            {
                Id = p.Id,
                Code = p.Code,
                Description = p.Description,
                Discount = p.Discount,
                Status = p.Status
            };
            return View(vm);
        }

        [HttpPost("admin/promotions/delete", Name = "AdminPromotionDeletePost")]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> DeleteConfirmed(int id)
        {
            var (ok, msg) = await _svc.DeleteAsync(id);
            TempData[ok ? "SuccessMessage" : "ErrorMessage"] = msg;
            return RedirectToRoute("AdminPromotionIndex");
        }

        private string GetLayout(string userRole)
        {
            string layout = userRole switch
            {
                "Customer" => "~/Views/Shared/_Layout.cshtml",
                "Admin" => "~/Views/Shared/_LayoutAdmin.cshtml",
                _ => "~/Views/Shared/_LayoutStaff.cshtml",
            };
            return layout;
        }
    }
}
