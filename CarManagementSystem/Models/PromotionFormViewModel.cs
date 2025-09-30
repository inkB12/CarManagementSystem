// WebMVC/Models/PromotionFormViewModel.cs
using System.ComponentModel.DataAnnotations;

namespace CarManagementSystem.WebMVC.Models
{
    public class PromotionFormViewModel
    {
        public int Id { get; set; }

        [Required(ErrorMessage = "Mã khuyến mãi không được để trống")]
        [StringLength(50, ErrorMessage = "Mã khuyến mãi tối đa 50 ký tự")]
        public string Code { get; set; } = "";

        [StringLength(200, ErrorMessage = "Mô tả tối đa 200 ký tự")]
        public string? Description { get; set; }

        [Range(0.1, 100, ErrorMessage = "Giảm giá phải từ 0.1% đến 100%")]
        public decimal Discount { get; set; }

        [Required(ErrorMessage = "Trạng thái là bắt buộc")]
        [RegularExpression("Active|Inactive", ErrorMessage = "Trạng thái chỉ nhận Active hoặc Inactive")]
        public string Status { get; set; } = "Active";
    }
}
