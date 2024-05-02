namespace NorthwindData.Models
{
    public class Product
    {
        public int      ProductID       { get; set; }
        public string   ProductName     { get; set; } = string.Empty;
        public int?     SupplierID      { get; set; } = null;
        public int?     CategoryID      { get; set; } = null;
        public string?  QuantityPerUnit { get; set; } = null;
        public decimal? UnitPrice       { get; set; } = null;
        public short?   UnitsInStock    { get; set; } = null;
        public short?   UnitsOnOrder    { get; set; } = null;
        public short?   ReorderLevel    { get; set; } = null;
        public bool     Discontinued    { get; set; }
    }
}
