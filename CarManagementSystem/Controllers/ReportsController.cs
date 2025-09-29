using CarManagementSystem.Services.Interfaces;
using CarManagementSystem.WebMVC.Models;
using Microsoft.AspNetCore.Mvc;

namespace CarManagementSystem.WebMVC.Controllers
{
    public class ReportsController : Controller
    {
        private readonly IReportService _reportService;

        public ReportsController(IReportService reportService)
        {
            _reportService = reportService;
        }

        [HttpGet]
        public async Task<IActionResult> Index(int? month, int? year)
        {
            var m = month ?? DateTime.Now.Month;
            var y = year ?? DateTime.Now.Year;

            var dto = await _reportService.GetMonthlyRevenueAsync(m, y);

            var vm = new MonthlyRevenueReportViewModel
            {
                Month = dto.Month,
                Year = dto.Year,
                TotalRevenue = dto.TotalRevenue,
                TotalOrders = dto.TotalOrders,
                Orders = dto.Orders.Select(o => new OrderSummaryViewModel
                {
                    OrderId = o.OrderId,
                    CustomerName = o.CustomerName,
                    Datetime = o.Datetime,
                    Total = o.Total,
                    PaymentMethod = o.PaymentMethod
                }).ToList(),
                RevenueByPaymentMethod = dto.RevenueByPaymentMethod
            };

            ViewData["Title"] = "Báo cáo doanh thu tháng";
            return View(vm);
        }
    }
}
