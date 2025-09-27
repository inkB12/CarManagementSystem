using CarManagementSystem.Services.Interfaces;
using CarManagementSystem.WebMVC.Models;
using Microsoft.AspNetCore.Mvc;

namespace CarManagementSystem.WebMVC.Controllers
{
    public class HomeController : Controller
    {
        private readonly IElectricVehicleService _vehicleService;
        private readonly ICarCompanyService _companyService;
        private readonly IVehicleCategoryService _categoryService;
        private readonly IPromotionService _promotionService;
        private readonly IFeedbackService _feedbackService;

        public HomeController(
            IElectricVehicleService vehicleService,
            ICarCompanyService companyService,
            IVehicleCategoryService categoryService,
            IPromotionService promotionService,
            IFeedbackService feedbackService)
        {
            _vehicleService = vehicleService;
            _companyService = companyService;
            _categoryService = categoryService;
            _promotionService = promotionService;
            _feedbackService = feedbackService;
        }

        public async Task<IActionResult> Index()
        {
            // 6 xe nổi bật (2 hàng × 3 cột)
            var featured = (await _vehicleService.GetAllAsync())
                           .OrderBy(v => v.Price)
                           .Take(6)
                           .ToList();

            var vm = new HomePageViewModel
            {
                Companies = await _companyService.GetAllAsync(true),
                Categories = await _categoryService.GetAllAsync(true),
                FeaturedVehicles = (await _vehicleService.GetAllAsync()).Take(6).ToList(),
                ActivePromotions = (await _promotionService.GetAllAsync(true))
                            .Where(p => (p.Status ?? "").Equals("Active", StringComparison.OrdinalIgnoreCase))
                            .OrderByDescending(p => p.Id)
                            .ToList(),
            };


            // ----- Banner code-cứng của bạn -----
            vm.HeroImages = new()
            {
                "https://res.cloudinary.com/dratbz8bh/image/upload/v1758770414/VQ_DATBIKE181024_5426_bqtrnw.png",
                "https://res.cloudinary.com/dratbz8bh/image/upload/v1758770393/mgvskl-1680828328667953668266_jbciku.jpg",
                "https://res.cloudinary.com/dratbz8bh/image/upload/v1758770725/pngtree-visual-representation-of-an-electric-vehicle-being-charged-in-3d-picture-image_3759588_mszxqr.png"
            };
            vm.IntroTitle = "Đại lý xe điện – thông minh & thân thiện môi trường";  
            vm.IntroText = "Tư vấn chọn xe, hỗ trợ lái thử, bảo hành chính hãng và nhiều ưu đãi dành cho khách hàng mới.";
            vm.CategoryCards = new()
            {
                ("https://electrek.co/wp-content/uploads/sites/3/2024/11/Toyota-bZ7-EV-side.jpeg","Sedan EV","/ElectricVehicles?categoryId=1"),
                ("https://media.wired.com/photos/664e61613abb82db1c0e08e7/master/pass/kia-ev3-gtline-aventurinegreen-exterior-digital-1920x1080-001.jpg","SUV EV","/ElectricVehicles?categoryId=2"),
                ("https://apolloscooters.co/cdn/shop/files/IMGL0742.jpg?v=1738181231&width=1077","City Scooter","/ElectricVehicles?categoryId=3"),
                ("https://static-cms-prod.vinfastauto.us/2025-03/exterior-color-blue_0.webp","VinFast Car","/ElectricVehicles?categoryId=4"),
                ("https://vinfastotominhdao.vn/wp-content/uploads/evo-200-2022-1110x577.jpg","VinFast Moto","/ElectricVehicles?categoryId=5"),
                ("https://res.cloudinary.com/dratbz8bh/image/upload/v1758770730/pngtree-electric-vehicles-recharge-at-the-garage-charging-hub-3d-render-image_3708753_gvwmv6.jpg","Trạm sạc","/ElectricVehicles?categoryId=6"),
            };
            vm.News = new()
            {
                new NewsItemVM { Title="Ra mắt SUV EV mới 600km", Summary="Pin LFP, sạc nhanh 250kW.",
                                 ImageUrl="https://i.ytimg.com/vi/CsuuOHAHfE4/hq720.jpg", Url="/News/1", PublishedAt=DateTime.Today },
                new NewsItemVM { Title="Ưu đãi tháng này", Summary="Giảm 10–20% phí đăng ký.", ImageUrl="https://bqn.1cdn.vn/2025/09/07/vinfast-motio.jpg", Url="/News/2"},
                new NewsItemVM { Title="Trạm sạc Q.1", Summary="40 cổng sạc nhanh.", ImageUrl="https://cdn.tienphong.vn/images/814b5533c866dc3540018a126103e93589d7dd15db36187319cb37bf2a1c41cb1a19597ebcee0ea67d46501b9db8087ff55f4d4cf291e10c38303c5c973ba32b/1-1076-5012.jpg", Url="/News/3"},
                new NewsItemVM { Title="Bảo dưỡng pin", Summary="Giữ dung lượng tối ưu.", ImageUrl="https://greencharge.vn/wp-content/uploads/2023/05/bao-tri-pin-xe-dien-1.jpg", Url="/News/4"},
            };

            // 3 feedback mới nhất (qua service)
            var latest3 = (await _feedbackService.GetAllAsync())
                 .OrderByDescending(f => f.Datetime)
                 .Take(3)
                 .Select(f => new FeedbackCardVM
                 {
                     FeedbackType = f.FeedbackType,
                     Content = f.Content,
                     UserFullName = f.User?.FullName ?? $"User #{f.UserId}",
                     Datetime = f.Datetime
                 })
                 .ToList();
            ViewBag.RecentFeedbacks = latest3;
            return View(vm);
        }
    }
}
