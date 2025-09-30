using CarManagementSystem.DataAccess.Repositories.Interfaces;
using CarManagementSystem.Services.Dtos;
using CarManagementSystem.Services.Interfaces;

namespace CarManagementSystem.Services.Implements
{
    public class ReportService : IReportService
    {
        private readonly IOrderRepository _orderRepo;
        private const string Completed = "Completed"; // Chuẩn hoá trạng thái đơn hoàn tất

        public ReportService(IOrderRepository orderRepo)
        {
            _orderRepo = orderRepo;
        }

        public async Task<MonthlyRevenueReportDto> GetMonthlyRevenueAsync(int month, int year)
        {
            // Dùng khoảng ngày để DB dùng index tốt hơn
            var start = new DateTime(year, month, 1);
            var end = start.AddMonths(1);

            var orders = await _orderRepo.GetOrdersInRangeByStatusAsync(start, end, "SUCCESS");

            var dto = new MonthlyRevenueReportDto
            {
                Month = month,
                Year = year,
                TotalRevenue = orders.Sum(o => o.Total),
                TotalOrders = orders.Count,
                Orders = orders.Select(o => new OrderSummaryDto
                {
                    OrderId = o.Id,
                    CustomerName = o.User.FullName,
                    Datetime = o.Datetime,
                    Total = o.Total,
                    PaymentMethod = o.PaymentMethod
                }).ToList(),
                RevenueByPaymentMethod = orders
                    .GroupBy(o => o.PaymentMethod)
                    .ToDictionary(g => g.Key, g => g.Sum(x => x.Total))
            };

            return dto;
        }
    }
}
