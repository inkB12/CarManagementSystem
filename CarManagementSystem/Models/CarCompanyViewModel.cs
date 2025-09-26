using System.ComponentModel.DataAnnotations;

namespace CarManagementSystem.WebMVC.Models
{
    public class CarCompanyViewModel
    {
        public int Id { get; set; }

        [Required(ErrorMessage = "Tên hãng xe không được để trống")]
        [StringLength(50, ErrorMessage = "Tên hãng xe không được vượt quá 50 ký tự")]
        [Display(Name = "Tên hãng xe")]
        public string CatalogName { get; set; } = string.Empty;

        [StringLength(200, ErrorMessage = "Mô tả không được vượt quá 200 ký tự")]
        [Display(Name = "Mô tả")]
        public string? Description { get; set; }

        [Display(Name = "Hoạt động")]
        public bool IsActive { get; set; } = true;
    }
}
