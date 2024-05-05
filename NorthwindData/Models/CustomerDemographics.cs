using System.ComponentModel.DataAnnotations;

namespace NorthwindData.Models
{
    public class CustomerDemographics
    {
        #region DB Field Properties
        [Required(AllowEmptyStrings = false)]
        public string   CustomerTypeID  { get; set; } = string.Empty;
        public string?  CustomerDesc    { get; set; } = null;
        #endregion

        #region Relational Properties
        public List<CustomerDemographics> CustomerDemographicsList { get; set; } = new List<CustomerDemographics>();
        #endregion
    }
}
