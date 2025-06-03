using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using WebBanHang.Areas.Customer.Models;
using WebBanHang.Models;

namespace WebBanHang.Areas.Customer.Controllers
{
	[Area("Customer")]
	public class ShoppingController : Controller
	{
		private readonly ApplicationDbContext _db;
		private readonly IWebHostEnvironment _hosting;

		public ShoppingController(ApplicationDbContext db, IWebHostEnvironment hosting)
		{
			_db = db;
			_hosting = hosting;
		}
		public IActionResult Index()
		{
			var categoryList = _db.Categories
				.Select(c => new CategoryViewModel
				{
					Id = c.Id,
					Name = c.Name,
					ProductCount = _db.Products.Where(p => p.CategoryId == c.Id).Count()
				})
				.ToList();
			ViewBag.CategoryList = categoryList;

			var productByCatId = _db.Products
				.Where(p => p.CategoryId == 1)
				.OrderByDescending(p => p.Price)
				.ToList();

			return PartialView("index", productByCatId);
		}
		public IActionResult Category(int id)
		{
			var productByCatId = _db.Products
				.Where(p => p.CategoryId == id)
				.OrderByDescending(p => p.Price)
				.ToList();
			return PartialView("_ProductByCategoryIdPartial", productByCatId);
		}
	}
}
