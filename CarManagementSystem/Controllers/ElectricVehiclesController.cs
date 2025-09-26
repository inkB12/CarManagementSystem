using CarManagementSystem.DataAccess.Repositories.Interfaces;
using CarManagementSystem.Services.Interfaces;
using CarManagementSystem.WebMVC.Models;
using Microsoft.AspNetCore.Mvc;

namespace CarManagementSystem.WebMVC.Controllers
{
    public class ElectricVehiclesController : Controller
    {
        private readonly IElectricVehicleService _service;

        public ElectricVehiclesController(IElectricVehicleService service)
        {
            _service = service;
        }

        public async Task<IActionResult> Index(int page = 1, int pageSize = 9)
        {
            var all = await _service.GetAllAsync();
            var totalCount = all.Count;

            var items = all.Skip((page - 1) * pageSize).Take(pageSize).ToList();

            var vm = new ElectricVehicleListViewModel
            {
                Vehicles = items,
                CurrentPage = page,
                TotalPages = (int)Math.Ceiling(totalCount / (double)pageSize)
            };

            return View(vm);
        }

        public async Task<IActionResult> Details(int id)
        {
            var vehicle = await _service.GetByIdAsync(id);
            if (vehicle == null) return NotFound();

            return View(vehicle);
        }
    }
}
