using CarManagementSystem.DataAccess;

namespace CarManagementSystem.WebMVC.Models
{
    public class ElectricVehicleListViewModel
    {
        public List<ElectricVehicle> Vehicles { get; set; } = new();
        public int CurrentPage { get; set; }
        public int TotalPages { get; set; }
    }
}
