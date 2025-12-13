namespace RugbyApiApp.Models
{
    public class Game
    {
        public int Id { get; set; }
        public int? LeagueId { get; set; }
        public int? Season { get; set; }
        public int HomeTeamId { get; set; }
        public int AwayTeamId { get; set; }
        public DateTime? Date { get; set; }
        public string? Status { get; set; }
        public string? Venue { get; set; }
        public int? HomeTeamScore { get; set; }
        public int? AwayTeamScore { get; set; }
        public bool IsDataComplete { get; set; }
        public DateTime CreatedAt { get; set; }
        public DateTime UpdatedAt { get; set; }

        public Team? HomeTeam { get; set; }
        public Team? AwayTeam { get; set; }
        public ICollection<Video>? Videos { get; set; }
    }
}
