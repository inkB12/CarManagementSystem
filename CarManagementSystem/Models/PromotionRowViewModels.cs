namespace CarManagementSystem.WebMVC.Models
{
    public class PromotionRowViewModels
    {
        public int Id { get; set; }
        public string Code { get; set; } = "";
        public string? Description { get; set; }
        public decimal Discount { get; set; }   // 0..100
        public string Status { get; set; } = "Active";
    }
}
