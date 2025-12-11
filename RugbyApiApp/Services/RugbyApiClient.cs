using RestSharp;
using RugbyApiApp.DTOs;
using RugbyApiApp.Models;

namespace RugbyApiApp.Services
{
    public class RugbyApiClient
    {
        private readonly RestClient _client;
        private const string BaseUrl = "https://v1.rugby.api-sports.io";
        private const string MediaApiHost = "media.api-sports.io";
        private const string CdnHost = "bl-media-api-sports.b-cdn.net";

        public RugbyApiClient(string apiKey)
        {
            var options = new RestClientOptions(BaseUrl);

            _client = new RestClient(options);
            _client.AddDefaultHeader("x-apisports-key", apiKey);
        }

        /// <summary>
        /// Check if an API response contains errors and return the error message
        /// </summary>
        private static string? GetErrorMessage(Dictionary<string, string>? errors)
        {
            if (errors == null || errors.Count == 0)
                return null;

            return string.Join("; ", errors.Values);
        }

        /// <summary>
        /// Get all available seasons (returns list of years as integers)
        /// </summary>
        public async Task<(List<int>? Seasons, string? ErrorMessage)> GetSeasonsAsync()
        {
            var request = new RestRequest("/seasons");
            var response = await _client.ExecuteAsync<SeasonsApiResponse>(request);
            var errorMessage = GetErrorMessage(response.Data?.Errors);
            return (response.Data?.Response, errorMessage);
        }

        /// <summary>
        /// Get all countries
        /// </summary>
        public async Task<(List<CountryResponse>? Countries, string? ErrorMessage)> GetCountriesAsync()
        {
            var request = new RestRequest("/countries");
            var response = await _client.ExecuteAsync<ApiResponse<CountryResponse>>(request);
            var errorMessage = GetErrorMessage(response.Data?.Errors);
            return (response.Data?.Response, errorMessage);
        }

        /// <summary>
        /// Get country by ID
        /// </summary>
        public async Task<(CountryResponse? Country, string? ErrorMessage)> GetCountryAsync(int countryId)
        {
            var request = new RestRequest($"/countries/{countryId}");
            var response = await _client.ExecuteAsync<ApiResponse<CountryResponse>>(request);
            var errorMessage = GetErrorMessage(response.Data?.Errors);
            return (response.Data?.Response?.FirstOrDefault(), errorMessage);
        }

        /// <summary>
        /// Get all leagues
        /// </summary>
        public async Task<(List<LeagueResponse>? Leagues, string? ErrorMessage)> GetLeaguesAsync()
        {
            var request = new RestRequest("/leagues");
            var response = await _client.ExecuteAsync<ApiResponse<LeagueResponse>>(request);
            var errorMessage = GetErrorMessage(response.Data?.Errors);
            return (response.Data?.Response, errorMessage);
        }

        /// <summary>
        /// Get league by ID
        /// </summary>
        public async Task<(LeagueResponse? League, string? ErrorMessage)> GetLeagueAsync(int leagueId)
        {
            var request = new RestRequest($"/leagues/{leagueId}");
            var response = await _client.ExecuteAsync<ApiResponse<LeagueResponse>>(request);
            var errorMessage = GetErrorMessage(response.Data?.Errors);
            return (response.Data?.Response?.FirstOrDefault(), errorMessage);
        }

        /// <summary>
        /// Get all teams
        /// </summary>
        public async Task<(List<TeamResponse>? Teams, string? ErrorMessage)> GetTeamsAsync()
        {
            var request = new RestRequest("/teams");
            var response = await _client.ExecuteAsync<ApiResponse<TeamResponse>>(request);
            var errorMessage = GetErrorMessage(response.Data?.Errors);
            return (response.Data?.Response, errorMessage);
        }

        /// <summary>
        /// Get teams by league and season
        /// </summary>
        public async Task<(List<TeamResponse>? Teams, string? ErrorMessage)> GetTeamsByLeagueAsync(int leagueId, int season)
        {
            var request = new RestRequest("/teams")
                .AddParameter("league", leagueId)
                .AddParameter("season", season);

            var response = await _client.ExecuteAsync<ApiResponse<TeamResponse>>(request);
            var errorMessage = GetErrorMessage(response.Data?.Errors);
            return (response.Data?.Response, errorMessage);
        }

