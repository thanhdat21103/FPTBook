using ASM.Areas.Identity.Data;
using ASM.Models;
using Microsoft.AspNetCore.Authorization;
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
        [Authorize(Roles = "test1")]
        //hienthibook
        public IActionResult Index()
        {
            var category = getAllCategory();
            ViewBag.category = category;
            return View();
        }
        [Authorize(Roles = "test1")]
        public IActionResult ListBook()
        {
            var _product = getAllBook();
            ViewBag.book = _product;
            return View();
        }
        [Authorize(Roles = "test1")]
        public IActionResult Details(int id)
        {
            var book = _db.Books.Find(id);
            return View(book);
        }
        [Authorize(Roles = "test1")]
        //hamphu
        public List<Book> getAllBook()
        {
            return _db.Books.ToList();
        }
        [Authorize(Roles = "test1")]
        private List<Category> getAllCategory()
        {
            return _db.Categorys.ToList();
        }
        [Authorize(Roles = "test1")]
        private Book getDetailBook(int id)
        {
            return _db.Books.Find(id);
        }
        [Authorize(Roles = "test1")]
        private Cart getCart(int id)
        {
            return _db.Carts.Find(id);
        }
        //
        [Authorize(Roles = "test1")]
        public IActionResult Privacy()
        {
            return View();
        }
        [Authorize(Roles = "test1")]
        [ResponseCache(Duration = 0, Location = ResponseCacheLocation.None, NoStore = true)]
        public IActionResult Error()
        {
            return View(new ErrorViewModel { RequestId = Activity.Current?.Id ?? HttpContext.TraceIdentifier });
        }
    }
}