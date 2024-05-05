using System.ComponentModel.DataAnnotations;

namespace NorthwindData.Models
{
    public class Customer
    {
        [Required(      ErrorMessage = "CustomerID is Required")]
        [MaxLength(5,   ErrorMessage = "CustomerID Must be at Most 5 Characters Long")]
        public string   CustomerID      { get; set; } = string.Empty;

        [Required(      ErrorMessage = "CompanyName is Required")]
        [MaxLength(40,  ErrorMessage = "CompanyName Must be at Most 40 Characters Long")]
        public string   CompanyName     { get; set; } = string.Empty;

        [MaxLength(30, ErrorMessage = "ContactName Must be at Most 30 Characters Long")]
        public string?  ContactName     { get; set; } = null;

        [MaxLength(30, ErrorMessage = "ContactTitle Must be at Most 30 Characters Long")]
        public string?  ContactTitle    { get; set; } = null;

        [MaxLength(60, ErrorMessage = "Address Must be at Most 60 Characters Long")]
        public string?  Address         { get; set; } = null;

        [MaxLength(15, ErrorMessage = "City Must be at Most 15 Characters Long")]
        public string?  City            { get; set; } = null;

        [MaxLength(15, ErrorMessage = "Region Must be at Most 15 Characters Long")]
        public string?  Region          { get; set; } = null;

        [MaxLength(10, ErrorMessage = "PostalCode Must be at Most 10 Characters Long")]
        public string?  PostalCode      { get; set; } = null;

        [MaxLength(15, ErrorMessage = "Country Must be at Most 15 Characters Long")]
        public string?  Country         { get; set; } = null;

        [Phone(         ErrorMessage = "Invalid Phone Number")]
        [MaxLength(24,  ErrorMessage = "Phone Must be at Most 24 Characters Long")]
        public string?  Phone           { get; set; } = null;

        [Phone(        ErrorMessage = "Invalid Fax Number")]
        [MaxLength(24, ErrorMessage = "Fax Must be at Most 24 Characters Long")]
        public string?  Fax             { get; set; } = null;
    }
}
