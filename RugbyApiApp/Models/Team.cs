namespace RugbyApiApp.Models
{
    public class Team
    {
        public int Id { get; set; }
        public string? Name { get; set; }
        public string? Code { get; set; }
        public string? Flag { get; set; }
        public string? Logo { get; set; }
        public bool IsDataComplete { get; set; }
        public DateTime CreatedAt { get; set; }
        public DateTime UpdatedAt { get; set; }
    }
}
