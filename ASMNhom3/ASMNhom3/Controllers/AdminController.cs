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
            return View();
        }
        public IActionResult ListUser()
        {
            ViewBag.user = getAllUser();
            return View();
        }
        public IActionResult ListCate()
        {
            ViewBag.category = _db.Categorys.ToList();
            return View();
        }
        public IActionResult DeleteCate(int id)
        {
            _db.Categorys.Remove(getDetailCate(id));
            _db.SaveChanges();
            return RedirectToAction("ListCate");
        }
        public IActionResult ConfirmCate(int id)
        {
            foreach(var cate in _db.Categorys)
            {
                if(cate.CategoryId == id)
                {
                    cate.IsConfirm = true;
                }
            }
            _db.SaveChanges();
            return RedirectToAction("ListCate");
        }
        //hamphu
        public List<Account> getAllUser()
        {
            return _db.Accounts.Where(c => c.Roles == "User").ToList();
        }
        public List<Category> getAllCate()
        {
            return _db.Categorys.ToList();
        }
        private Category getDetailCate(int id)
        {
            return _db.Categorys.Find(id);
        }
    }
}
