using BanSach.Data;
using BanSach.Models;

namespace BanSach.Repo
{
  public class HomeRepository
  {
    private readonly ApplicationDbContext _db;

    public HomeRepository(ApplicationDbContext db)
    {
      _db = db;
    }

    // quy ước giá trị mặc định
    // term = ""
    // categoryId = 0
    public void DisplayBooks(string term = "", int categoryId = 0)
    {
      var books = (from book in _db.Books
                   join genre in _db.Genres
                   on book.GenreId equals genre.Id
                   select new Book
                   {
                     Id = book.Id,
                     Image = book.Image,
                     AuthorName = book.AuthorName,
                     BookName = book.BookName,
                     Genre = book.Genre
                   }
                   );
    }
  }
}
