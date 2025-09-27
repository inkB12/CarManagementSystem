namespace CarManagementSystem.WebMVC.Models
{
    public class MonthlyRevenueReportViewModel
    {
        public int Month { get; set; }
        public int Year { get; set; }

        public decimal TotalRevenue { get; set; }
        public int TotalOrders { get; set; }

        public List<OrderSummaryViewModel> Orders { get; set; } = new();
        public Dictionary<string, decimal> RevenueByPaymentMethod { get; set; } = new();
    }
}
