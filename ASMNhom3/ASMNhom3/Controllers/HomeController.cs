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
        //Profile
        public IActionResult Profile()
        {
            if (HttpContext.Session.GetString("Email") != null)
            {
                ViewBag.UserEmail = HttpContext.Session.GetString("Email");
                ViewBag.user = _db.Accounts.FirstOrDefault(c => c.Email == HttpContext.Session.GetString("Email"));
                ViewBag.history = _db.Histories.Where(c => c.Email == HttpContext.Session.GetString("Email"));
                ViewBag.checkout = _db.QueueCheckOuts.Where(c => c.Email == HttpContext.Session.GetString("Email"));
                return View();
            }
            else
            {
                return RedirectToAction("Login", "Login");
            }
        }
        [HttpPost]
        public IActionResult updateProfile(Account model)
        {
            if (HttpContext.Session.GetString("Email") != null)
            {
                _db.Accounts.Update(model);
                _db.SaveChanges();
                return RedirectToAction("Profile");
            }
            else
            {
                return RedirectToAction("Login", "Login");
            }
        }
        [HttpPost]
        public IActionResult updatePassword(Account model)
        {
            if (HttpContext.Session.GetString("Email") != null)
            {
                _db.Accounts.Update(model);
                _db.SaveChanges();
                return RedirectToAction("Profile");
            }
            else
            {
                return RedirectToAction("Login", "Login");
            }
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
                return RedirectToAction("CartList", "Home");
            }
            else
            {
                return RedirectToAction("Login", "Login");
            }
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
                ViewBag.TotalPrice = total.ToString();
                return View();
            }
            else
            {
                return RedirectToAction("Login", "Login");
            }
        }
        public IActionResult ChecKOut()
        {
            if (HttpContext.Session.GetString("Email") != null)
            {
                if (ViewBag.TotalPrice != "0")
                {
                    ViewBag.UserEmail = HttpContext.Session.GetString("Email");
                    var userEmail = HttpContext.Session.GetString("Email");
                    List<Cart> carts = new List<Cart>();
                    ViewBag.TotalPrice = 0;
                    ViewBag.TotalQuanlity = 0;
                    foreach (var cart in _db.Carts)
                    {
                        if (userEmail == cart.EmailUser)
                        {
                            carts.Add(cart);
                        }
                    }
                    foreach (var cart in _db.Carts)
                    {
                        foreach (var book in _db.Books)
                        {
                            if (userEmail == cart.EmailUser && cart.BookID == book.BookID)
                            {
                                ViewBag.TotalPrice += cart.Total;
                                ViewBag.TotalQuanlity += cart.Quanlity;
                            }
                        }
                    }
                    ViewBag.book = getAllBook();
                    ViewBag.product = carts;
                    ViewBag.user = _db.Accounts.FirstOrDefault(c => c.Email == userEmail);
                    return View();
                }
                else
                {
                    ModelState.AddModelError("", "Gio hang trong");
                    return RedirectToAction("CartList");
                }
            }
            else
            {
                return RedirectToAction("Login", "Login");
            }
        }
        public IActionResult ConfirmCheckOut(QueueCheckOut model)
        {
            if (HttpContext.Session.GetString("Email") != null)
            {
                var userEmail = HttpContext.Session.GetString("Email");
                if (ModelState.IsValid && model.TotalPrice != 0 && model.TotalQuantity != 0)
                {
                    var newQueue = new QueueCheckOut
                    {
                        FirstName = model.FirstName,
                        LastName = model.LastName,
                        PhoneNum = model.PhoneNum,
                        Address = model.Address,
                        Email = model.Email,
                        Note = model.Note,
                        TotalPrice = model.TotalPrice,
                        TotalQuantity = model.TotalQuantity,
                        IsConfirm = false
                    };
                    _db.QueueCheckOuts.Add(newQueue);
                    List<Cart> oldcart = new List<Cart>();
                    foreach (var cart in _db.Carts)
                    {
                        if (cart.EmailUser == userEmail)
                        {
                            oldcart.Add(cart);
                        }
                    }
                    for (int i = 0; i < oldcart.Count(); i++)
                    {
                        _db.Carts.Remove(oldcart[i]);
                    }
                    _db.SaveChanges();
                    return RedirectToAction("Index");
                }
                else
                {
                    ModelState.AddModelError("", "Gio hang trong");
                    return RedirectToAction("ChecKOut");
                }
            }
            else
            {
                return RedirectToAction("Login", "Login");
            }
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
            ViewBag.category = getAllCategory();
            var _product = getAllBook();
            ViewBag.book = _product;
            return View();
        }
        public IActionResult SearchByCate(int id)
        {          
            List<Book> books = new List<Book>();
            foreach (var book in _db.Books)
            {
                if (book.CategoryID == id)
                {
                    books.Add(book);
                }
            }
            if (books.Count != 0)
            {
                ViewBag.book = books;
                ViewBag.category = getAllCategory();
                ViewBag.notice = null;
            }
            else
            {
                ViewBag.book = null;
                ViewBag.category = getAllCategory();
                ViewBag.notice = "EMPTY";
            }
            return View();
        }
        [HttpPost]
        public IActionResult SearchByName(string name)
        {
            List<Book> books = new List<Book>();
            foreach (var book in _db.Books)
            {
                if (book.Title == name)
                {
                    books.Add(book);
                }
            }
            if (books.Count != 0)
            {
                ViewBag.book = books;
                ViewBag.category = getAllCategory();
            }
            else
            {
                ViewBag.book = null;
                ViewBag.category = getAllCategory();
            }
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
        private Category getCate(int id)
        {
            return _db.Categorys.Find(id);
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