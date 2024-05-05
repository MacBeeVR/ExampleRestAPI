using System.ComponentModel.DataAnnotations;
using ValidationTools.ValidationAttributes;

namespace NorthwindData.Models
{
    public class Employee
    {
        #region DB Field Properties
        [RequiredInt(DefaultValue = int.MinValue)]
        public int          EmployeeID      { get; set; } = int.MinValue;

        [Required, MaxLength(20)]
        public string       LastName        { get; set; } = string.Empty;

        [Required, MaxLength(10)]
        public string       FirstName       { get; set; } = string.Empty;

        [MaxLength(30)]
        public string?      Title           { get; set; } = null;

        [MaxLength(25)]
        public string?      TitleOfCourtesy { get; set; } = null;

        public DateTime?    BirthDate       { get; set; } = null;
        public DateTime?    HireDate        { get; set; } = null;

        [MaxLength(60)]
        public string?      Address         { get; set; } = null;

        [MaxLength(15)]
        public string?      City            { get; set; } = null;

        [MaxLength(15)]
        public string?      Region          { get; set; } = null;

        [MaxLength(10)]
        public string?      PostalCode      { get; set; } = null;

        [MaxLength(15)]
        public string?      Country         { get; set; } = null;

        [Phone, MaxLength(24)]
        public string?      HomePhone       { get; set; } = null;

        [MaxLength(4)]
        public string?      Extension       { get; set; } = null;
        public byte[]?      Photo           { get; set; } = null;
        public string?      Notes           { get; set; } = null;
        public int?         ReportsTo       { get; set; } = null;

        [MaxLength(255)]
        public string?      PhotoPath       { get; set; } = null;
        #endregion

        #region Relational Properties
        public Employee? Boss { get; set; } = null;
        #endregion
    }
}
