using System.Text.Json.Serialization;

namespace RugbyApiApp.DTOs
{
    public class GameResponse
    {
        [JsonPropertyName("id")]
        public int? Id { get; set; }
        
        [JsonPropertyName("date")]
        public DateTime? Date { get; set; }
        
        [JsonPropertyName("time")]
        public string? Time { get; set; }
        
        [JsonPropertyName("timestamp")]
        public long? Timestamp { get; set; }
        
        [JsonPropertyName("timezone")]
        public string? Timezone { get; set; }
        
        [JsonPropertyName("week")]
        public string? Week { get; set; }
        
        [JsonPropertyName("status")]
        public GameStatusResponse? Status { get; set; }
        
        [JsonPropertyName("country")]
        public CountryResponse? Country { get; set; }
        
        [JsonPropertyName("league")]
        public GameLeagueResponse? League { get; set; }
        
        [JsonPropertyName("teams")]
        public GameTeamsResponse? Teams { get; set; }
        
        [JsonPropertyName("scores")]
        public GameScoresResponse? Scores { get; set; }
        
        [JsonPropertyName("periods")]
        public GamePeriodsResponse? Periods { get; set; }

        // Convenience properties for compatibility with existing code
        public int? LeagueId => League?.Id;
        public int? Season => League?.Season;
        public TeamResponse? Home => Teams?.Home;
        public TeamResponse? Away => Teams?.Away;
        public GameScoreResponse? Score => Scores;
        public string? Venue => null; // API doesn't provide venue in this response
    }

    public class GameStatusResponse
    {
        [JsonPropertyName("long")]
        public string? Long { get; set; }
        
        [JsonPropertyName("short")]
        public string? Short { get; set; }
    }

    public class GameLeagueResponse
    {
        [JsonPropertyName("id")]
        public int? Id { get; set; }
        
        [JsonPropertyName("name")]
        public string? Name { get; set; }
        
        [JsonPropertyName("type")]
        public string? Type { get; set; }
        
        [JsonPropertyName("logo")]
        public string? Logo { get; set; }
        
        [JsonPropertyName("season")]
        public int? Season { get; set; }
    }

    public class GameTeamsResponse
    {
        [JsonPropertyName("home")]
        public TeamResponse? Home { get; set; }
        
        [JsonPropertyName("away")]
        public TeamResponse? Away { get; set; }
    }

    public class GameScoresResponse : GameScoreResponse
    {
    }

    public class GameScoreResponse
    {
        [JsonPropertyName("home")]
        public int? Home { get; set; }
        
        [JsonPropertyName("away")]
        public int? Away { get; set; }
    }

    public class GamePeriodsResponse
    {
        [JsonPropertyName("first")]
        public GamePeriodResponse? First { get; set; }
        
        [JsonPropertyName("second")]
        public GamePeriodResponse? Second { get; set; }
        
        [JsonPropertyName("overtime")]
        public GamePeriodResponse? Overtime { get; set; }
        
        [JsonPropertyName("second_overtime")]
        public GamePeriodResponse? SecondOvertime { get; set; }
    }

    public class GamePeriodResponse
    {
        [JsonPropertyName("home")]
        public int? Home { get; set; }
        
        [JsonPropertyName("away")]
        public int? Away { get; set; }
    }
}
