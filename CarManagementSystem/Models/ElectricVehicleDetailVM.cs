namespace CarManagementSystem.WebMVC.Models
{
    public class ElectricVehicleDetailVM
    {
        internal string? Specification;

        public int Id { get; set; }
        public string Model { get; set; }
        public string? Version { get; set; }
        public decimal Price { get; set; }
        public string? ImageUrl { get; set; }
        public string? Color { get; set; }
        public string CompanyName { get; set; }
        public string CategoryName { get; set; }
        public string? Specifications { get; set; }
        //Hiển thị spec theo dòng/bullet

        public List<string>? SpecLines { get; set; }
    }
}
