﻿using Microsoft.AspNetCore.Mvc;
using WebBanHang.Areas.Customer.Models;
using WebBanHang.Models;

namespace WebBanHang.Areas.Customer.Controllers
{
	[Area("Customer")]
	public class CartController : Controller
	{
		private readonly ApplicationDbContext _db;
		private readonly IWebHostEnvironment _hosting;

		public CartController(ApplicationDbContext db, IWebHostEnvironment hosting)
		{
			_db = db;
			_hosting = hosting;
		}
		//action : hiển thị giao diện quản lý giỏ hàng
		public IActionResult Index()
		{
			Cart cart = HttpContext.Session.GetJson<Cart>("CART");

			if (cart == null)
			{
				cart = new Cart();
			}
			return View(cart);
		}
		//action: xử lý thêm sản phẩm vào giỏ hàng
		public IActionResult AddToCart(int productId)
		{
			var product = _db.Products.FirstOrDefault(x => x.Id == productId);
			if (product != null)
			{
				Cart cart = HttpContext.Session.GetJson<Cart>("CART");
				if (cart == null)
				{
					cart = new Cart();
				}
				cart.Add(product, 1);
				HttpContext.Session.SetJson("CART", cart);
				// return Json(new { msg="success", qty = cart.Quantity});
				return RedirectToAction("Index");
			}
			return Json(new { msg = "error" });
		}
		//action: xử lý cập nhật số lượng
		public IActionResult Update(int productId, int qty)
		{
			var product = _db.Products.FirstOrDefault(x => x.Id == productId);
			if (product != null)
			{
				//lấy cart từ Session
				Cart cart = HttpContext.Session.GetJson<Cart>("CART");
				if (cart != null)
				{
					cart.Update(productId, qty); //cập nhật lại số lượng
					HttpContext.Session.SetJson("CART", cart); //lưu cart vào Session
					return RedirectToAction("Index");
				}
			}

			return Json(new { msg = "error" });
		}
		//action: xử lý xoá sản phẩm trong giỏ hàng
		public IActionResult Remove(int productId)
		{
			var product = _db.Products.FirstOrDefault(x => x.Id == productId);
			if (product != null)
			{
				//lấy cart từ Session
				Cart cart = HttpContext.Session.GetJson<Cart>("CART");
				if (cart != null)
				{
					cart.Remove(productId); //xoá sản phẩm khỏi giỏ
					HttpContext.Session.SetJson("CART", cart); //lưu cart vào Session
					return RedirectToAction("Index");
				}
			}
			return NotFound();
		}
		public IActionResult AddToCartAPI(int productId)
		{
			var product = _db.Products.FirstOrDefault(x => x.Id == productId);
			if (product != null)
			{
				Cart cart = HttpContext.Session.GetJson<Cart>("CART");
				if (cart == null)
				{
					cart = new Cart();
				}
				cart.Add(product, 1);
				HttpContext.Session.SetJson("CART", cart);
				return Json(new { msg = "Product added to cart", qty = cart.Quantity });
			}
			return Json(new { msg = "error" });
		}
		public IActionResult GetQuantityOfCart()
		{
			Cart cart = HttpContext.Session.GetJson<Cart>("CART");
			if (cart != null)
			{

				return Json(new { qty = cart.Quantity });
			}
			return Json(new { qty = 0 });
		}
	}
}
