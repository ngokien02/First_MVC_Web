using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Diagnostics.CodeAnalysis;

namespace WebBanHang.Models
{
    public class Product
    {
        public int Id { get; set; }
        [Required(ErrorMessage ="chua nhap ten"), StringLength(200)]
        public string? Name { get; set; }
        public string? Description { get; set; }
        [Range(0, 10000000000)]
        public double Price { get; set; }
        [Required(ErrorMessage ="Chua chon loai")]
        public int CategoryId { get; set; }
        //khai báo mối kết hợp 1-n
        [ForeignKey("CategoryId")]
        public virtual Category? Category { set; get; } //khai báo mối kết hợp 1 - nhiều
        public string? ImageUrl { get; set; }
        [NotMapped]
        public IFormFile? ImgFile { get; set; }
    }
}
