using CarManagementSystem.DataAccess;
using CarManagementSystem.Models;
using Microsoft.AspNetCore.Mvc;
using System.Diagnostics;

namespace CarManagementSystem.Controllers
{
    public class HomeController : Controller
    {
        private readonly CarManagementDbContext _context;

        public HomeController(CarManagementDbContext context)
        {
            _context = context;
        }

        public IActionResult Index()
        {
            var users = _context.Users.ToList();
            return View(users);
        }
    }
}
