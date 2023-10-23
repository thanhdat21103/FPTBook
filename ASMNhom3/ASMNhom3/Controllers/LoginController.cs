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
        private readonly ASMNhom3Context _context;

        public LoginController(ASMNhom3Context context)
        {
            _context = context;
        }

        // GET: Login
        
    }
}
