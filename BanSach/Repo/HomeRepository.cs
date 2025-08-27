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
      // Cách 1: Query Syntax
      // chuyển từ chữ hoa thành chữ thường
      // chuyển nguyên một chuỗi nhập vào thành chữ thường
      // term = term.ToLower();

      // đoạn này sẽ có vấn đề về hiệu năng
      // dữ liệu càng nhiều thì truy vấn càng chậm
      // nếu có 1 triệu cuốn sách thì sẽ rất chậm
      // giống bài toán 1 triệu công tơ điện (viết trang quản lý công tơ cho công ty)
      // nên chuyển sang dùng biểu thức lambda
      // chúng ta cần nối nhiều bảng
      // bảng book, bảng stock (kho hàng)
      //
      //IEnumerable<Book> books = await (from book in _db.Books
      //             join genre in _db.Genres
      //             on book.GenreId equals genre.Id
      //             where string.IsNullOrWhiteSpace(term) || (book != null && book.BookName.ToLower().StartsWith(term))
      //             select new Book
      //             {
      //               Id = book.Id,
      //               Image = book.Image,
      //               AuthorName = book.AuthorName,
      //               BookName = book.BookName,
      //               GenreId = book.GenreId,
      //               Price = book.Price,
      //               GenreName = genre.GenreName
      //             }
      //             ).ToListAsync();
      // bây giờ chúng ta đã fix được lỗi
      // muốn được các thông tin ra màn hình thì phải
      // mapping/binding cho đầy đủ (danh sách binding ở trên nha)

      //if (genreId > 0)
      //{
      //  books = books.Where(a => a.GenreId == genreId).ToList();
      //}

      // Cách 2: Method Syntax
      // code do chatgpt generate
      //var books = await _db.Books
      //        .Join(_db.Genres,
      //              book => book.GenreId,
      //              genre => genre.Id,
      //              (book, genre) => new { book, genre })
      //        .Where(x => string.IsNullOrWhiteSpace(term) || x.book.BookName.ToLower().StartsWith(term))
      //        .Select(x => new Book
      //        {
      //          Id = x.book.Id,
      //          Image = x.book.Image,
      //          AuthorName = x.book.AuthorName,
      //          BookName = x.book.BookName,
      //          GenreId = x.book.GenreId,
      //          Price = x.book.Price,
      //          GenreName = x.genre.GenreName
      //        })
      //        .ToListAsync();

      //if (genreId > 0)
      //{
      //  books = books.Where(a => a.GenreId == genreId).ToList();
      //}

      // Cách 3: Method Syntax
      var bookQuery = _db.Books
        .AsNoTracking()
        .Include(x => x.Genre)
        .AsQueryable();

      if (!string.IsNullOrWhiteSpace(term))
      {
        bookQuery = bookQuery.Where(b => b.BookName.StartsWith(term.ToLower()));
      }

      if (genreId > 0)
      {
        bookQuery = bookQuery.Where(b => b.GenreId == genreId);
      }

      // binding/mapping
      var books = await bookQuery
          .AsNoTracking()
          .Select(book => new Book {
            Id = book.Id,
            Image = book.Image,
            AuthorName = book.AuthorName,
            BookName = book.BookName,
            GenreId = book.GenreId,
            Price = book.Price,
            GenreName = book.Genre.GenreName
          })
          .ToListAsync();

      // Cách 4: Method Syntax
      // kết hợp thêm bảng Stock (tính số lượng tồn kho)
      // bài tập về nhà

      return books;

      // ở đây mình đang giải thích code nên viết comment lung tung
      // trong thực tế việc comment cũng có quy định chung
      // tức là comment convention
    }
  }
}
