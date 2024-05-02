namespace NorthwindData.Models
{
    public class Employee
    {
        public int          EmployeeID      { get; set; } 
        public string       LastName        { get; set; } = string.Empty;
        public string       FirstName       { get; set; } = string.Empty;
        public string?      Title           { get; set; } = null;
        public string?      TitleOfCourtesy { get; set; } = null;
        public DateTime?    BirthDate       { get; set; } = null;
        public DateTime?    HireDate        { get; set; } = null;
        public string?      Address         { get; set; } = null;
        public string?      City            { get; set; } = null;
        public string?      Region          { get; set; } = null;
        public string?      PostalCode      { get; set; } = null;
        public string?      Country         { get; set; } = null;
        public string?      HomePhone       { get; set; } = null;
        public string?      Extension       { get; set; } = null;
        public byte[]?      Photo           { get; set; } = null;
        public string?      Notes           { get; set; } = null;
        public int?         ReportsTo       { get; set; } = null;
        public string?      PhotoPath       { get; set; } = null;
    }
}
