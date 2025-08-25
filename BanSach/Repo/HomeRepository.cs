using BanSach.Data;

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

    }
  }
}
