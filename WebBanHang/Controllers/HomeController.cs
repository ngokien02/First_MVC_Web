using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Rendering;
using Microsoft.EntityFrameworkCore;
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
			ViewBag.ProductCount = _db.Products.Count();
            ViewData["Categories"] = _db.Categories.Select(x => new SelectListItem
            {
                Text = x.Name,
                Value = x.Id.ToString()
            }).ToList();
			return View();
        }
        [HttpGet]
        public IActionResult LoadMore(int skip = 0, int take = 3)
        {
            var products = _db.Products
                .Include(p => p.Category)
                .OrderByDescending(p => p.Price)
                .Skip(skip)
                .Take(take)
                .ToList();
            return PartialView("PartialProducts", products);
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
