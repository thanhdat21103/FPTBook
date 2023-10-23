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
    }
}
