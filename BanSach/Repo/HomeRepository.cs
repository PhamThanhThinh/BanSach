using BanSach.Data;
using BanSach.Models;
using Microsoft.EntityFrameworkCore;

namespace BanSach.Repo
{
  // triển khai interface
  // HomeRepository : IHomeRepository không gọi là kế thừa đâu nha
  // mà gọi là triển khai interface
  public class HomeRepository : IHomeRepository
  {
    private readonly ApplicationDbContext _db;

    public HomeRepository(ApplicationDbContext db)
    {
      _db = db;
    }

    public async Task<IEnumerable<Genre>> Genres()
    {
      return await _db.Genres.ToListAsync();
    }

    // quy ước giá trị mặc định
    // term = "" (trống/rỗng khác null nha)
    // categoryId = 0 (số 0: khi id không load được thì mặc định là 0 hoặc là số âm, nhưng thường là số 0, tức là id = 0)
    public async Task<IEnumerable<Book>> GetBooks(string term = "", int genreId = 0)
    {
      // chuyển từ chữ hoa thành chữ thường
      // chuyển nguyên một chuỗi nhập vào thành chữ thường
      term = term.ToLower();

      // đoạn này sẽ có vấn đề về hiệu năng
      // dữ liệu càng nhiều thì truy vấn càng chậm
      // nếu có 1 triệu cuốn sách thì sẽ rất chậm
      // giống bài toán 1 triệu công tơ điện
      // nên chuyển sang dùng biểu thức lambda
      // chúng ta cần nối nhiều bảng
      // bảng book, bảng stock (kho hàng)
      IEnumerable<Book> books = await (from book in _db.Books
                   join genre in _db.Genres
                   on book.GenreId equals genre.Id
                   where string.IsNullOrWhiteSpace(term) || (book != null && book.BookName.ToLower().StartsWith(term))
                   select new Book
                   {
                     Id = book.Id,
                     Image = book.Image,
                     AuthorName = book.AuthorName,
                     BookName = book.BookName,
                     GenreId = book.GenreId,
                     Price = book.Price,
                     GenreName = genre.GenreName
                   }
                   ).ToListAsync();
      // bây giờ chúng ta đã fix được lỗi
      // muốn được các thông tin ra màn hình thì phải
      // mapping/binding cho đầy đủ (danh sách binding ở trên nha)

      if (genreId > 0)
      {
        books = books.Where(a => a.GenreId == genreId).ToList();
      }

      return books;

      // ở đây mình đang giải thích code nên viết comment lung tung
      // trong thực tế việc comment cũng có quy định chung
      // tức là comment convention
    }
  }
}
