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
        //cart
        public IActionResult addCart(int id, int quanlity)
        {
            if (HttpContext.Session.GetString("Email") != null)
            {
                var userEmail = HttpContext.Session.GetString("Email");
                var product = getDetailBook(id);
                Cart existingCartItem = null;
                foreach (var cart in _db.Carts)
                {
                    if (cart.BookID == product.BookID && userEmail == cart.EmailUser)
                    {
                        existingCartItem = cart;
                        break;
                    }
                }

                if (existingCartItem != null)
                {
                    existingCartItem.Quanlity += quanlity;
                }
                else
                {
                    var newCartItem = new Cart
                    {
                        EmailUser = userEmail,
                        BookID = product.BookID,
                        Quanlity = quanlity,
                        Price = product.Price,
                        Total = quanlity * product.Price
                    };
                    _db.Carts.Add(newCartItem);
                }
                product.Quantity -= quanlity;
                _db.SaveChanges(); // Lưu dữ liệu vào cơ sở dữ liệu

                return RedirectToAction("BookDetail", "Home", new { id = product.BookID });
            }
            else
            {
                return RedirectToAction("Login", "Login");
            }
        }
        public IActionResult deleteCart(int id)
        {
            if (HttpContext.Session.GetString("Email") != null)
            {
                ViewBag.UserEmail = HttpContext.Session.GetString("Email");
                var cart = getCart(id);
                if (cart != null)
                {
                    var product = getDetailBook(cart.BookID);
                    if (product != null)
                    {
                        product.Quantity += cart.Quanlity;
                        _db.Carts.Remove(cart);
                        _db.SaveChanges();
                    }
                }
            }
            return RedirectToAction("CartList", "Home");
        }

        public IActionResult CartList()
        {
            if (HttpContext.Session.GetString("Email") != null)
            {
                ViewBag.UserEmail = HttpContext.Session.GetString("Email");
                var userEmail = HttpContext.Session.GetString("Email");
                var userCarts = _db.Carts
                    .Where(c => c.EmailUser == userEmail)// Include thông tin của Book từ BookID
                    .ToList();

                var total = 0;
                foreach (var cart in userCarts)
                {
                    foreach (var book in _db.Books)
                    {
                        if (book.BookID == cart.BookID)
                        {
                            total += cart.Total;
                        }
                    }

                }
                ViewBag.userCarts = userCarts;
                ViewBag.TotalPrice = total;

                return View();
            }

            return RedirectToAction(nameof(Index));
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