namespace RugbyApiApp.MAUI.ViewModels
{
    /// <summary>
    /// Represents a data item displayed in the DataGrid
    /// Supports all entity types: Countries, Seasons, Leagues, Teams, Games
    /// </summary>
    public class GridDataItem
    {
        public int Id { get; set; }
        public string? Name { get; set; }
        public string? Code { get; set; }
        public string? Country { get; set; }
        public string? Type { get; set; }
        public int? Year { get; set; }
        public string? Current { get; set; }
        public string? Status { get; set; }
        public string? Home { get; set; }
        public string? Away { get; set; }
        public string? Date { get; set; }
        public string? Venue { get; set; }
        public bool Favorite { get; set; }
    }
}
