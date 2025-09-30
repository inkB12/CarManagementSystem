using System.ComponentModel.DataAnnotations;
using Microsoft.AspNetCore.Http;

namespace CarManagementSystem.WebMVC.Models
{
    public class VehicleViewModel
    {
        public int Id { get; set; }

        [Required(ErrorMessage = "Tên mẫu xe không được để trống")]
        [StringLength(50, ErrorMessage = "Tên mẫu xe không được vượt quá 50 ký tự")]
        [Display(Name = "Mẫu xe")]
        public string Model { get; set; } = string.Empty;

        [StringLength(50, ErrorMessage = "Tên phiên bản không được vượt quá 50 ký tự")]
        [Display(Name = "Phiên bản")]
        public string? Version { get; set; }

        [Required(ErrorMessage = "Giá không được để trống")]
        [Range(1, double.MaxValue, ErrorMessage = "Giá phải lớn hơn 0")]
        [Display(Name = "Giá bán")]
        [RegularExpression(@"^\d+(\.\d{1,2})?$", ErrorMessage = "Giá phải là số hợp lệ")]
        public decimal Price { get; set; }


        [StringLength(200, ErrorMessage = "Tên thông số kỹ thuật không được vượt quá 200 ký tự")]
        [Display(Name = "Thông số kỹ thuật")]
        public string? Specification { get; set; }


        [StringLength(50, ErrorMessage = "Tên màu sắc không được vượt quá 50 ký tự")]
        [Display(Name = "Màu sắc")]
        public string? Color { get; set; }

        [Display(Name = "Ảnh xe")]
        public string? ImageUrl { get; set; }   // link Cloudinary sẽ lưu vào đây
        public IFormFile? ImageFile { get; set; } // dùng khi upload ảnh mới

        [Required(ErrorMessage = "Bạn phải chọn hãng xe")]
        [Display(Name = "Hãng xe")]
        public int CarCompanyId { get; set; }
        public string? CarCompanyName { get; set; } // hiển thị ở Index

        [Required(ErrorMessage = "Bạn phải chọn danh mục xe")]
        [Display(Name = "Danh mục xe")]
        public int VehicleCategoryId { get; set; }
        public string? VehicleCategoryName { get; set; } // hiển thị ở Index

        [Display(Name = "Hoạt động")]
        public bool IsActive { get; set; }
    }
}
