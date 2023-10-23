using ASMNhom3.Models;
using Microsoft.AspNetCore.Mvc;
using System.Diagnostics;

namespace ASMNhom3.Controllers
{
    public class HomeController : Controller
    {
        private readonly ILogger<HomeController> _logger;
        private readonly ASMNhom3Context _db;
        public HomeController(ILogger<HomeController> logger, ASMNhom3Context context)
        {
            _logger = logger;
            _db = context;
        }
        //ko can dang nhap, hien thi book
        public IActionResult Index()
        {
            ViewBag.UserEmail = HttpContext.Session.GetString("Email");
            var category = getAllCategory();
            ViewBag.category = category;
            return View();
        }
        public IActionResult BookList()
        {
            ViewBag.UserEmail = HttpContext.Session.GetString("Email");
            var _product = getAllBook();
            ViewBag.book = _product;
            return View();
        }
        public IActionResult BookDetail(int id)
        {
            ViewBag.UserEmail = HttpContext.Session.GetString("Email");
            var book = _db.Books.Find(id);
            return View(book);
        }
        //hamphu
        public List<Book> getAllBook()
        {
            return _db.Books.ToList();
        }
        private List<Category> getAllCategory()
        {
            return _db.Categorys.ToList();
        }
        private Book getDetailBook(int id)
        {
            return _db.Books.Find(id);
        }
        private Cart getCart(int id)
        {
            return _db.Carts.Find(id);
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