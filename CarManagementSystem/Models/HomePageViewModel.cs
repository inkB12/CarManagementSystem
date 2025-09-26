using CarManagementSystem.BusinessObjects;

namespace CarManagementSystem.WebMVC.Models
{
    public class NewsItemVM
    {
        public string Title { get; set; } = "";
        public string Summary { get; set; } = "";
        public string? ImageUrl { get; set; }
        public string? Url { get; set; }
        public DateTime? PublishedAt { get; set; }
    }
    public class HomePageViewModel
    {
        public List<CarCompany> Companies { get; set; } = new ();
        public List<VehicleCategory> Categories { get; set; } = new();
        public List<ElectricVehicle> FeaturedVehicles { get; set; } = new();
        public List<Promotion> ActivePromotions { get; set; } = new();
        // === Các banner CODE CỨNG (không gọi DB) ===
        public List<string> HeroImages { get; set; } = new();                       // 3 ảnh carousel
        public string? IntroTitle { get; set; }
        public string? IntroText { get; set; }
        public List<(string Img, string? Label, string? Link)> CategoryCards { get; set; } = new(); // tối đa 6
        public List<NewsItemVM> News { get; set; } = new();
    }
}
