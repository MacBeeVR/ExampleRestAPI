﻿namespace NorthwindData.Models
{
    public class Category
    {
        public int      CategoryID      { get; set; }
        public string   CategoryName    { get; set; } = string.Empty;
        public string?  Description     { get; set; } = null;
        public byte[]?  Picture         { get; set; } = null;
    }
}
