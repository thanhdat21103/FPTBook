using ASM.Areas.Identity.Data;
using ASM.Models;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;

namespace ASM.Controllers
{
    public class OwnerController : Controller
    {
        private readonly ILogger<OwnerController> _logger;
        private readonly ASMDBContext _db;
        private readonly UserManager<ManageUser> _userManager;
        public OwnerController(ILogger<OwnerController> logger, ASMDBContext context, UserManager<ManageUser> userManager)
        {
            _logger = logger;
            _db = context;
            _userManager = userManager;
        }
        public IActionResult ListBook()
        {
            ViewBag.book = getAllBook();
            ViewBag.category = getAllCategory();
            return View();
        }
        public IActionResult Index()
        {
            return View();
        }
        //private List<History> getAllHistory()
        //{
        //    return _db.Histories.ToList();
        //}
        //public QueueCheckOut getQueue(int id)
        //{
        //    return _db.QueueCheckOuts.Find(id);
        //}
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
    }
}
