using ASMNhom3.Models;
using Microsoft.AspNetCore.Mvc;

namespace ASMNhom3.Controllers
{
    public class OwnerController : Controller
    {
        private readonly ILogger<OwnerController> _logger;
        private readonly ASMNhom3Context _db;
        public OwnerController(ILogger<OwnerController> logger, ASMNhom3Context context)
        {
            _logger = logger;
            _db = context;
        }
        public IActionResult Index()
        {
            if (HttpContext.Session.GetString("Role") == "Owner")
            {
                return View();
            }
            else
            {
                return RedirectToAction("Login", "Login");
            }
        }
        public IActionResult ListBook()
        {
            if (HttpContext.Session.GetString("Role") == "Owner")
            {
                ViewBag.book = getAllBook();
                ViewBag.category = getAllCategory();
                return View();
            }
            else
            {
                return RedirectToAction("Login", "Login");
            }
        }
        public IActionResult CreateBook()
        {
            if (HttpContext.Session.GetString("Role") == "Owner")
            {
                ViewBag.category = getAllCategory().Where(c => c.IsConfirm == true).ToList();
                return View();
            }
            else
            {
                return RedirectToAction("Login", "Login");
            }
        }
        [HttpPost]
        public IActionResult CreateBook(Book model)
        {
            if (HttpContext.Session.GetString("Role") == "Owner")
            {
                if (ModelState.IsValid)
                {
                    _db.Books.Add(model);
                    _db.SaveChanges();

                    return RedirectToAction("ListBook");
                }
                return View(model);
            }
            else
            {
                return RedirectToAction("Login", "Login");
            }
        }
        public IActionResult EditBook(int id)
        {
            if (HttpContext.Session.GetString("Role") == "Owner")
            {
                ViewBag.book = getDetailBook(id);
                ViewBag.category = getAllCategory();
                return View();
            }
            else
            {
                return RedirectToAction("Login", "Login");
            }
        }
        [HttpPost]
        [ValidateAntiForgeryToken]
        public IActionResult EditBook(Book model)
        {
            if (HttpContext.Session.GetString("Role") == "Owner")
            {
                if (ModelState.IsValid)
                {
                    _db.Books.Update(model);
                    _db.SaveChanges();
                    return RedirectToAction("ListBook");
                }
                return View(model);
            }
            else
            {
                return RedirectToAction("Login", "Login");
            }
        }
        public IActionResult Delete(int id)
        {
            if (HttpContext.Session.GetString("Role") == "Owner")
            {
                _db.Books.Remove(getDetailBook(id));
                _db.SaveChanges();
                return RedirectToAction("ListBook");
            }
            else
            {
                return RedirectToAction("Login", "Login");
            }
        }
        public IActionResult WaitingCheckOut()
        {
            if (HttpContext.Session.GetString("Role") == "Owner")
            {
                List<QueueCheckOut> queues = new List<QueueCheckOut>();
                foreach (var queue in _db.QueueCheckOuts)
                {
                    if (queue.IsConfirm == false)
                    {
                        queues.Add(queue);
                    }
                }
                ViewBag.queue = queues;
                return View();
            }
            else
            {
                return RedirectToAction("Login", "Login");
            }
        }
        public IActionResult ConfirmQueue(int id)
        {
            if (HttpContext.Session.GetString("Role") == "Owner")
            {
                foreach (var queue in _db.QueueCheckOuts)
                {
                    if (id == queue.QueueCheckOutID)
                    {
                        queue.IsConfirm = true;
                    }
                }
                _db.SaveChanges();
                return RedirectToAction("WaitingCheckOut");
            }
            else
            {
                return RedirectToAction("Login", "Login");
            }
        }
        public IActionResult ShippingCheckOut()
        {
            if (HttpContext.Session.GetString("Role") == "Owner")
            {
                List<QueueCheckOut> queues = new List<QueueCheckOut>();
                foreach (var queue in _db.QueueCheckOuts)
                {
                    if (queue.IsConfirm == true)
                    {
                        queues.Add(queue);
                    }
                }
                ViewBag.queue = queues;
                return View();
            }
            else
            {
                return RedirectToAction("Login", "Login");
            }
        }
        public IActionResult ConfirmShip(int id)
        {
            if (HttpContext.Session.GetString("Role") == "Owner")
            {
                var shipping = getQueue(id);
                var history = new History
                {
                    FirstName = shipping.FirstName,
                    LastName = shipping.LastName,
                    PhoneNum = shipping.PhoneNum,
                    Address = shipping.Address,
                    Email = shipping.Email,
                    Note = shipping.Note,
                    TotalPrice = shipping.TotalPrice,
                    TotalQuantity = shipping.TotalQuantity
                };
                _db.Histories.Add(history);
                _db.QueueCheckOuts.Remove(shipping);
                _db.SaveChanges();
                return RedirectToAction("ShippingCheckOut");
            }
            else
            {
                return RedirectToAction("Login", "Login");
            }
        }
        public IActionResult Hisory()
        {
            if (HttpContext.Session.GetString("Role") == "Owner")
            {
                ViewBag.Null = "hong co null";
                ViewBag.history = getAllHistory();
                return View();
            }
            else
            {
                return RedirectToAction("Login", "Login");
            }
        }
        [HttpPost]
        public IActionResult Hisory(string email)
        {
            if (HttpContext.Session.GetString("Role") == "Owner")
            {
                if (!string.IsNullOrEmpty(email))
                {
                    var history = _db.Histories.Where(u => u.Email.Contains(email)).ToList();
                    ViewBag.history = history;
                }
                else
                {
                    ViewBag.history = getAllHistory();
                }

                return View();
            }
            else
            {
                return RedirectToAction("Login", "Login");
            }
        }
        public IActionResult ListType()
        {
            if (HttpContext.Session.GetString("Role") == "Owner")
            {
                ViewBag.category = _db.Categorys.ToList();
                return View();
            }
            else
            {
                return RedirectToAction("Login", "Login");
            }
        }
        public IActionResult addCate(string name)
        {
            if (HttpContext.Session.GetString("Role") == "Owner")
            {
                var cate = new Category
                {
                    Name = name,
                    IsConfirm = false
                };
                _db.Categorys.Add(cate);
                _db.SaveChanges();
                return RedirectToAction("ListType");
            }
            else
            {
                return RedirectToAction("Login", "Login");
            }
        }
        public IActionResult DeleteCate(int id)
        {
            if (HttpContext.Session.GetString("Role") == "Owner")
            {
                _db.Categorys.Remove(getDetailCate(id));
                _db.SaveChanges();
                return RedirectToAction("ListType");
            }
            else
            {
                return RedirectToAction("Login", "Login");
            }
        }
        //hamphu
        private QueueCheckOut getDetailCheckout(int id)
        {
            return _db.QueueCheckOuts.Find(id);
        }
        private List<History> getAllHistory()
        {
            return _db.Histories.ToList();
        }
        public QueueCheckOut getQueue(int id)
        {
            return _db.QueueCheckOuts.Find(id);
        }
        public List<Book> getAllBook()
        {
            return _db.Books.ToList();
        }
        private Book getDetailBook(int id)
        {
            return _db.Books.Find(id);
        }
        public List<Category> getAllCategory()
        {
            return _db.Categorys.ToList();
        }
        private Category getDetailCate(int id)
        {
            return _db.Categorys.Find(id);
        }
    }
}
