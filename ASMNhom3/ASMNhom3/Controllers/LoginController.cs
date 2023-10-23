using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Rendering;
using Microsoft.EntityFrameworkCore;
using ASMNhom3.Models;

namespace ASMNhom3.Controllers
{
    public class LoginController : Controller
    {
        private readonly ILogger<LoginController> _logger;
        private readonly ASMNhom3Context _db;
        public LoginController(ILogger<LoginController> logger, ASMNhom3Context context)
        {
            _logger = logger;
            _db = context;
        }
        public IActionResult Login()
        {
            return View();
        }
        [HttpPost]
        public IActionResult Login(Account account)
        {
            if (account == null)
            {
                return View("Error");
            }
            else
            {
                var user = _db.Accounts.FirstOrDefault(u =>
                    u.Email == account.Email && u.Password == account.Password);

                if (user != null)
                {
                    if (user.Roles == "User")
                    {
                        HttpContext.Session.SetString("Email", account.Email);
                        return RedirectToAction("Index", "Home");
                    }
                    else if (user.Roles == "Owner")
                    {
                        return RedirectToAction("Index", "Owner");
                    }
                    else if (user.Roles == "Admin")
                    {
                        return RedirectToAction("Index", "Admin");
                    }
                    else
                    {
                        ModelState.AddModelError("", "bạn không có quyền truy cập.");
                    }
                }
            }
            ModelState.AddModelError("", "Email hoặc mật khẩu không đúng");
            return View();
        }
        public IActionResult Register()
        {
            return View();
        }
        [HttpPost]
        public IActionResult Register(Account model)
        {
            if (ModelState.IsValid)
            {
                foreach (var user in _db.Accounts)
                {
                    if (user.Email == model.Email)
                    {
                        ModelState.AddModelError("", "Email nay da duoc su dung");
                        return View();
                    }
                    else if (user.PhoneNum == model.PhoneNum)
                    {
                        ModelState.AddModelError("", "SDT nay da duoc su dung");
                        return View();
                    }
                    else if (user.Email == model.Email && user.PhoneNum == model.PhoneNum)
                    {
                        ModelState.AddModelError("", "SDT va email nay da duoc su dung");
                        return View();
                    }
                }
                _db.Accounts.Add(model);
                _db.SaveChanges();
                return RedirectToAction("Login", "Login");
            }

            else
            {
                ViewBag.error = "Loi vl";
                return View();
            }
        }

        public IActionResult Logout()
        {
            HttpContext.Session.Clear();
            return RedirectToAction("Index", "Home");
        }

    }
}
