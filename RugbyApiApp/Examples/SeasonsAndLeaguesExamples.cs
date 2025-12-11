using RugbyApiApp.Services;

namespace RugbyApiApp.Examples
{
    /// <summary>
    /// Examples for using Seasons and Leagues functionality
    /// </summary>
    public static class SeasonsAndLeaguesExamples
    {
        /// <summary>
        /// Example: Get all seasons and display information
        /// </summary>
        public static async Task Example_DisplayAllSeasons(DataService dataService)
        {
            var seasons = await dataService.GetSeasonsAsync();

            Console.WriteLine("\n=== All Seasons ===");
            foreach (var season in seasons)
            {
                var status = season.IsCurrent ? "[CURRENT]" : "[PAST]";
                Console.WriteLine($"{status} Season {season.Year}: {season.StartDate:yyyy-MM-dd} to {season.EndDate:yyyy-MM-dd}");
            }
        }

        /// <summary>
        /// Example: Get the current season and its details
        /// </summary>
        public static async Task Example_GetCurrentSeason(DataService dataService)
        {
            var currentSeason = await dataService.GetCurrentSeasonAsync();

            if (currentSeason != null)
            {
                Console.WriteLine($"\n=== Current Season ===");
                Console.WriteLine($"Year: {currentSeason.Year}");
                Console.WriteLine($"Start: {currentSeason.StartDate:yyyy-MM-dd}");
                Console.WriteLine($"End: {currentSeason.EndDate:yyyy-MM-dd}");
            }
            else
            {
                Console.WriteLine("\nNo current season found");
            }
        }

        /// <summary>
        /// Example: List all leagues with their countries
        /// </summary>
        public static async Task Example_ListAllLeagues(DataService dataService)
        {
            var leagues = await dataService.GetLeaguesAsync();

            Console.WriteLine("\n=== All Leagues ===");
            foreach (var league in leagues)
            {
                Console.WriteLine($"{league.Name} ({league.Type})");
                Console.WriteLine($"  Country: {league.CountryName} ({league.CountryCode})");
            }
        }

        /// <summary>
        /// Example: Get games for a specific league and season
        /// </summary>
        public static async Task Example_LeagueGames(DataService dataService, int leagueId, int season)
        {
            var league = await dataService.GetLeagueAsync(leagueId);
            if (league == null)
            {
                Console.WriteLine($"League {leagueId} not found");
                return;
            }

            var games = await dataService.GetGamesByLeagueAndSeasonAsync(leagueId, season);

            Console.WriteLine($"\n=== {league.Name} - Season {season} ===");
            Console.WriteLine($"Total Games: {games.Count}");

            foreach (var game in games.Take(10))
            {
                var homeTeam = game.HomeTeam?.Name ?? "Unknown";
                var awayTeam = game.AwayTeam?.Name ?? "Unknown";
                var score = game.HomeTeamScore.HasValue && game.AwayTeamScore.HasValue
                    ? $"{game.HomeTeamScore} - {game.AwayTeamScore}"
                    : "- vs -";

                Console.WriteLine($"{game.Date:yyyy-MM-dd} | {homeTeam} {score} {awayTeam} ({game.Status})");
            }
        }

        /// <summary>
        /// Example: Get all leagues for a specific country
        /// </summary>
        public static async Task Example_LeaguesByCountry(DataService dataService, string countryCode)
        {
            var leagues = await dataService.GetLeaguesAsync();
            var countryLeagues = leagues.Where(l => l.CountryCode == countryCode).ToList();

            Console.WriteLine($"\n=== Leagues in {countryCode} ===");
            if (countryLeagues.Count == 0)
            {
                Console.WriteLine("No leagues found for this country");
                return;
            }

            foreach (var league in countryLeagues)
            {
                Console.WriteLine($"- {league.Name} ({league.Type})");
            }
        }

        /// <summary>
        /// Example: Get teams for a league in the current season
        /// </summary>
        public static async Task Example_TeamsByLeagueCurrentSeason(DataService dataService, RugbyApiClient apiClient)
        {
            var currentSeason = await dataService.GetCurrentSeasonAsync();
            var leagues = await dataService.GetLeaguesAsync();

            if (currentSeason == null || currentSeason.Year == null)
            {
                Console.WriteLine("No current season available");
                return;
            }

            var firstLeague = leagues.FirstOrDefault();
            if (firstLeague == null)
            {
                Console.WriteLine("No leagues available");
                return;
            }

            Console.WriteLine($"\n=== Teams in {firstLeague.Name} ({currentSeason.Year}) ===");

            try
            {
                var (teams, errorMessage) = await apiClient.GetTeamsByLeagueAsync(
                    leagueId: firstLeague.Id,
                    season: currentSeason.Year.Value
                );

                if (errorMessage != null)
                {
                    Console.WriteLine($"API Error: {errorMessage}");
                    return;
                }

                if (teams != null)
                {
                    foreach (var team in teams.Take(10))
                    {
                        Console.WriteLine($"- {team.Name} ({team.Code})");
                    }
                }
            }
            catch (Exception ex)
            {
                Console.WriteLine($"Error fetching teams: {ex.Message}");
            }
        }

