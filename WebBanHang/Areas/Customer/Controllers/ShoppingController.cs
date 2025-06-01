using Microsoft.AspNetCore.Mvc;

namespace WebBanHang.Areas.Customer.Controllers
{
	[Area("Customer")]
	public class ShoppingController : Controller
	{
		public IActionResult Index()
		{
			return View();
		}
	}
}
