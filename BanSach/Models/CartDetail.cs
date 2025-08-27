using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace BanSach.Models
{
  [Table("CartDetail")]
  public class CartDetail
  {
    public int Id { get; set; }

    [Required]
    public int ShoppingCartId { get; set; }
    public ShoppingCart ShoppingCart { get; set; }

    [Required]
    public int BookId { get; set; }
    public Book Book { get; set; }

    // số lượng của một cuốn
    [Required]
    public int Quantity { get; set; }

    // giá của món hàng
    public double UnitPrice { get; set; }

  }
}
