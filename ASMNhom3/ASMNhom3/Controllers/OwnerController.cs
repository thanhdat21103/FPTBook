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
            return View();
        }
        public IActionResult ListBook()
        {
            ViewBag.book = getAllBook();
            ViewBag.category = getAllCategory();
            return View();
        }
        public IActionResult CreateBook()
        {
            ViewBag.category = getAllCategory();
            return View();
        }
        [HttpPost]
        public IActionResult CreateBook(Book model)
        {
            if (ModelState.IsValid)
            {
                _db.Books.Add(model);
                _db.SaveChanges();

                return RedirectToAction("ListBook");
            }
            return View(model);
        }
        public IActionResult EditBook(int id)
        {
            ViewBag.book = getDetailBook(id);
            ViewBag.category = getAllCategory();
            return View();
        }
        [HttpPost]
        [ValidateAntiForgeryToken]
        public IActionResult EditBook(Book model)
        {
            if (ModelState.IsValid)
            {
                _db.Books.Update(model);
                _db.SaveChanges();
                return RedirectToAction("ListBook");
            }
            return View(model);
        }
        public IActionResult Delete(int id)
        {
            _db.Books.Remove(getDetailBook(id));
            _db.SaveChanges();
            return RedirectToAction("ListBook");
        }
        public IActionResult WaitingCheckOut()
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
        public IActionResult ConfirmQueue(int id)
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
        public IActionResult ShippingCheckOut()
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
        public IActionResult ConfirmShip(int id)
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
        public IActionResult Hisory()
        {
            ViewBag.Null = "hong co null";
            ViewBag.history = getAllHistory();
            return View();
        }
        [HttpPost]
        public IActionResult Hisory(string email)
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
        public IActionResult ListType()
        {
            ViewBag.category = _db.Categorys.ToList();
            return View();
        }
        public IActionResult addCate(string name)
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
        public IActionResult DeleteCate(int id)
        {
            _db.Categorys.Remove(getDetailCate(id));
            _db.SaveChanges();
            return RedirectToAction("ListType");
        }
        //hamphu
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
