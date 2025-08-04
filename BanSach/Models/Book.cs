using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace BanSach.Models
{
  [Table("Book")]
  public class Book
  {
    public int Id { get; set; }

    [Required]
    [MaxLength(50)]
    public string? BookName { get; set; }

    public string? Image { get; set; }

    [Required]
    public int GenreId { get; set; }

    public Genre Genre { get; set; }

  }
}
