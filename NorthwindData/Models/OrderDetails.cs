using ValidationTools.ValidationAttributes;

namespace NorthwindData.Models
{
    public class OrderDetails
    {
        [RequiredInt(DefaultValue = int.MinValue)]
        public int      OrderID     { get; set; } = int.MinValue;

        [RequiredInt(DefaultValue = int.MaxValue)]
        public int      ProductID   { get; set; } = int.MinValue;
        public decimal  UnitPrice   { get; set; }
        public short    Quantity    { get; set; }
        public Single   Discount    { get; set; }

        #region Relational Properties
        public Product Product { get; set; } = new Product();
        #endregion
    }
}
