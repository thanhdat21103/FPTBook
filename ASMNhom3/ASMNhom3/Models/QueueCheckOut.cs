namespace ASMNhom3.Models
{
    public class QueueCheckOut
    {
        public int QueueCheckOutID { get; set; }
        public string FirstName { get; set; }
        public string LastName { get; set; }
        public string Address { get; set; }
        public string PhoneNum { get; set; }
        public string Email { get; set; }
        public int TotalQuantity { get; set; }
        public int TotalPrice { get; set; }
        public string? Note { get; set; }
        public bool IsConfirm { get; set; }
    }
}
