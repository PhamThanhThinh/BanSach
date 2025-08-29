using BanSach.Data;
using BanSach.Models;
using Microsoft.AspNetCore.Identity;
using Microsoft.EntityFrameworkCore;
using System.Security.Claims;
using System.Threading.Tasks;

namespace BanSach.Repo
{
  public class CartRepository : ICartRepository
  {
    // code hàm dựng constructor
    private readonly ApplicationDbContext _db;
    private readonly UserManager<IdentityUser> _userManager;
    private readonly IHttpContextAccessor _httpContextAccessor;

    public CartRepository(ApplicationDbContext db, UserManager<IdentityUser> userManager, IHttpContextAccessor httpContextAccessor)
    {
      _db = db;
      _userManager = userManager;
      _httpContextAccessor = httpContextAccessor;
    }

    // thêm một sản phẩm vào giỏ hàng
    // Cách 1: cơ bản (viết nháp, test chức năng, không dùng cơ chế đồng bộ)
    // còn cách 2 thì dùng cơ chế bất đồng bộ vào (async await), cộng với việc duyệt qua tất cả các dòng dữ liệu
    public async Task<int> AddItem(int bookId, int quantity)
    {
      // dữ liệu rỗng/trống khác với null nha

      // userId sẽ nhận giá trị: null hoặc UserId
      // người dùng đã đăng nhập trả về UserId
      // người dùng chưa đăng nhập trả về null
      string userId = GetUserId();

      try
      {
        if (string.IsNullOrEmpty(userId))
        {
          // User chưa đăng nhập
          //throw new Exception("User chưa đăng nhập");
          throw new UnauthorizedAccessException("User chưa đăng nhập");
        }
        var cart = await GetCart(userId);
        if (cart is null)
        {
          // thì thêm giỏ hàng cho user đó
          // user đó chưa có giỏ hàng thì thêm cho nó một giỏ hàng
          cart = new ShoppingCart
          {
            UserId = userId
          };
          _db.ShoppingCarts.Add(cart);
        }
        _db.SaveChanges();

        // cart detail chi tiết giỏ hàng
        var cartItem = _db.CartDetails
          .FirstOrDefault(a => a.ShoppingCartId == cart.Id && a.BookId == bookId);

        // kiểm tra item trong giỏ hàng
        // kiểm tra hàng hóa (sách) trong giỏ hàng
        if (cartItem is not null)
        {
          cartItem.Quantity += quantity;
          // ví dụ trong giỏ hàng có sách doctor who rồi
          // thì số lượng 1 + 1 = 2 (người dùng bấm thêm vào giỏ hàng lần thứ hai đối với 1 món hàng)
        }
        else
        {
          var book = _db.Books.Find(bookId);
          cartItem = new CartDetail
          {
            //binding/mapping
            BookId = bookId,
            ShoppingCartId = cart.Id,
            Quantity = quantity,
            UnitPrice = book.Price
          };
          _db.CartDetails.Add(cartItem);
        }
        _db.SaveChanges();

      }
      catch (Exception ex)
      {

      }

      // đếm số lượng hàng trong giỏ hàng
      // cập nhật lại
      var cartItemCount = await GetCartItemCount(userId);
      return cartItemCount;
    }

    // xóa một sản phẩm trong giỏ hàng
    public async Task<int> RemoveItem(int bookId)
    {
      string userId = GetUserId();

      try
      {
        // kiểm tra xem khách hàng đó có đăng nhập hay chưa
        if (string.IsNullOrEmpty(userId))
        {
          throw new UnauthorizedAccessException("Khách hàng chưa đăng nhập. Vui lòng đăng nhập.");
        }
        var cart = await GetCart(userId);
        // kiểm tra giỏ hàng
        // giỏ hàng tương ứng với user đăng nhập không tồn tại
        // là do user đó chưa được xác nhận email
        // phải xác nhận email thì tài khoản đó mới là hợp lệ
        if (cart is null)
        {
          throw new InvalidOperationException("giỏ hàng không tồn tại, tài khoản đăng nhập không hợp lệ");
        }
        // chi tiết giỏ hàng
        // cart detail chi tiết giỏ hàng
        var cartItem = _db.CartDetails
          .FirstOrDefault(a => a.ShoppingCartId == cart.Id && a.BookId == bookId);

        // trường hợp giỏ hàng không có một món hàng nào
        if (cartItem is null)
        {
          throw new InvalidOperationException("giỏ hàng không có một món hàng nào");
        }
        else if (cartItem.Quantity == 1)
        {
          // nếu số lượng hàng là 1 thì xóa luôn món hàng đó trong giỏ hàng
          // cập nhật data trong bảng CartDetail
          _db.CartDetails.Remove(cartItem);
        }
        else
        {
          cartItem.Quantity = cartItem.Quantity - 1;
        }
        _db.SaveChanges();
      }
      catch (Exception)
      {

        throw;
      }
      // đếm số lượng hàng trong giỏ hàng
      // cập nhật lại
      var cartItemCount = await GetCartItemCount(userId);
      return cartItemCount;
    }

    public async Task<int> GetCartItemCount(string userId = "")
    {
      if (string.IsNullOrEmpty(userId))
      {
        userId = GetUserId();
      }

      // ta dùng query syntax (code giống giống sql)
      var data = await (from cart in _db.ShoppingCarts
                        join cartDetail in _db.CartDetails
                        on cart.Id equals cartDetail.ShoppingCartId
                        where cart.UserId == userId
                        select new { cartDetail.Id }
                        ).ToListAsync();
      return data.Count;
    }

    // tưởng mô cách này dễ
    //private bool HasCart(string userId)
    //{
    //  var ketQua = _db.ShoppingCarts.FirstOrDefault(x => x.UserId == userId);
    //  // toán tử 3 ngôi (lập trình C/C++)
    //  return ketQua != null ? true : false;
    //}
    
    // viết code dễ đọc hơn
    //private ShoppingCart GetCart(string userId)
    //{
    //  var gioHang = _db.ShoppingCarts.FirstOrDefault(x => x.UserId == userId);
    //  return gioHang;
    //}
    // ở đây ta không dùng nạp chồng toán tử (lập trình căn bản với C/C++, cấu trúc dữ liệu và giải thuật C/C++)
    // overloading, operator
    // duyệt tất cả các dòng dữ liệu trong bảng Giỏ Hàng (Cart)
    public async Task<ShoppingCart> GetCart(string userId)
    {
      var gioHang = await _db.ShoppingCarts.FirstOrDefaultAsync(x => x.UserId == userId);
      return gioHang;
    }

    // truy vấn dùng kiểu method syntax
    public async Task<ShoppingCart> GetUserCart()
    {
      var userId = GetUserId();

      // chưa đăng nhập
      // người dùng không tồn tại
      // không tồn tại người dùng này
      if (userId == null)
      {
        throw new InvalidOperationException("tài khoản người dùng không tồn tại");
      }

      // binding/truy vấn
      var cart = await _db.ShoppingCarts
        .Include(x => x.CartDetails)
        .ThenInclude(x => x.Book)
        .ThenInclude(x => x.Stock)
        .Include(x => x.CartDetails)
        .ThenInclude(x => x.Book)
        .ThenInclude(x => x.Genre)
        .Where(x => x.UserId == userId)
        .FirstOrDefaultAsync();

      return cart;
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

    // làm cơ bản thôi
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

    // code chức năng thanh toán thì rất là dài
    // dùng paypal: atm, ...
    // có api
    // thanh toán online và thanh toán COD
    // blogspot (thanh toán COD) + google sheet...


  }
}
