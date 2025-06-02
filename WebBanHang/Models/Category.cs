using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace WebBanHang.Models
{
    public class Category
    {
        public int Id { get; set; }
        public string? Name { get; set; }
        public int DisplayOrder { get; set; }
    }
}
