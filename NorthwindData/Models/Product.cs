using System.ComponentModel.DataAnnotations;
using ValidationTools.ValidationAttributes;

namespace NorthwindData.Models
{
    public class Product
    {
        [RequiredInt(DefaultValue = int.MinValue)]
        public int      ProductID       { get; set; } = int.MinValue;

        [Required(AllowEmptyStrings = false), MaxLength(40)]
        public string   ProductName     { get; set; } = string.Empty;
        public int?     SupplierID      { get; set; } = null;
        public int?     CategoryID      { get; set; } = null;

        [MaxLength(20)]
        public string?  QuantityPerUnit { get; set; } = null;
        public decimal? UnitPrice       { get; set; } = null;
        public short?   UnitsInStock    { get; set; } = null;
        public short?   UnitsOnOrder    { get; set; } = null;
        public short?   ReorderLevel    { get; set; } = null;
        public bool     Discontinued    { get; set; }

        #region Relational Properties
        public Supplier? Supplier { get; set; } = null;
        public Category? Category { get; set; } = null;
        #endregion
    }
}
