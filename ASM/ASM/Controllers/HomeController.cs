using ASM.Areas.Identity.Data;
using ASM.Models;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using System.Diagnostics;

namespace ASM.Controllers
{
    public class HomeController : Controller
    {
        private readonly ILogger<HomeController> _logger;
        private readonly ASMDBContext _db;
        private readonly UserManager<ManageUser> _userManager;
        public HomeController(ILogger<HomeController> logger, ASMDBContext context, UserManager<ManageUser> userManager)
        {
            _logger = logger;
            _db = context;
            _userManager = userManager;
        }

        //hienthibook
        public IActionResult Index()
        {
            var category = getAllCategory();
            ViewBag.category = category;
            return View();
        }
        public IActionResult ListBook()
        {
            var _product = getAllBook();
            ViewBag.book = _product;
            return View();
        }
        public IActionResult Details(int id)
        {
            var book = _db.Book.Find(id);
            return View(book);
        }
        //hamphu
        public List<Book> getAllBook()
        {
            return _db.Book.ToList();
        }
        private List<Category> getAllCategory()
        {
            return _db.Category.ToList();
        }
        private Book getDetailBook(int id)
        {
            return _db.Book.Find(id);
        }
        private Cart getCart(int id)
        {
            return _db.Cart.Find(id);
        }
        //

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