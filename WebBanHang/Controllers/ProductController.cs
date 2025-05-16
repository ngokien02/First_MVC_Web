using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Rendering;
using Microsoft.EntityFrameworkCore;
using WebBanHang.Models;
using static System.Net.Mime.MediaTypeNames;

namespace WebBanHang.Controllers
{
    public class ProductController : Controller
    {
        private readonly ApplicationDbContext _db;
        private readonly IWebHostEnvironment _hosting;
        public ProductController(ApplicationDbContext db, IWebHostEnvironment hosting)
        {
            _db = db;
            _hosting = hosting;
        }
        public IActionResult Index()
        {
            var productList = _db.Products.Include(x => x.Category).ToList();
            return View(productList);
        }
        public IActionResult Add()
        {
            ViewBag.CategoryList = _db.Categories.Select(x => new SelectListItem
            {
                Value = x.Id.ToString(),
                Text = x.Name
            });
            return View();
        }
        [HttpPost]
        public IActionResult Add(Product p, IFormFile ImageUrl)
        {
            if (ModelState.IsValid)
            {
                if (ImageUrl != null)
                {
                    p.ImageUrl = SaveImage(ImageUrl);
                }

                _db.Products.Add(p);
                _db.SaveChanges();
                TempData["success"] = "Thêm sản phẩm thành công";

                return RedirectToAction("Index");

            }
			ViewBag.CategoryList = _db.Categories.Select(x => new SelectListItem
			{
				Value = x.Id.ToString(),
				Text = x.Name
			});
			return View();
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
    }
}
