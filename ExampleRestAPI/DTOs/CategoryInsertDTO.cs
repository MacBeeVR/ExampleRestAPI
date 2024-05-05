using System.ComponentModel.DataAnnotations;

namespace ExampleRestAPI.DTOs
{
    public class CategoryInsertDTO
    {
        [Required(      ErrorMessage = "CategoryName is Required")]
        [MaxLength(15,  ErrorMessage = "CategoryName is Too Long!")]
        public string   CategoryName    { get; set; } = string.Empty;
        public string?  Description     { get; set; }
        public byte[]?  Picture         { get; set; }
    }
}
