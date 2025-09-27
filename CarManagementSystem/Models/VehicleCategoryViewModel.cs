using System.ComponentModel.DataAnnotations;

namespace CarManagementSystem.WebMVC.Models
{
    public class VehicleCategoryViewModel
    {
        public int Id { get; set; }

        [Required(ErrorMessage = "Tên danh mục không được để trống")]
        [StringLength(50, ErrorMessage = "Tên danh mục không được vượt quá 50 ký tự")]
        [Display(Name = "Tên danh mục")]
        public string CategoryName { get; set; } = string.Empty;

        [Display(Name = "Hoạt động")]
        public bool IsActive { get; set; }
    }
}
