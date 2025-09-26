// WebMVC/Models/ElectricVehicleListViewModel.cs
using CarManagementSystem.BusinessObjects;
using Microsoft.AspNetCore.Mvc.Rendering;
using System.Collections.Generic;

namespace CarManagementSystem.WebMVC.Models
{
    public class ElectricVehicleListViewModel
    {
        public List<ElectricVehicle> Vehicles { get; set; } = new();

        // Tìm kiếm + lọc
        public string? Q { get; set; }
        public int? CompanyId { get; set; }
        public int? CategoryId { get; set; }

        // Dữ liệu cho filter
        public List<SelectListItem> CompanyOptions { get; set; } = new();
        public List<SelectListItem> CategoryOptions { get; set; } = new();

        // Phân trang
        public int CurrentPage { get; set; } = 1;
        public int PageSize { get; set; } = 6;
        public int TotalPages { get; set; }
        public int TotalCount { get; set; }
    }
}
