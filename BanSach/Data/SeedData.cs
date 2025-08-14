using BanSach.Constants;
using BanSach.Models;
using Microsoft.AspNetCore.Identity;
using Microsoft.EntityFrameworkCore;
using System.Security.Cryptography.Xml;
using System.Threading.Tasks;

namespace BanSach.Data
{
  public class SeedData
  {
    //public static async Task SeedDefaultData(IServiceProvider service)
    //{
    //  try
    //  {
    //    var context = service.GetService<ApplicationDbContext>();

    //    var user = service.GetService<UserManager<IdentityUser>>();
    //    var role = service.GetService<RoleManager<IdentityRole>>();

    //    var adminRole = await role.RoleExistsAsync(Roles.Admin.ToString());
    //    if (!adminRole)
    //    {
    //      // add role to db
    //      await role.CreateAsync(new IdentityRole(Roles.Admin.ToString()));
    //    }

    //    var userRole = await role.RoleExistsAsync(Roles.User.ToString());
    //    if (!userRole)
    //    {
    //      // add role to db
    //      await role.CreateAsync(new IdentityRole(Roles.User.ToString()));
    //    }

    //    // tạo tài khoản admin
    //    var admin = new IdentityUser
    //    {
    //      UserName = "admin@test.xyz",
    //      Email = "admin@test.xyz",
    //      EmailConfirmed = true,
    //    };

    //    // tạo mật khẩu cho tài khoản
    //    var userAdmin = await user.FindByEmailAsync(admin.Email);
    //    if (userAdmin is null)
    //    {
    //      // vừa có hoa vừa có thường 
    //      // ký tự đặc biệt
    //      // > 8 ký tự
    //      await user.CreateAsync(admin, "Admin12345!@#$%");
    //      await user.AddToRoleAsync(admin, Roles.Admin.ToString());
    //    }

    //    // data cho bảng Genre (thể loại)
    //    if (!context.Genres.Any())
    //    {
    //      await SeedGenreAsync(context);
    //    }

    //    // data dành cho bảng Book
    //    if (!context.Books.Any())
    //    {
    //      await SeedBookAsync(context);
    //      // liên kết bảng Book với bảng Stock
    //      // giúp cập nhật dữ liệu 2 bảng Book và Stock
    //      await context.Database.ExecuteSqlRawAsync(@"
    //      INSERT INTO Stock(BookId, Quantity)
    //      SELECT b.Id, 1
    //      FROM Book b
    //      WHERE NOT EXISTS (
    //        select * from [Stock]
    //      );
    //    ");
    //    }

    //    // Bảng OrderStatus: Chờ, Đang Giao, Hoàn Thành
    //    if (!context.OrderStatuses.Any())
    //    {
    //      await SeedOrderStatusAsync(context);
    //    }
    //  }
    //  catch (System.Exception ex)
    //  {
    //    Console.WriteLine(ex.Message);
    //  }

    //}
    //private static async Task SeedGenreAsync(ApplicationDbContext context)
    //{
    //  var genres = new[]{
    //     new Genre { GenreName = "Action" },
    //     new Genre { GenreName = "Romance" },
    //     new Genre { GenreName = "Drama" },
    //   };

    //  await context.Genres.AddRangeAsync(genres);
    //  await context.SaveChangesAsync();
    //}

    //private static async Task SeedBookAsync(ApplicationDbContext context)
    //{
    //  var books = new List<Book>
    //  {
    //    // tương ứng với thể loại là Action
    //    new Book { BookName = "Doctor Who: The Tenth Doctor 1: Revolutions of Terror", AuthorName = "Nick Abadzis", Price = 19.99, GenreId = 1 },
    //    // doctor + Missy(the Master)
    //    new Book { BookName = "Doctor Who : The Twelfth Doctor Complete Year One", AuthorName = "Robbie Morrison", Price = 36.37, GenreId = 2 },

    //    new Book { BookName = "Cô Vợ Đáng Yêu Của Hào Môn Thiếu Gia", AuthorName = "abc", Price = 19, GenreId = 3 },
    //  };

    //  await context.Books.AddRangeAsync(books);
    //  await context.SaveChangesAsync();
    //}

    //private static async Task SeedOrderStatusAsync(ApplicationDbContext context)
    //{
    //  var orderStatuses = new []
    //  {
    //    new OrderStatus { StatusId = 1, StatusName = "Chờ" },
    //    new OrderStatus { StatusId = 2, StatusName = "Đang Giao" },
    //    new OrderStatus { StatusId = 3, StatusName = "Hoàn Thành" },
    //    new OrderStatus { StatusId = 4, StatusName = "Trả Hàng" },
    //    new OrderStatus { StatusId = 5, StatusName = "Hoàn Tiền" },
    //  };

    //  await context.OrderStatuses.AddRangeAsync(orderStatuses);
    //  await context.SaveChangesAsync();
    //}

    public static async Task SeedDefaultData(IServiceProvider service)
    {
      var userMgr = service.GetService<UserManager<IdentityUser>>();
      var roleMgr = service.GetService<RoleManager<IdentityRole>>();
      // add role
      await roleMgr.CreateAsync(new IdentityRole(Roles.Admin.ToString()));
      await roleMgr.CreateAsync(new IdentityRole(Roles.User.ToString()));

      var admin = new IdentityUser
      {
        UserName = "admin@gmail.com",
        Email = "admin@gmail.com",
        EmailConfirmed = true
      };

      var userInDatabase = await userMgr.FindByEmailAsync(admin.Email);
      if (userInDatabase is null)
      {
        await userMgr.CreateAsync(admin, "Admin12345!@#$%");
        await userMgr.AddToRoleAsync(admin, Roles.Admin.ToString());
      }
    }
  }
}
