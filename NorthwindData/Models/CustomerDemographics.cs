using System.ComponentModel.DataAnnotations;

namespace NorthwindData.Models
{
    public class CustomerDemographics
    {
        [Required(AllowEmptyStrings = false, ErrorMessage = "CustomerTypeID is Required")]
        public string   CustomerTypeID  { get; set; } = string.Empty;
        public string?  CustomerDesc    { get; set; } = null;

        #region Relational Properties
        public List<CustomerDemographics> CustomerDemographicsList { get; set; } = new List<CustomerDemographics>();
        #endregion
    }
}
