using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
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
				.GroupJoin(_db.Products,
				c => c.Id,
				p => p.CategoryId,
				(c, products) => new
				{
					CategoryId = c.Id,
					CategoryName = c.Name,
					c.DisplayOrder,
					ProductCount = products.Count()
				})
				.OrderBy(c => c.DisplayOrder)
				.ToList();
			ViewBag.CategoryList = categoryList;
			var productByCatId = _db.Products
				.Where(p => p.CategoryId == 1)
				.OrderByDescending(p => p.Price)
				.ToList();
			return View("index", productByCatId);
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
