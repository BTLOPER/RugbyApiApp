using System.Text.Json.Serialization;

namespace RugbyApiApp.DTOs
{
    public class StatusResponse
    {
        [JsonPropertyName("get")]
        public string? Get { get; set; }

        [JsonPropertyName("parameters")]
        public List<object>? Parameters { get; set; }

        [JsonPropertyName("errors")]
        public List<object>? Errors { get; set; }

        [JsonPropertyName("results")]
        public int? Results { get; set; }

        [JsonPropertyName("paging")]
        public ApiPaging? Paging { get; set; }

        [JsonPropertyName("response")]
        public StatusData? Response { get; set; }
    }

    public class StatusData
    {
        [JsonPropertyName("account")]
        public AccountInfo? Account { get; set; }

        [JsonPropertyName("subscription")]
        public SubscriptionInfo? Subscription { get; set; }

        [JsonPropertyName("requests")]
        public RequestInfo? Requests { get; set; }
    }

    public class AccountInfo
    {
        [JsonPropertyName("firstname")]
        public string? FirstName { get; set; }

        [JsonPropertyName("lastname")]
        public string? LastName { get; set; }

        [JsonPropertyName("email")]
        public string? Email { get; set; }
    }

    public class SubscriptionInfo
    {
        [JsonPropertyName("plan")]
        public string? Plan { get; set; }

        [JsonPropertyName("end")]
        public DateTime? End { get; set; }

        [JsonPropertyName("active")]
        public bool? Active { get; set; }
    }

    public class RequestInfo
    {
        [JsonPropertyName("current")]
        public int? Current { get; set; }

        [JsonPropertyName("limit_day")]
        public int? LimitDay { get; set; }
    }
}
