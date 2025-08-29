using BanSach.Models;

namespace BanSach.Repo
{
  public interface ICartRepository
  {
    Task<int> AddItem(int bookId, int quantity);
    Task<int> RemoveItem(int bookId);
    Task<ShoppingCart> GetCart(string userId);
    Task<ShoppingCart> GetUserCart();
  }
}
