using System.Text.Json.Serialization;

namespace RugbyApiApp.DTOs
{
    public class TeamResponse
    {
        [JsonPropertyName("id")]
        public int? Id { get; set; }
        
        [JsonPropertyName("name")]
        public string? Name { get; set; }
        
        [JsonPropertyName("code")]
        public string? Code { get; set; }
        
        [JsonPropertyName("flag")]
        public string? Flag { get; set; }
        
        [JsonPropertyName("logo")]
        public string? Logo { get; set; }
    }
}
