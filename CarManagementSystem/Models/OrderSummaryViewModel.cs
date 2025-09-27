namespace CarManagementSystem.WebMVC.Models
{
    public class OrderSummaryViewModel
    {
        public int OrderId { get; set; }
        public string CustomerName { get; set; } = string.Empty;
        public DateTime Datetime { get; set; }
        public decimal Total { get; set; }
        public string PaymentMethod { get; set; } = string.Empty;
    }

}
