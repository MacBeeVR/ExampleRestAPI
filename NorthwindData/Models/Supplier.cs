using System.ComponentModel.DataAnnotations;
using ValidationTools.ValidationAttributes;

namespace NorthwindData.Models
{
    public class Supplier
    {
        [RequiredInt(DefaultValue = int.MinValue)]
        public int      SupplierID      { get; set; } = int.MinValue;

        [Required(AllowEmptyStrings = false), MaxLength(40)]
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
        public string?  HomePage        { get; set; } = null;
    }
}
