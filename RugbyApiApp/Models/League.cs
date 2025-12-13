namespace RugbyApiApp.Models
{
    public class League
    {
        public int Id { get; set; }
        public string? Name { get; set; }
        public string? Type { get; set; }
        public string? Logo { get; set; }
        public string? CountryName { get; set; }
        public string? CountryCode { get; set; }
        public string? CountryFlag { get; set; }
        public bool IsDataComplete { get; set; }
        public DateTime CreatedAt { get; set; }
        public DateTime UpdatedAt { get; set; }
    }
}
