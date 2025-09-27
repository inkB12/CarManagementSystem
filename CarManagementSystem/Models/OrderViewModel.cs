using CarManagementSystem.BusinessObjects;

namespace CarManagementSystem.WebMVC.Models
{
    public class OrderViewModel
    {
        public int Id { get; set; }

        public DateTime Datetime { get; set; }

        public decimal Total { get; set; }

        public string PaymentMethod { get; set; } = null!;

        public string Status { get; set; } = null!;

        public string Address { get; set; } = null!;

        public string ZipCode { get; set; } = null!;

        public string? Note { get; set; }

        public List<OrderDetail> OrderDetails { get; set; } = [];

        public Promotion? Promotion { get; set; }

        public User User { get; set; } = null!;
    }
}

