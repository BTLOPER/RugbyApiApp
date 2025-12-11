using System.Text.Json.Serialization;

namespace RugbyApiApp.DTOs
{
    public class LeagueResponse
    {
        [JsonPropertyName("id")]
        public int? Id { get; set; }
        
        [JsonPropertyName("name")]
        public string? Name { get; set; }
        
        [JsonPropertyName("type")]
        public string? Type { get; set; }
        
        [JsonPropertyName("logo")]
        public string? Logo { get; set; }
        
        [JsonPropertyName("country")]
        public CountryResponse? Country { get; set; }
    }
}
