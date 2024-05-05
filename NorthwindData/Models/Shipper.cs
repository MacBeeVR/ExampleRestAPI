using System.ComponentModel.DataAnnotations;
using ValidationTools.ValidationAttributes;

namespace NorthwindData.Models
{
    public class Shipper
    {
        [RequiredInt(DefaultValue = int.MinValue)]
        public int      ShipperID           { get; set; } = int.MinValue;

        [Required(AllowEmptyStrings = false), MaxLength(40)]
        public string   CompanyName         { get; set; } = string.Empty;

        [Phone, MaxLength(24)]
        public string?  Phone               { get; set; } = null;
    }
}