        /// <summary>
        /// Get team by ID
        /// </summary>
        public async Task<(TeamResponse? Team, string? ErrorMessage)> GetTeamAsync(int teamId)
        {
            var request = new RestRequest($"/teams/{teamId}");
            var response = await _client.ExecuteAsync<ApiResponse<TeamResponse>>(request);
            var errorMessage = GetErrorMessage(response.Data?.Errors);
            return (response.Data?.Response?.FirstOrDefault(), errorMessage);
        }

        /// <summary>
        /// Get all games
        /// </summary>
        public async Task<(List<GameResponse>? Games, string? ErrorMessage)> GetGamesAsync()
        {
            var request = new RestRequest("/games");
            var response = await _client.ExecuteAsync<ApiResponse<GameResponse>>(request);
            var errorMessage = GetErrorMessage(response.Data?.Errors);
            return (response.Data?.Response, errorMessage);
        }

        /// <summary>
        /// Get games for a season
        /// </summary>
        public async Task<(List<GameResponse>? Games, string? ErrorMessage)> GetGamesBySeasonAsync(int season)
        {
            var request = new RestRequest("/games")
                .AddParameter("season", season);

            var response = await _client.ExecuteAsync<ApiResponse<GameResponse>>(request);
            var errorMessage = GetErrorMessage(response.Data?.Errors);
            return (response.Data?.Response, errorMessage);
        }

        /// <summary>
        /// Get games for a league and season
        /// </summary>
        public async Task<(List<GameResponse>? Games, string? ErrorMessage)> GetGamesByLeagueAndSeasonAsync(int leagueId, int season)
        {
            var request = new RestRequest("/games")
                .AddParameter("league", leagueId)
                .AddParameter("season", season);

            var response = await _client.ExecuteAsync<ApiResponse<GameResponse>>(request);
            var errorMessage = GetErrorMessage(response.Data?.Errors);
            return (response.Data?.Response, errorMessage);
        }

        /// <summary>
        /// Get games for a league
        /// </summary>
        public async Task<(List<GameResponse>? Games, string? ErrorMessage)> GetGamesByLeagueAsync(int leagueId)
        {
            var request = new RestRequest("/games")
                .AddParameter("league", leagueId);

            var response = await _client.ExecuteAsync<ApiResponse<GameResponse>>(request);
            var errorMessage = GetErrorMessage(response.Data?.Errors);
            return (response.Data?.Response, errorMessage);
        }

        /// <summary>
        /// Get games for a league across multiple seasons
        /// </summary>
        public async Task<(List<GameResponse>? Games, string? ErrorMessage)> GetGamesByLeagueAndSeasonsAsync(int leagueId, int startSeason, int endSeason)
        {
            var allGames = new List<GameResponse>();
            string? lastError = null;

            for (int season = startSeason; season <= endSeason; season++)
            {
                var (games, errorMessage) = await GetGamesByLeagueAndSeasonAsync(leagueId, season);
                if (errorMessage != null)
                {
                    lastError = errorMessage;
                }
                if (games != null)
                {
                    allGames.AddRange(games);
                }
            }

            return (allGames.Count > 0 ? allGames : null, lastError);
        }

        /// <summary>
        /// Convert media.api-sports.io URL to CDN URL
        /// </summary>
        public static string ConvertMediaUrlToCdn(string? imageUrl)
        {
            if (string.IsNullOrEmpty(imageUrl))
                return string.Empty;

            // Replace media.api-sports.io domain with BunnyCDN domain
            return imageUrl.Replace(MediaApiHost, CdnHost);
        }

        /// <summary>
        /// Convert api-sports.io URL to CDN URL
        /// </summary>
        public static string ConvertApiUrlToCdn(string? imageUrl)
        {
            if (string.IsNullOrEmpty(imageUrl))
                return string.Empty;

            // Replace api-sports.io domain with BunnyCDN domain
            return imageUrl.Replace("api-sports.io", CdnHost);
        }

        /// <summary>
        /// Get CDN URL for a resource path
        /// </summary>
        public static string GetCdnUrl(string path)
        {
            if (string.IsNullOrEmpty(path))
                return string.Empty;

            return $"https://{CdnHost}{path}";
        }
    }
}
