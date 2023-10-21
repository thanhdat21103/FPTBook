namespace ASM.Models
{
    public class Book
    {
        public int BookID { get; set; }
        public string Title { get; set; }
        public string Description { get; set; }
        public string Image { get; set; }
        public int CategoryID { get; set; }
        public int Quantity { get; set; }
        public int Price { get; set; }
        public Category Category { get; set; }

    }
}
