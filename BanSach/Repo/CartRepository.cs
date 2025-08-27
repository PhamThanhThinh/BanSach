using BanSach.Data;
using BanSach.Models;
using Microsoft.AspNetCore.Identity;
using System.Security.Claims;

namespace BanSach.Repo
{
  public class CartRepository /*: ICartRepository*/
  {
    // code hàm dựng
    private readonly ApplicationDbContext _db;
    private readonly UserManager<IdentityUser> _userManager;
    private readonly IHttpContextAccessor _httpContextAccessor;

    public CartRepository(ApplicationDbContext db, UserManager<IdentityUser> userManager, IHttpContextAccessor httpContextAccessor)
    {
      _db = db;
      _userManager = userManager;
      _httpContextAccessor = httpContextAccessor;
    }

    // Cách 1: cơ bản (viết nháp, test chức năng)
    public void AddItem(int bookId, int quantity)
    {
      // dữ liệu rỗng/trống khác với null nha
      string userId = GetUserId();

      try
      {
        if (string.IsNullOrEmpty(userId))
        {
          // User chưa đăng nhập
          //throw new Exception("User chưa đăng nhập");
          throw new UnauthorizedAccessException("User chưa đăng nhập");
        }
      }
      catch (Exception ex)
      {

      }

      var cart = GetCart(userId);
    }
    // tưởng mô cách này dễ
    //private bool HasCart(string userId)
    //{
    //  var ketQua = _db.ShoppingCarts.FirstOrDefault(x => x.UserId == userId);
    //  // toán tử 3 ngôi (lập trình C/C++)
    //  return ketQua != null ? true : false;
    //}
    // viết code dễ đọc hơn
    private ShoppingCart GetCart(string userId)
    {
      var gioHang = _db.ShoppingCarts.FirstOrDefault(x => x.UserId == userId);
      return gioHang;
    }
    // chúng ta nên trả về boolean hay string???
    private string GetUserId()
    {
      var httpContext = _httpContextAccessor.HttpContext;

      if (httpContext?.User?.Identity != null && httpContext.User.Identity.IsAuthenticated)
      {
        // Lấy UserId từ claim
        return httpContext.User.FindFirst(ClaimTypes.NameIdentifier)?.Value;
      }

      return null; // User chưa đăng nhập
    }

    // Cách 2: nâng cao
    //public Task<int> AddItem(int bookId, int quantity)
    //{
    //  throw new NotImplementedException();
    //}

    //public Task<ShoppingCart> GetCart(string userId)
    //{
    //  throw new NotImplementedException();
    //}

    //public Task<int> RemoveItem(int bookId)
    //{
    //  throw new NotImplementedException();
    //}
  }
}
