using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.EntityFrameworkCore;
using ASMNhom3.Models;

    public class ASMNhom3Context : DbContext
    {
        public ASMNhom3Context (DbContextOptions<ASMNhom3Context> options)
            : base(options)
        {
        }

    public DbSet<Book> Books { get; set; }
    public DbSet<Category> Categorys { get; set; }
    public DbSet<Cart> Carts { get; set; }
    public DbSet<QueueCheckOut> QueueCheckOuts { get; set; }
    public DbSet<History> Histories { get; set; }
    public DbSet<Account> Accounts { get; set; }
}
