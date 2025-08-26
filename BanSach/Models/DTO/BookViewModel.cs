namespace BanSach.Models.DTO
{
  public class BookViewModel
  {
    // chỗ này viết đơn giản thôi
    // nếu không sẽ dễ bị lỗi
    public IEnumerable<Book> Books { get; set; }
    public IEnumerable<Genre> Genres { get; set; }
    // từ khóa người dùng nhập vào để tìm kiếm trên trang web của chúng ta
    public string Term { get; set; } = "";
    // lọc theo id thể loại (lọc theo tên thể loại)
    public int GenreId { get; set; } = 0;
    // chúng ta có nên để dữ liệu khởi tạo mặc định trong này không
    // đã test và nó ok
  }
}
