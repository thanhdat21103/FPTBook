namespace ASM.Models
{
    public class Cart
    {
        public int CartID { get; set; }
        public string EmailUser { get; set; }
        public int BookID { get; set; }
        public int Quanlity { get; set; }
        public int Total { get; set; }
        public Book Book { get; set; }
    }
}