        /// <summary>
        /// Example: Get game statistics for a league across all seasons
        /// </summary>
        public static async Task Example_LeagueStatisticsAllSeasons(DataService dataService, int leagueId)
        {
            var league = await dataService.GetLeagueAsync(leagueId);
            if (league == null)
            {
                Console.WriteLine($"League {leagueId} not found");
                return;
            }

            var seasons = await dataService.GetSeasonsAsync();
            
            Console.WriteLine($"\n=== {league.Name} - Statistics by Season ===");
            
            foreach (var season in seasons)
            {
                var games = await dataService.GetGamesByLeagueAndSeasonAsync(leagueId, season.Year ?? 0);
                var completedGames = games.Where(g => g.Status == "FT" || g.Status == "AET").ToList();
                
                Console.WriteLine($"Season {season.Year}: {games.Count} total, {completedGames.Count} completed");
            }
        }

        /// <summary>
        /// Example: Compare leagues by game count
        /// </summary>
        public static async Task Example_CompareLeagueActivity(DataService dataService)
        {
            var leagues = await dataService.GetLeaguesAsync();
            var leagueStats = new List<(string Name, int GameCount)>();
            
            foreach (var league in leagues.Take(5))
            {
                var games = await dataService.GetGamesByLeagueAsync(league.Id);
                leagueStats.Add((league.Name, games.Count));
            }

            Console.WriteLine("\n=== League Activity Comparison ===");
            foreach (var (name, count) in leagueStats.OrderByDescending(x => x.GameCount))
            {
                Console.WriteLine($"{name}: {count} games");
            }
        }

        /// <summary>
        /// Example: Get season progression timeline
        /// </summary>
        public static async Task Example_SeasonTimeline(DataService dataService)
        {
            var currentSeason = await dataService.GetCurrentSeasonAsync();

            if (currentSeason == null)
            {
                Console.WriteLine("No current season");
                return;
            }

            var daysElapsed = DateTime.Now - currentSeason.StartDate;
            var totalDays = currentSeason.EndDate - currentSeason.StartDate;
            var percentProgress = totalDays.HasValue
                ? (daysElapsed.Value.TotalDays / totalDays.Value.TotalDays * 100)
                : 0;

            Console.WriteLine($"\n=== Season {currentSeason.Year} Progress ===");
            Console.WriteLine($"Start: {currentSeason.StartDate:yyyy-MM-dd}");
            Console.WriteLine($"End: {currentSeason.EndDate:yyyy-MM-dd}");
            Console.WriteLine($"Progress: {percentProgress:F1}%");
            Console.WriteLine($"Days remaining: {((totalDays - daysElapsed)?.TotalDays ?? 0):F0}");
        }

        /// <summary>
        /// Example: Find league by name (search)
        /// </summary>
        public static async Task Example_SearchLeague(DataService dataService, string searchTerm)
        {
            var leagues = await dataService.GetLeaguesAsync();
            var matches = leagues
                .Where(l => l.Name != null && l.Name.Contains(searchTerm, StringComparison.OrdinalIgnoreCase))
                .ToList();

            Console.WriteLine($"\n=== Search Results for '{searchTerm}' ===");
            if (matches.Count == 0)
            {
                Console.WriteLine("No leagues found");
                return;
            }

            foreach (var league in matches)
            {
                Console.WriteLine($"- {league.Name}");
                Console.WriteLine($"  Type: {league.Type}");
                Console.WriteLine($"  Country: {league.CountryName}");
            }
        }

        /// <summary>
        /// Example: Export league structure as CSV
        /// </summary>
        public static async Task Example_ExportLeagueStructure(DataService dataService, string filePath)
        {
            var leagues = await dataService.GetLeaguesAsync();

            using (var writer = new StreamWriter(filePath))
            {
                writer.WriteLine("LeagueId,LeagueName,Type,CountryName,CountryCode");
                
                foreach (var league in leagues)
                {
                    writer.WriteLine($"{league.Id},{league.Name},{league.Type},{league.CountryName},{league.CountryCode}");
                }
            }

            Console.WriteLine($"✓ Exported {leagues.Count} leagues to {filePath}");
        }

        /// <summary>
        /// Example: Get teams that haven't played recent games
        /// </summary>
        public static async Task Example_InactiveTeams(DataService dataService, int leagueId, int daysThreshold = 30)
        {
            var games = await dataService.GetGamesByLeagueAsync(leagueId);
            var recentGames = games
                .Where(g => (DateTime.UtcNow - g.Date?.ToUniversalTime()) < TimeSpan.FromDays(daysThreshold))
                .ToList();

            var activeTeamIds = new HashSet<int>();
            foreach (var game in recentGames)
            {
                activeTeamIds.Add(game.HomeTeamId);
                activeTeamIds.Add(game.AwayTeamId);
            }

            var allTeams = await dataService.GetTeamsAsync();
            var inactiveTeams = allTeams.Where(t => !activeTeamIds.Contains(t.Id)).ToList();

            Console.WriteLine($"\n=== Inactive Teams (no games in {daysThreshold} days) ===");
            foreach (var team in inactiveTeams.Take(10))
            {
                Console.WriteLine($"- {team.Name}");
            }
        }
    }
}
