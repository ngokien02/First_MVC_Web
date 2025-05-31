using Microsoft.AspNetCore.Identity;

namespace WebBanHang.Models
{
	public class Customer : IdentityUser
	{
		public string HoTen {  get; set; }
		public DateTime NgaySinh { get; set; }
	}
}
