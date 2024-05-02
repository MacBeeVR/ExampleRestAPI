namespace NorthwindData.Models
{
    public class Order
    {
        public int          OrderID         { get; set; }
        public string?      CustomerID      { get; set; } = null;
        public int?         EmployeeID      { get; set; } = null;
        public DateTime?    OrderDate       { get; set; } = null;
        public DateTime?    RequiredDate    { get; set; } = null;
        public DateTime?    ShippedDate     { get; set; } = null;
        public int?         ShipVia         { get; set; } = null;
        public decimal?     Freight         { get; set; } = null;
        public string?      ShipName        { get; set; } = null;
        public string?      ShipAddress     { get; set; } = null;
        public string?      ShipCity        { get; set; } = null;
        public string?      ShipRegion      { get; set; } = null;
        public string?      ShipPostalCode  { get; set; } = null;
        public string?      ShipCountry     { get; set; } = null;
    }
}
