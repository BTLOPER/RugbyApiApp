using System.Text.Json;
using System.Text.Json.Serialization;

namespace RugbyApiApp.DTOs
{
    public class ApiResponse<T>
    {
        [JsonPropertyName("get")]
        public string? Get { get; set; }
        
        [JsonPropertyName("paging")]
        public ApiPaging? Paging { get; set; }
        
        [JsonPropertyName("errors")]
        [JsonConverter(typeof(ErrorsJsonConverter))]
        public Dictionary<string, string>? Errors { get; set; }
        
        [JsonPropertyName("response")]
        public List<T>? Response { get; set; }
    }

    public class ApiPaging
    {
        [JsonPropertyName("current")]
        public int? Current { get; set; }
        
        [JsonPropertyName("total")]
        public int? Total { get; set; }
    }

    /// <summary>
    /// Custom JSON converter to handle errors field that can be either an empty array or an object
    /// </summary>
    public class ErrorsJsonConverter : JsonConverter<Dictionary<string, string>>
    {
        public override Dictionary<string, string>? Read(ref Utf8JsonReader reader, Type typeToConvert, JsonSerializerOptions options)
        {
            if (reader.TokenType == JsonTokenType.StartArray)
            {
                // If it's an array (empty or not), skip it and return null
                reader.Skip();
                return null;
            }
            
            if (reader.TokenType == JsonTokenType.StartObject)
            {
                // If it's an object, deserialize it as a dictionary
                return JsonSerializer.Deserialize<Dictionary<string, string>>(ref reader, options);
            }

            return null;
        }

        public override void Write(Utf8JsonWriter writer, Dictionary<string, string>? value, JsonSerializerOptions options)
        {
            if (value == null)
            {
                writer.WriteNullValue();
            }
            else
            {
                JsonSerializer.Serialize(writer, value, options);
            }
        }
    }
}
