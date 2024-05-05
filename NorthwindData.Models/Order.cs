using System.ComponentModel.DataAnnotations;
using ValidationTools.ValidationAttributes;

namespace NorthwindData.Models
{
    public class Order
    {
        #region DB Field Properties
        [RequiredInt(DefaultValue = int.MinValue)]
        public int          OrderID         { get; set; } = int.MinValue;

        [MaxLength(5)]
        public string?      CustomerID      { get; set; } = null;
        public int?         EmployeeID      { get; set; } = null;
        public DateTime?    OrderDate       { get; set; } = null;
        public DateTime?    RequiredDate    { get; set; } = null;
        public DateTime?    ShippedDate     { get; set; } = null;
        public int?         ShipVia         { get; set; } = null;
        public decimal?     Freight         { get; set; } = null;

        [MaxLength(40)]
        public string?      ShipName        { get; set; } = null;

        [MaxLength(60)]
        public string?      ShipAddress     { get; set; } = null;

        [MaxLength(15)]
        public string?      ShipCity        { get; set; } = null;

        [MaxLength(15)]
        public string?      ShipRegion      { get; set; } = null;

        [MaxLength(10)]
        public string?      ShipPostalCode  { get; set; } = null;

        [MaxLength(15)]
        public string?      ShipCountry     { get; set; } = null;
        #endregion

        #region Relational Properties
        public Employee?            Employee    { get; set; } = null;
        public Customer?            Customer    { get; set; } = null;
        public Shipper?             Shipper     { get; set; } = null;
        public List<OrderDetails>   Details     { get; set; } = new List<OrderDetails>();
        #endregion
    }
}
