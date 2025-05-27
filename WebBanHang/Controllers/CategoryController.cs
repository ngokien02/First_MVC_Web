using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using WebBanHang.Models;
namespace WebBanHang.Controllers
{
	public class CategoryController : Controller
	{
		private readonly ApplicationDbContext _db;
		public CategoryController(ApplicationDbContext db)
		{
			_db = db;
		}
		//Hiển thị danh sách chủng loại
		public IActionResult Index()
		{
			var listCategory = _db.Categories.OrderBy(c => c.DisplayOrder).ToList();
			return PartialView("Index", listCategory);
		}
		// Xử lý thêm chủng loại mới
		[HttpPost]
		public IActionResult Add(Category category)
		{
			if (ModelState.IsValid) //kiem tra hop le
			{
				//thêm category vào table Categories
				_db.Categories.Add(category);
				_db.SaveChanges();
				TempData["success"] = "Thêm thể loại thành công!";
				return RedirectToAction("Index");
			}
			return View();
		}
		//Hiển thị form cập nhật chủng loại
		public IActionResult Update(int id)
		{
			var category = _db.Categories.Find(id);
			if (category == null)
			{
				return NotFound();
			}
			return View(category);
		}
		// Xử lý cập nhật chủng loại
		[HttpPost]
		public IActionResult Update(Category category)
		{
			if (ModelState.IsValid) //kiem tra hop le
			{
				//cập nhật category vào table Categories
				_db.Categories.Update(category);
				_db.SaveChanges();
				TempData["success"] = "Cập nhật thể loại thành công!";
				return RedirectToAction("Index");
			}
			return View();
		}
		// Xử lý xóa chủng loại
		public Boolean DeleteConfirm(int id)
		{
			var category = _db.Categories.Find(id);

			if (category == null)
			{
				return false;
			}

			if (_db.Products.Where(x => x.CategoryId == id).ToList().Count() > 0)
			{
				return false;
			};

			_db.Categories.Remove(category);
			_db.SaveChanges();

			return true;
		}
	}
}