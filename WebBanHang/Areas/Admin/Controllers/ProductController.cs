using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Rendering;
using Microsoft.EntityFrameworkCore;
using WebBanHang.Models;

namespace WebBanHang.Areas.Admin.Controllers
{
	[Area("Admin")]
	[Authorize(Roles = "Admin")]
	public class ProductController : Controller
	{
		private readonly ApplicationDbContext _db;
		private readonly IWebHostEnvironment _hosting;
		public ProductController(ApplicationDbContext db, IWebHostEnvironment hosting)
		{
			_db = db;
			_hosting = hosting;
		}
		public IActionResult Index(int page = 1)
		{
			int pageSize = 5;
			var totalProducts = _db.Products.Count();
			var productList = _db.Products
				.Include(x => x.Category)
				.OrderByDescending(x => x.Id)
				.Skip((page - 1) * pageSize)
				.Take(pageSize)
				.ToList();
			ViewBag.TotalPages = (int)Math.Ceiling((double)totalProducts / pageSize);
			ViewBag.CurrentPage = page;
			return PartialView("Index", productList);
		}

		[HttpPost]
		public bool Add(Product p)
		{
			if (ModelState.IsValid)
			{
				if (p.ImgFile != null)
				{
					p.ImageUrl = SaveImage(p.ImgFile);
				}
				_db.Products.Add(p);
				_db.SaveChanges();

				return true;

			}
			ViewBag.CategoryList = _db.Categories.Select(x => new SelectListItem
			{
				Value = x.Id.ToString(),
				Text = x.Name
			}).ToList();
			return false;
		}
		private string SaveImage(IFormFile img)
		{
			var fileName = Guid.NewGuid().ToString() + Path.GetExtension(img.FileName);
			var path = Path.Combine(_hosting.WebRootPath, @"images/products");
			var saveFile = Path.Combine(path, fileName);
			using (var filestream = new FileStream(saveFile, FileMode.Create))
			{
				img.CopyTo(filestream);
			}
			return @"images/products/" + fileName;
		}

		[HttpGet]
		public IActionResult Update(int id)
		{
			var product = _db.Products.FirstOrDefault(x => x.Id == id);
			ViewBag.CategoriesEdit = _db.Categories.Select(c => new SelectListItem
			{
				Text = c.Name,
				Value = c.Id.ToString()
			});

			if (product != null)
			{
				return PartialView("_UpdateProductModal", product);
			}


			else return NotFound();
		}

		[HttpPost, ActionName("Update")]
		public bool Update(Product product)
		{
			if (ModelState.IsValid) //kiem tra hop le
			{
				var existingProduct = _db.Products.Find(product.Id);
				if (product.ImgFile != null)
				{
					//xu ly upload và lưu ảnh đại diện mới
					product.ImageUrl = SaveImage(product.ImgFile);
					//xóa ảnh cũ (nếu có)
					if (!string.IsNullOrEmpty(existingProduct.ImageUrl))
					{
						var oldFilePath = Path.Combine(_hosting.WebRootPath, existingProduct.ImageUrl);
						if (System.IO.File.Exists(oldFilePath))
						{
							System.IO.File.Delete(oldFilePath);
						}
					}
				}
				else
				{
					product.ImageUrl = existingProduct.ImageUrl;
				}
				//cập nhật product vào table Product
				existingProduct.Name = product.Name;
				existingProduct.Description = product.Description;
				existingProduct.Price = product.Price;
				existingProduct.CategoryId = product.CategoryId;
				existingProduct.ImageUrl = product.ImageUrl;
				_db.SaveChanges();
				TempData["success"] = "Cập nhật sản phẩm thành công!";
				return true;
			}
			ViewBag.CategoriesEdit = _db.Categories.Select(x => new SelectListItem
			{
				Value = x.Id.ToString(),
				Text = x.Name
			});
			return false;
		}

		[HttpPost]
		public bool DeleteConfirm(int id)
		{
			var product = _db.Products.Find(id);
			if (product == null)
			{
				return false;
			}
			// xoá hình cũ của sản phẩm
			if (!string.IsNullOrEmpty(product.ImageUrl))
			{
				var oldFilePath = Path.Combine(_hosting.WebRootPath, product.ImageUrl);
				if (System.IO.File.Exists(oldFilePath))
				{
					System.IO.File.Delete(oldFilePath);
				}
			}
			// xoa san pham khoi CSDL
			_db.Products.Remove(product);
			_db.SaveChanges();
			//chuyen den action index
			return true;
		}
	}
}
