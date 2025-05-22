using Microsoft.AspNetCore.Mvc;
using System.Diagnostics;
using WebBanHang.Models;

namespace WebBanHang.Controllers
{
    public class HomeController : Controller
    {
        private readonly ILogger<HomeController> _logger;
		private readonly ApplicationDbContext _db;
		private readonly IWebHostEnvironment _hosting;

		public HomeController(ILogger<HomeController> logger, ApplicationDbContext db, IWebHostEnvironment hosting)
		{
			_logger = logger;
			_db = db;
			_hosting = hosting;
		}

		public IActionResult Index()
        {
            var productList = _db.Products.ToList();
            return View(productList);
        }

        public IActionResult Privacy()
        {
            return View();
        }

        [ResponseCache(Duration = 0, Location = ResponseCacheLocation.None, NoStore = true)]
        public IActionResult Error()
        {
            return View(new ErrorViewModel { RequestId = Activity.Current?.Id ?? HttpContext.TraceIdentifier });
        }
    }
}
