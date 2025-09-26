using CarManagementSystem.DataAccess;
using CarManagementSystem.WebMVC.Models; // đổi theo nơi bạn đặt HomePageVM
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using System.Linq;
using System.Threading.Tasks;

namespace CarManagementSystem.WebMVC.Controllers // sửa theo namespace WebMVC của bạn
{
    public class HomeController : Controller
    {
        private readonly CarManagementDbContext _context;
        public HomeController(CarManagementDbContext context) => _context = context;

        public async Task<IActionResult> Index()
        {
            var vm = new HomePageViewModel
            {
                Companies = await _context.CarCompanies
                                          .Where(c => c.IsActive == true)
                                          .AsNoTracking().ToListAsync(),

                Categories = await _context.VehicleCategories
                                           .Where(c => c.IsActive == true)
                                           .AsNoTracking().ToListAsync(),

                FeaturedVehicles = await _context.ElectricVehicles
                                                 .Where(v => v.IsActive == true)
                                                 .Include(v => v.CarCompany)
                                                 .Include(v => v.VehicleCategory)
                                                 .OrderBy(v => v.Price).Take(8)
                                                 .AsNoTracking().ToListAsync(),

                ActivePromotions = await _context.Promotions
                                                 .Where(p => p.Status == "Active")
                                                 .OrderByDescending(p => p.Id)
                                                 .AsNoTracking().ToListAsync()

  

            }; vm.HeroImages = new()
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
                                 ImageUrl="https://i.ytimg.com/vi/CsuuOHAHfE4/hq720.jpg?sqp=-oaymwEhCK4FEIIDSFryq4qpAxMIARUAAAAAGAElAADIQj0AgKJD&rs=AOn4CLDpqdDp9Iy274euGFnB36XmTaYAnQ", Url="/News/1",
                                 PublishedAt=DateTime.Today },
                new NewsItemVM { Title="Ưu đãi tháng này", Summary="Giảm 10–20% phí đăng ký.",
                                 ImageUrl="https://bqn.1cdn.vn/2025/09/07/vinfast-motio.jpg", Url="/News/2"},
                new NewsItemVM { Title="Trạm sạc Q.1", Summary="40 cổng sạc nhanh.",
                                 ImageUrl="https://cdn.tienphong.vn/images/a6bf4f60924201126af6849ca45a398089d7dd15db36187319cb37bf2a1c41cb5d251b9e7fb1cadd34a93368d8ec5b9ff55f4d4cf291e10c38303c5c973ba32b/5-7667-3767.jpg", Url="/News/3"},
                new NewsItemVM { Title="Bảo dưỡng pin", Summary="Giữ dung lượng tối ưu.",
                                 ImageUrl="https://greencharge.vn/wp-content/uploads/2023/05/bao-tri-pin-xe-dien-1.jpg", Url="/News/4"},
            };
            // lấy 3 feedback mới nhất
            var recentFeedbacks = await _context.Feedbacks
                .Include(f => f.User)
                .OrderByDescending(f => f.Datetime)
                .Take(3)
                .ToListAsync();

            ViewBag.RecentFeedbacks = recentFeedbacks;


            return View(vm);
        }
    }
}
