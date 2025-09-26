namespace CarManagementSystem.WebMVC.Models.Cart
{
    public class CartItem
    {
        public int CarId { get; set; }
        public string? CarName { get; set; }
        public decimal Price { get; set; }
        public string? ImageUrl { get; set; }
        public int Quantity { get; set; }
    }
}
