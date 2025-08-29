using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace BanSach.Models
{
  [Table("ShoppingCart")]
  public class ShoppingCart
  {
    // mã định danh
    public int Id { get; set; }

    // mỗi khách hàng có một giỏ hàng riêng
    [Required]
    public string UserId { get; set; }

    public bool IsDeleted { get; set; } = false;

    public ICollection<CartDetail> CartDetails { get; set; }
  }
}
