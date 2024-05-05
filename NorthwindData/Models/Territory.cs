using System.ComponentModel.DataAnnotations;
using ValidationTools.ValidationAttributes;

namespace NorthwindData.Models
{
    public class Territory
    {
        #region DB Field Properties
        [Required(AllowEmptyStrings = false), MaxLength(20)]
        public string   TerritoryID             { get; set; } = string.Empty;

        [Required(AllowEmptyStrings = false), MaxLength(50)]
        public string   TerritoryDescription    { get; set; } = string.Empty;

        [RequiredInt(DefaultValue = int.MinValue)]
        public int      RegionID                { get; set; } = int.MinValue;
        #endregion

        #region Relational Properties
        public Region Region { get; set; } = new Region();
        #endregion
    }
}
