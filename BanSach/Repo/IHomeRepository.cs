using BanSach.Models;

namespace BanSach.Repo
{
  public interface IHomeRepository
  {
    Task<IEnumerable<Book>> GetBooks(string term = "", int genreId = 0);
    Task<IEnumerable<Genre>> Genres();
  }
}
