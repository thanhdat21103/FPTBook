using ASMNhom3.Models;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;

namespace ASMNhom3.Controllers
{
    public class AdminController : Controller
    {
        private readonly ILogger<AdminController> _logger;
        private readonly ASMNhom3Context _db;
        public AdminController(ILogger<AdminController> logger, ASMNhom3Context context)
        {
            _logger = logger;
            _db = context;
        }
        public IActionResult Index()
        {
            if (HttpContext.Session.GetString("Role") == "Admin")
            {
                return View();
            }
            else
            {
                return RedirectToAction("Login", "Login");
            }

        }
        public IActionResult ListUser()
        {
            if (HttpContext.Session.GetString("Role") == "Admin")
            {

                ViewBag.user = getAllUser();
                return View();
            }
            else
            {
                return RedirectToAction("Login", "Login");
            }

        }

        public IActionResult ListCate()
        {
            if (HttpContext.Session.GetString("Role") == "Admin")
            {
                ViewBag.category = _db.Categorys.ToList();
                return View();
            }
            else
            {
                return RedirectToAction("Login", "Login");
            }
        }
        public IActionResult DeleteCate(int id)
        {
            if (HttpContext.Session.GetString("Role") == "Admin")
            {
                _db.Categorys.Remove(getDetailCate(id));
                _db.SaveChanges();
                return RedirectToAction("ListCate");
            }
            else
            {
                return RedirectToAction("Login", "Login");
            }
        }
        public IActionResult ConfirmCate(int id)
        {
            if (HttpContext.Session.GetString("Role") == "Admin")
            {
                foreach (var cate in _db.Categorys)
                {
                    if (cate.CategoryId == id)
                    {
                        cate.IsConfirm = true;
                    }
                }
                _db.SaveChanges();
                return RedirectToAction("ListCate");
            }
            else
            {
                return RedirectToAction("Login", "Login");
            }
        }
        public IActionResult ResetPassUser(int id)
        {
            if (HttpContext.Session.GetString("Role") == "Admin")
            {
                ViewBag.user = getDetailUser(id);
                return View();
            }
            else
            {
                return RedirectToAction("Login", "Login");
            }
        }
       
        [HttpPost]
        [ValidateAntiForgeryToken]
        public IActionResult ResetPassUser(Account model)
        {
            if (HttpContext.Session.GetString("Role") == "Admin")
            {
                if (ModelState.IsValid)
                {
                    _db.Accounts.Update(model);
                    _db.SaveChanges();
                    return RedirectToAction("ListUser");
                }
                return View(model);
            }
            else
            {
                return RedirectToAction("Login", "Login");
            }
        }
    
        //hamphu
        private List<Account> getAllUser()
        {
            return _db.Accounts.Where(c => c.Roles == "User").ToList();
        }
        private List<Account> getAllOwner()
        {
            return _db.Accounts.Where(c => c.Roles == "Owner").ToList();
        }
        private List<Category> getAllCate()
        {
            return _db.Categorys.ToList();
        }
        private Category getDetailCate(int id)
        {
            return _db.Categorys.Find(id);
        }
        private List<History> getAllHistory()
        {
            return _db.Histories.ToList();
        }
        private Account getDetailUser(int id)
        {
            return _db.Accounts.Find(id);
        }
    }
}
