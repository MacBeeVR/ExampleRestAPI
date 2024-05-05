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
    }
}
