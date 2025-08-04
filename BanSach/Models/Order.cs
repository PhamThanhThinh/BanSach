using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace BanSach.Models
{
  [Table("Order")]
  public class Order
  {
    // mã định danh order
    public int Id { get; set; }

    // ai là người đã order
    [Required]
    public string UserId { get; set; }

    public DateTime CreatedDate { get; set; } = DateTime.UtcNow;

    [Required]
    public int OrderStatusId { get; set; }

    public OrderStatus OrderStatus { get; set; }

    public bool IsDeleted { get; set; } = false;

    public List<OrderDetail> OrderDetail { get; set; }

  }
}
