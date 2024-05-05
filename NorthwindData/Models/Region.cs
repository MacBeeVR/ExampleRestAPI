﻿using System.ComponentModel.DataAnnotations;
using ValidationTools.ValidationAttributes;

namespace NorthwindData.Models
{
    public class Region
    {
        [RequiredInt(DefaultValue = int.MinValue)]
        public int      RegionID            { get; set; } = int.MinValue;

        [MaxLength(50)]
        public string   RegionDescription   { get; set; } = string.Empty;
    }
}
