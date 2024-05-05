using System.ComponentModel.DataAnnotations;
using ValidationTools.ValidationAttributes;

namespace NorthwindData.Models
{
    public class Category
    {
        [RequiredInt(DefaultValue = int.MinValue, ErrorMessage = "CategoryID is Required")]
        public int      CategoryID      { get; set; } = int.MinValue;

        [Required(      ErrorMessage = "CategoryName is Required")]
        [MaxLength(15,  ErrorMessage = "CategoryName is Too Long!")]
        public string   CategoryName    { get; set; } = string.Empty;
        public string?  Description     { get; set; } = null;
        public byte[]?  Picture         { get; set; } = null;
    }
}
