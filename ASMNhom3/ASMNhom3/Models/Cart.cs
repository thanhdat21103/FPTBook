using Microsoft.AspNetCore.Identity;

namespace ASMNhom3.Models
{
    public class Cart
    {
        public int CartID { get; set; }
        public string EmailUser { get; set; }
        public int BookID { get; set; }
        public int Quanlity { get; set; }
        public int Price { get; set; }
        public int Total { get; set; }
    }
}
