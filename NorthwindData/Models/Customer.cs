namespace NorthwindData.Models
{
    public class Customer
    {
        public string   CustomerID      { get; set; } = string.Empty;
        public string   CompanyName     { get; set; } = string.Empty;
        public string?  ContactName     { get; set; } = null;
        public string?  ContactTitle    { get; set; } = null;
        public string?  Address         { get; set; } = null;
        public string?  City            { get; set; } = null;
        public string?  Region          { get; set; } = null;
        public string?  PostalCode      { get; set; } = null;
        public string?  Country         { get; set; } = null;
        public string?  Phone           { get; set; } = null;
        public string?  Fax             { get; set; } = null;
    }
}
