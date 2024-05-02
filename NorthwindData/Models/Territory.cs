namespace NorthwindData.Models
{
    public class Territory
    {
        public string   TerritoryID             { get; set; } = string.Empty;
        public string   TerritoryDescription    { get; set; } = string.Empty;
        public int      RegionID                { get; set; }
    }
}
