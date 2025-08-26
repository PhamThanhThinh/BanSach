using BanSach.Models;
using BanSach.Models.DTO;
using BanSach.Repo;
using Microsoft.AspNetCore.Mvc;
using System.Collections.Generic;
using System.Diagnostics;
using System.Threading.Tasks;

namespace BanSach.Controllers
{
  public class HomeController : Controller
  {
    // khai báo constructor (hàm dựng) khác với hàm hủy destructor (không cần khai báo)
    // dùng ctrl . để mở gợi ý code => sau đó dùng chức năng generate code tự động
    private readonly ILogger<HomeController> _logger;
    private readonly IHomeRepository _homeRepository;

    public HomeController(ILogger<HomeController> logger, IHomeRepository homeRepository)
    {
      _logger = logger;
      _homeRepository = homeRepository;
    }

    //public HomeController(ILogger<HomeController> logger)
    //{
    //  _logger = logger;
    //}
    // chức năng mới của vs2022 ồ wow
    // khi ta tạo một variable(biến) thì method sẽ biến đổi theo kiểu dữ liệu mà biến đó dùng
    public async Task<IActionResult> Index(string term = "", int genreId = 0)
    {
      IEnumerable<Book> books = await _homeRepository.GetBooks(term, genreId);
      IEnumerable<Genre> genres = await _homeRepository.Genres();

      BookViewModel bookViewModel = new BookViewModel
      {
        Books = books,
        Genres = genres,
        //chúng ta sẽ viết thêm ở đây
        Term = term,
        GenreId = genreId
        // viết như trên là ok
      };

      //return View();

      // nếu dùng return View(); thì sẽ bị null

      // ở đây ta nháp thử cách đơn giản, test xem nó có chạy không
      //return View(books);

      // thực tế dữ liệu sẽ được lấy từ bảng
      // nên chúng ta return viewmodel
      return View(bookViewModel);
    }

    public IActionResult Privacy()
    {
      return View();
    }

    [ResponseCache(Duration = 0, Location = ResponseCacheLocation.None, NoStore = true)]
    public IActionResult Error()
    {
      return View(new ErrorViewModel { RequestId = Activity.Current?.Id ?? HttpContext.TraceIdentifier });
    }
  }
}
