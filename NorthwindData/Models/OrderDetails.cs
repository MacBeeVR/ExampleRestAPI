namespace NorthwindData.Models
{
    public class OrderDetails
    {
        public int      OrderID     { get; set; }
        public int      ProductID   { get; set; }
        public decimal  UnitPrice   { get; set; }
        public short    Quantity    { get; set; }
        public Single   Discount    { get; set; }
    }
}
