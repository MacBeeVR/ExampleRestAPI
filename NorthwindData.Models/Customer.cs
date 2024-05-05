using System.ComponentModel.DataAnnotations;

namespace NorthwindData.Models
{
    public class Customer
    {
        #region DB Field Properties
        [Required, MaxLength(5)]
        public string   CustomerID      { get; set; } = string.Empty;

        [Required, MaxLength(40)]
        public string   CompanyName     { get; set; } = string.Empty;

        [MaxLength(30)]
        public string?  ContactName     { get; set; } = null;

        [MaxLength(30)]
        public string?  ContactTitle    { get; set; } = null;

        [MaxLength(60)]
        public string?  Address         { get; set; } = null;

        [MaxLength(15)]
        public string?  City            { get; set; } = null;

        [MaxLength(15)]
        public string?  Region          { get; set; } = null;

        [MaxLength(10)]
        public string?  PostalCode      { get; set; } = null;

        [MaxLength(15)]
        public string?  Country         { get; set; } = null;

        [Phone, MaxLength(24)]
        public string?  Phone           { get; set; } = null;

        [Phone, MaxLength(24)]
        public string?  Fax             { get; set; } = null;
        #endregion

        #region Relational Properties
        public List<CustomerDemographics> CustomerDemographics { get; set; } = new List<CustomerDemographics>();
        #endregion
    }
}
