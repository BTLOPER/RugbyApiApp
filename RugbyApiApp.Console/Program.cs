using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Configuration;
using RugbyApiApp.Data;
using RugbyApiApp.Extensions;
using RugbyApiApp.Services;

namespace RugbyApiApp.Console
{
    internal class Program
    {
        private static RugbyApiClient _apiClient = null!;
        private static DataService _dataService = null!;
        private static SecretsService _secretsService = null!;

        static async Task Main(string[] args)
        {
            // Configure console for Unicode support
            System.Console.OutputEncoding = System.Text.Encoding.UTF8;
            System.Console.InputEncoding = System.Text.Encoding.UTF8;

            System.Console.WriteLine("═════════════════════════════════════════════\n");
            System.Console.WriteLine("═══ Rugby API Console Application ═══\n");
            System.Console.WriteLine("═════════════════════════════════════════════\n");

            // Initialize database
            using (var context = new RugbyDbContext())
            {
                context.Database.EnsureCreated();
                System.Console.WriteLine("✓ Database initialized\n");
            }

            // Configure User Secrets
            var config = new ConfigurationBuilder()
                .AddUserSecrets<Program>()
                .Build();

            _secretsService = new SecretsService(config);

            // Get API key from User Secrets or environment variable
            string? apiKey = _secretsService.GetApiKey();
            if (string.IsNullOrEmpty(apiKey))
            {
                System.Console.WriteLine("ERROR: No API key found.");
                System.Console.WriteLine("Please set your api-sports.io API key using User Secrets:");
                System.Console.WriteLine("  dotnet user-secrets set RugbyApiKey \"your-api-key\"");
                System.Console.WriteLine("\nOr set the RUGBY_API_KEY environment variable.");
                return;
            }

            System.Console.WriteLine($"✓ API key loaded from: {GetKeySource(config)}\n");

            // Setup dependency injection
            var services = new ServiceCollection();
            services.AddRugbyApiServices(apiKey);

            var serviceProvider = services.BuildServiceProvider();
            _apiClient = serviceProvider.GetRequiredService<RugbyApiClient>();
            _dataService = serviceProvider.GetRequiredService<DataService>();

            try
            {
                await ShowMainMenuAsync();
            }
            catch (Exception ex)
            {
                System.Console.WriteLine($"ERROR: {ex.Message}");
                if (ex.InnerException != null)
                {
                    System.Console.WriteLine($"Details: {ex.InnerException.Message}");
                }
            }

            System.Console.WriteLine("\n═══ Application finished ═══");
        }

        static string GetKeySource(IConfiguration config)
        {
            if (!string.IsNullOrWhiteSpace(config["RugbyApiKey"]))
                return "User Secrets";
            return "Environment Variable";
        }

        static async Task ShowMainMenuAsync()
        {
            while (true)
            {
                System.Console.Clear();
                System.Console.WriteLine("╔════════════════════════════════════════════╗");
                System.Console.WriteLine("║   Rugby API Data Management System         ║");
                System.Console.WriteLine("╚════════════════════════════════════════════╝\n");

                // Show data statistics
                var stats = await _dataService.GetCompletionStatsAsync();
                System.Console.WriteLine("📊 Current Data Status:");
                System.Console.WriteLine($"  • Countries:  {stats.CompleteCountries:D3}/{stats.TotalCountries:D3} ({stats.CountryCompletionPercent:F1}%)");
                System.Console.WriteLine($"  • Seasons:    {stats.CompleteSeasons:D3}/{stats.TotalSeasons:D3} ({stats.SeasonCompletionPercent:F1}%)");
                System.Console.WriteLine($"  • Leagues:    {stats.CompleteLeagues:D3}/{stats.TotalLeagues:D3} ({stats.LeagueCompletionPercent:F1}%)");
                System.Console.WriteLine($"  • Teams:      {stats.CompleteTeams:D3}/{stats.TotalTeams:D3} ({stats.TeamCompletionPercent:F1}%)");
                System.Console.WriteLine($"  • Games:      {stats.CompleteGames:D3}/{stats.TotalGames:D3} ({stats.GameCompletionPercent:F1}%)\n");

                System.Console.WriteLine("📋 Main Menu:");
                System.Console.WriteLine("  [1] Browse & Fetch Countries");
                System.Console.WriteLine("  [2] Browse & Fetch Seasons");
                System.Console.WriteLine("  [3] Browse & Fetch Leagues");
                System.Console.WriteLine("  [4] Browse & Fetch Teams");
                System.Console.WriteLine("  [5] Browse & Fetch Games");
                System.Console.WriteLine("  [6] View All Stored Data");
                System.Console.WriteLine("  [7] Auto-Fetch All Incomplete Data");
                System.Console.WriteLine("  [8] Clear All Data");
                System.Console.WriteLine("  [0] Exit\n");

                System.Console.Write("Enter your choice (0-8): ");
                string? choice = System.Console.ReadLine();

                switch (choice)
                {
                    case "1":
                        await ShowCountriesMenuAsync();
                        break;
                    case "2":
                        await ShowSeasonsMenuAsync();
                        break;
                    case "3":
                        await ShowLeaguesMenuAsync();
                        break;
                    case "4":
                        await ShowTeamsMenuAsync();
                        break;
                    case "5":
                        await ShowGamesMenuAsync();
                        break;
                    case "6":
                        await DisplayStoredDataAsync(_dataService);
                        WaitForKeyPress();
                        break;
                    case "7":
                        await AutoFetchAllIncompleteAsync();
                        WaitForKeyPress();
                        break;
                    case "8":
                        await ClearAllDataAsync();
                        WaitForKeyPress();
                        break;
                    case "0":
                        return;
                    default:
                        System.Console.WriteLine("Invalid choice. Press any key to continue...");
                        System.Console.ReadKey();
                        break;
                }
            }
        }

        static async Task ShowCountriesMenuAsync()
        {
            while (true)
            {
                System.Console.Clear();
                System.Console.WriteLine("╔════════════════════════════════════════════╗");
                System.Console.WriteLine("║         Countries Management               ║");
                System.Console.WriteLine("╚════════════════════════════════════════════╝\n");

                var countries = await _dataService.GetCountriesAsync();
                var incompleteCountries = await _dataService.GetIncompleteCountriesAsync();

                System.Console.WriteLine($"📍 Countries: {countries.Count} total");
                System.Console.WriteLine($"   ✓ Complete: {countries.Count(c => c.IsDataComplete)}");
                System.Console.WriteLine($"   ⚠ Incomplete: {incompleteCountries.Count}\n");

                if (countries.Any())
                {
                    System.Console.WriteLine("📋 Countries in Database:");
                    int index = 1;
                    foreach (var country in countries.Take(10))
                    {
                        var status = country.IsDataComplete ? "✓" : "⚠";
                        System.Console.WriteLine($"  {status} {country.Name} ({country.Code})");
                        index++;
                    }
                    if (countries.Count > 10)
                        System.Console.WriteLine($"  ... and {countries.Count - 10} more");
                }

                System.Console.WriteLine("\n◈ Menu:");
                System.Console.WriteLine("  [1] Fetch Countries from API");
                System.Console.WriteLine("  [2] View All Countries (paginated)");
                System.Console.WriteLine("  [0] Back to Main Menu\n");

                System.Console.Write("Enter your choice (0-2): ");
                string? choice = System.Console.ReadLine();

                switch (choice)
                {
                    case "1":
                        await FetchAndStoreCountriesAsync();
                        WaitForKeyPress();
                        break;
                    case "2":
                        await ViewAllCountriesAsync();
                        break;
                    case "0":
                        return;
                    default:
                        System.Console.WriteLine("Invalid choice. Press any key to continue...");
                        System.Console.ReadKey();
                        break;
                }
            }
        }

        static async Task ShowSeasonsMenuAsync()
        {
            while (true)
            {
                System.Console.Clear();
                System.Console.WriteLine("╔════════════════════════════════════════════╗");
                System.Console.WriteLine("║         Seasons Management                 ║");
                System.Console.WriteLine("╚════════════════════════════════════════════╝\n");

                var seasons = await _dataService.GetSeasonsAsync();
                var incompleteSeasons = await _dataService.GetIncompleteSeasonAsync();

                System.Console.WriteLine($"📅 Seasons: {seasons.Count} total");
                System.Console.WriteLine($"   ✓ Complete: {seasons.Count(s => s.IsDataComplete)}");
                System.Console.WriteLine($"   ⚠ Incomplete: {incompleteSeasons.Count}\n");

                if (seasons.Any())
                {
                    System.Console.WriteLine("📋 Seasons in Database:");
                    foreach (var season in seasons.Take(10))
                    {
                        var status = season.IsDataComplete ? "✓" : "⚠";
                        var current = season.IsCurrent ? " (CURRENT)" : "";
                        System.Console.WriteLine($"  {status} Season {season.Year}{current}");
                    }
                    if (seasons.Count > 10)
                        System.Console.WriteLine($"  ... and {seasons.Count - 10} more");
                }

                System.Console.WriteLine("\n◈ Menu:");
                System.Console.WriteLine("  [1] Fetch Seasons from API");
                System.Console.WriteLine("  [2] View All Seasons");
                System.Console.WriteLine("  [0] Back to Main Menu\n");

                System.Console.Write("Enter your choice (0-2): ");
                string? choice = System.Console.ReadLine();

                switch (choice)
                {
                    case "1":
                        await FetchAndStoreSeasonAsync();
                        WaitForKeyPress();
                        break;
                    case "2":
                        await ViewAllSeasonsAsync();
                        break;
                    case "0":
                        return;
                    default:
                        System.Console.WriteLine("Invalid choice. Press any key to continue...");
                        System.Console.ReadKey();
                        break;
                }
            }
        }

        static async Task ShowLeaguesMenuAsync()
        {
            while (true)
            {
                System.Console.Clear();
                System.Console.WriteLine("╔════════════════════════════════════════════╗");
                System.Console.WriteLine("║         Leagues Management                 ║");
                System.Console.WriteLine("╚════════════════════════════════════════════╝\n");

                var leagues = await _dataService.GetLeaguesAsync();
                var incompleteLeagues = await _dataService.GetIncompleteLeaguesAsync();

                System.Console.WriteLine($"🏆 Leagues: {leagues.Count} total");
                System.Console.WriteLine($"   ✓ Complete: {leagues.Count(l => l.IsDataComplete)}");
                System.Console.WriteLine($"   ⚠ Incomplete: {incompleteLeagues.Count}\n");

                if (leagues.Any())
                {
                    System.Console.WriteLine("📋 Leagues in Database:");
                    foreach (var league in leagues.Take(10))
                    {
                        var status = league.IsDataComplete ? "✓" : "⚠";
                        System.Console.WriteLine($"  {status} {league.Name} ({league.CountryCode})");
                    }
                    if (leagues.Count > 10)
                        System.Console.WriteLine($"  ... and {leagues.Count - 10} more");
                }

                System.Console.WriteLine("\n◈ Menu:");
                System.Console.WriteLine("  [1] Fetch Leagues from API");
                System.Console.WriteLine("  [2] View All Leagues (paginated)");
                System.Console.WriteLine("  [0] Back to Main Menu\n");

                System.Console.Write("Enter your choice (0-2): ");
                string? choice = System.Console.ReadLine();

                switch (choice)
                {
                    case "1":
                        await FetchAndStoreLeaguesAsync();
                        WaitForKeyPress();
                        break;
                    case "2":
                        await ViewAllLeaguesAsync();
                        break;
                    case "0":
                        return;
                    default:
                        System.Console.WriteLine("Invalid choice. Press any key to continue...");
                        System.Console.ReadKey();
                        break;
                }
            }
        }

        static async Task ShowTeamsMenuAsync()
        {
            while (true)
            {
                System.Console.Clear();
                System.Console.WriteLine("╔════════════════════════════════════════════╗");
                System.Console.WriteLine("║         Teams Management                   ║");
                System.Console.WriteLine("╚════════════════════════════════════════════╝\n");

                var teams = await _dataService.GetTeamsAsync();
                var incompleteTeams = await _dataService.GetIncompleteTeamsAsync();

                System.Console.WriteLine($"🏉 Teams: {teams.Count} total");
                System.Console.WriteLine($"   ✓ Complete: {teams.Count(t => t.IsDataComplete)}");
                System.Console.WriteLine($"   ⚠ Incomplete: {incompleteTeams.Count}\n");

                if (incompleteTeams.Any())
                {
                    System.Console.WriteLine("⚠️ Teams Incomplete:");
                    foreach (var team in incompleteTeams.Take(10))
                    {
                        System.Console.WriteLine($"  • {team.Name} ({team.Code})");
                    }
                    if (incompleteTeams.Count > 10)
                        System.Console.WriteLine($"  ... and {incompleteTeams.Count - 10} more");
                    System.Console.WriteLine();
                }

                if (teams.Any())
                {
                    System.Console.WriteLine("📋 Recent Teams:");
                    foreach (var team in teams.Take(5))
                    {
                        var status = team.IsDataComplete ? "✓" : "⚠";
                        System.Console.WriteLine($"  {status} {team.Name} ({team.Code})");
                    }
                    if (teams.Count > 5)
                        System.Console.WriteLine($"  ... and {teams.Count - 5} more");
                }

                System.Console.WriteLine("\n◈ Menu:");
                System.Console.WriteLine("  [1] View All Teams (paginated)");
                System.Console.WriteLine("  [0] Back to Main Menu\n");

                System.Console.Write("Enter your choice (0-1): ");
                string? choice = System.Console.ReadLine();

                switch (choice)
                {
                    case "1":
                        await ViewAllTeamsAsync();
                        break;
                    case "0":
                        return;
                    default:
                        System.Console.WriteLine("Invalid choice. Press any key to continue...");
                        System.Console.ReadKey();
                        break;
                }
            }
        }

        static async Task ShowGamesMenuAsync()
        {
            while (true)
            {
                System.Console.Clear();
                System.Console.WriteLine("╔════════════════════════════════════════════╗");
                System.Console.WriteLine("║         Games Management                   ║");
                System.Console.WriteLine("╚════════════════════════════════════════════╝\n");

                var games = await _dataService.GetGamesAsync();
                var incompleteGames = await _dataService.GetIncompleteGamesAsync();

                System.Console.WriteLine($"🎮 Games: {games.Count} total");
                System.Console.WriteLine($"   ✓ Complete: {games.Count(g => g.IsDataComplete)}");
                System.Console.WriteLine($"   ⚠ Incomplete: {incompleteGames.Count}\n");

                if (games.Any())
                {
                    System.Console.WriteLine("📋 Recent Games:");
                    foreach (var game in games.TakeLast(10))
                    {
                        var homeTeam = game.HomeTeam?.Name ?? "Unknown";
                        var awayTeam = game.AwayTeam?.Name ?? "Unknown";
                        var location = game.Venue ?? "TBD";
                        var date = game.Date?.ToString("yyyy-MM-dd") ?? "TBD";
                        System.Console.WriteLine($"  • {date} | {homeTeam} vs {awayTeam} @ {location}");
                    }
                }

                System.Console.WriteLine("\n◈ Menu:");
                System.Console.WriteLine("  [1] Fetch Games for League & Year Range");
                System.Console.WriteLine("  [2] View All Games (paginated)");
                System.Console.WriteLine("  [0] Back to Main Menu\n");

                System.Console.Write("Enter your choice (0-2): ");
                string? choice = System.Console.ReadLine();

                switch (choice)
                {
                    case "1":
                        await FetchGamesForLeagueAndYearsAsync();
                        WaitForKeyPress();
                        break;
                    case "2":
                        await ViewAllGamesAsync();
                        break;
                    case "0":
                        return;
                    default:
                        System.Console.WriteLine("Invalid choice. Press any key to continue...");
                        System.Console.ReadKey();
                        break;
                }
            }
        }

        // ... (Rest of the methods from the original Program.cs - all the FetchAndStore, ViewAll, etc. methods)
        // For brevity, I'll include key methods:

        static async Task ViewAllCountriesAsync()
        {
            System.Console.Clear();
            var countries = await _dataService.GetCountriesAsync();
            int pageSize = 10;
            int totalPages = (countries.Count + pageSize - 1) / pageSize;
            int currentPage = 0;

            while (true)
            {
                System.Console.Clear();
                System.Console.WriteLine($"╔════════════════════════════════════════════╗");
                System.Console.WriteLine($"║       Countries (Page {currentPage + 1}/{totalPages})                     ║");
                System.Console.WriteLine($"╚════════════════════════════════════════════╝\n");

                var pageItems = countries.Skip(currentPage * pageSize).Take(pageSize).ToList();
                foreach (var country in pageItems)
                {
                    var status = country.IsDataComplete ? "✓" : "⚠";
                    System.Console.WriteLine($"  {status} {country.Name,-30} ({country.Code})");
                }

                System.Console.WriteLine($"\nShowing {pageItems.Count} of {countries.Count} countries");
                System.Console.WriteLine("\n[N] Next | [P] Previous | [0] Back");
                System.Console.Write("Enter choice: ");
                string? choice = System.Console.ReadLine()?.ToUpper();

                if (choice == "0") break;
                if (choice == "N" && currentPage < totalPages - 1) currentPage++;
                if (choice == "P" && currentPage > 0) currentPage--;
            }
        }

        static async Task ViewAllSeasonsAsync()
        {
            System.Console.Clear();
            var seasons = await _dataService.GetSeasonsAsync();

            System.Console.WriteLine("╔════════════════════════════════════════════╗");
            System.Console.WriteLine("║          All Seasons                       ║");
            System.Console.WriteLine("╚════════════════════════════════════════════╝\n");

            foreach (var season in seasons)
            {
                var status = season.IsDataComplete ? "✓" : "⚠";
                var current = season.IsCurrent ? " (CURRENT)" : "";
                var dates = season.StartDate.HasValue && season.EndDate.HasValue
                    ? $" {season.StartDate:yyyy-MM-dd} to {season.EndDate:yyyy-MM-dd}"
                    : "";
                System.Console.WriteLine($"  {status} Season {season.Year}{current}{dates}");
            }

            System.Console.WriteLine("\nPress any key to continue...");
            System.Console.ReadKey();
        }

        static async Task ViewAllLeaguesAsync()
        {
            System.Console.Clear();
            var leagues = await _dataService.GetLeaguesAsync();
            int pageSize = 10;
            int totalPages = (leagues.Count + pageSize - 1) / pageSize;
            int currentPage = 0;

            while (true)
            {
                System.Console.Clear();
                System.Console.WriteLine($"╔════════════════════════════════════════════╗");
                System.Console.WriteLine($"║       Leagues (Page {currentPage + 1}/{totalPages})                      ║");
                System.Console.WriteLine($"╚════════════════════════════════════════════╝\n");

                var pageItems = leagues.Skip(currentPage * pageSize).Take(pageSize).ToList();
                foreach (var league in pageItems)
                {
                    var status = league.IsDataComplete ? "✓" : "⚠";
                    System.Console.WriteLine($"  {status} {league.Name,-30} ({league.CountryCode})");
                }

                System.Console.WriteLine($"\nShowing {pageItems.Count} of {leagues.Count} leagues");
                System.Console.WriteLine("\n[N] Next | [P] Previous | [0] Back");
                System.Console.Write("Enter choice: ");
                string? choice = System.Console.ReadLine()?.ToUpper();

                if (choice == "0") break;
                if (choice == "N" && currentPage < totalPages - 1) currentPage++;
                if (choice == "P" && currentPage > 0) currentPage--;
            }
        }

        static async Task ViewAllTeamsAsync()
        {
            System.Console.Clear();
            var teams = await _dataService.GetTeamsAsync();
            int pageSize = 10;
            int totalPages = (teams.Count + pageSize - 1) / pageSize;
            int currentPage = 0;

            while (true)
            {
                System.Console.Clear();
                System.Console.WriteLine($"╔════════════════════════════════════════════╗");
                System.Console.WriteLine($"║       Teams (Page {currentPage + 1}/{totalPages})                        ║");
                System.Console.WriteLine($"╚════════════════════════════════════════════╝\n");

                var pageItems = teams.Skip(currentPage * pageSize).Take(pageSize).ToList();
                foreach (var team in pageItems)
                {
                    var status = team.IsDataComplete ? "✓" : "⚠";
                    System.Console.WriteLine($"  {status} {team.Name,-25} ({team.Code})");
                }

                System.Console.WriteLine($"\nShowing {pageItems.Count} of {teams.Count} teams");
                System.Console.WriteLine("\n[N] Next | [P] Previous | [0] Back");
                System.Console.Write("Enter choice: ");
                string? choice = System.Console.ReadLine()?.ToUpper();

                if (choice == "0") break;
                if (choice == "N" && currentPage < totalPages - 1) currentPage++;
                if (choice == "P" && currentPage > 0) currentPage--;
            }
        }

        static async Task ViewAllGamesAsync()
        {
            System.Console.Clear();
            var games = await _dataService.GetGamesAsync();
            int pageSize = 10;
            int totalPages = (games.Count + pageSize - 1) / pageSize;
            int currentPage = 0;

            while (true)
            {
                System.Console.Clear();
                System.Console.WriteLine($"╔════════════════════════════════════════════╗");
                System.Console.WriteLine($"║       Games (Page {currentPage + 1}/{totalPages})                        ║");
                System.Console.WriteLine($"╚════════════════════════════════════════════╝\n");

                var pageItems = games.Skip(currentPage * pageSize).Take(pageSize).ToList();
                foreach (var game in pageItems)
                {
                    var homeTeam = game.HomeTeam?.Name ?? "Unknown";
                    var awayTeam = game.AwayTeam?.Name ?? "Unknown";
                    var location = game.Venue ?? "TBD";
                    var date = game.Date?.ToString("yyyy-MM-dd") ?? "TBD";
                    System.Console.WriteLine($"  {date} | {homeTeam,-18} vs {awayTeam,-18} @ {location}");
                }

                System.Console.WriteLine($"\nShowing {pageItems.Count} of {games.Count} games");
                System.Console.WriteLine("\n[N] Next | [P] Previous | [0] Back");
                System.Console.Write("Enter choice: ");
                string? choice = System.Console.ReadLine()?.ToUpper();

                if (choice == "0") break;
                if (choice == "N" && currentPage < totalPages - 1) currentPage++;
                if (choice == "P" && currentPage > 0) currentPage--;
            }
        }

        static async Task FetchGamesForLeagueAndYearsAsync()
        {
            System.Console.Clear();
            System.Console.WriteLine("╔════════════════════════════════════════════╗");
            System.Console.WriteLine("║    Fetch Games for League & Year Range     ║");
            System.Console.WriteLine("╚════════════════════════════════════════════╝\n");

            var leagues = await _dataService.GetLeaguesAsync();
            if (!leagues.Any())
            {
                System.Console.WriteLine("⚠ No leagues in database. Fetch leagues first.\n");
                return;
            }

            System.Console.WriteLine("Available Leagues:");
            var leagueList = leagues.OrderBy(l => l.Name).ToList();
            for (int i = 0; i < leagueList.Count; i++)
            {
                var league = leagueList[i];
                System.Console.WriteLine($"  [{i + 1}] {league.Name} ({league.CountryCode})");
            }

            System.Console.Write("\nSelect league (or 0 to cancel): ");
            if (!int.TryParse(System.Console.ReadLine(), out int leagueChoice) || leagueChoice == 0)
                return;

            if (leagueChoice < 1 || leagueChoice > leagueList.Count)
            {
                System.Console.WriteLine("Invalid choice.");
                return;
            }

            var selectedLeague = leagueList[leagueChoice - 1];

            var seasons = await _dataService.GetSeasonsAsync();
            if (!seasons.Any())
            {
                System.Console.WriteLine("⚠ No seasons in database. Fetch seasons first.\n");
                return;
            }

            var yearList = seasons.Where(s => s.Year.HasValue).OrderByDescending(s => s.Year).Select(s => s.Year!.Value).Distinct().ToList();

            System.Console.WriteLine("\nAvailable Years:");
            for (int i = 0; i < yearList.Count; i++)
            {
                System.Console.WriteLine($"  [{i + 1}] {yearList[i]}");
            }

            System.Console.Write("\nEnter starting year index (or 0 to cancel): ");
            if (!int.TryParse(System.Console.ReadLine(), out int startYearChoice) || startYearChoice == 0)
                return;

            if (startYearChoice < 1 || startYearChoice > yearList.Count)
            {
                System.Console.WriteLine("Invalid choice.");
                return;
            }

            int startYear = yearList[startYearChoice - 1];

            System.Console.Write("Enter ending year index (or press Enter for same year): ");
            string? endYearInput = System.Console.ReadLine();
            int endYear = startYear;

            if (!string.IsNullOrWhiteSpace(endYearInput))
            {
                if (!int.TryParse(endYearInput, out int endYearChoice) || endYearChoice == 0)
                {
                    System.Console.WriteLine("Invalid choice.");
                    return;
                }

                if (endYearChoice < 1 || endYearChoice > yearList.Count)
                {
                    System.Console.WriteLine("Invalid choice.");
                    return;
                }

                endYear = yearList[endYearChoice - 1];

                if (endYear < startYear)
                {
                    int temp = startYear;
                    startYear = endYear;
                    endYear = temp;
                }
            }

            System.Console.WriteLine($"\nFetching games for {selectedLeague.Name} from {startYear} to {endYear}...\n");

            await FetchAndStoreGamesForLeagueAndYearsAsync(_apiClient, _dataService, selectedLeague, startYear, endYear);
        }

        static async Task FetchAndStoreCountriesAsync()
        {
            System.Console.WriteLine("Fetching countries from API...");
            try
            {
                var incompleteCountries = await _dataService.GetIncompleteCountriesAsync();
                if (incompleteCountries.Count > 0)
                {
                    System.Console.WriteLine($"Found {incompleteCountries.Count} incomplete countries, skipping API call\n");
                    return;
                }

                var (countries, errorMessage) = await _apiClient.GetCountriesAsync();

                if (errorMessage != null)
                {
                    System.Console.WriteLine($"⚠ API Error: {errorMessage}\n");
                    return;
                }

                if (countries != null && countries.Count > 0)
                {
                    System.Console.WriteLine($"Retrieved {countries.Count} countries");

                    foreach (var countryResponse in countries)
                    {
                        if (countryResponse.Id.HasValue && countryResponse.Name != null)
                        {
                            var cdnFlag = RugbyApiClient.ConvertMediaUrlToCdn(countryResponse.Flag);

                            await _dataService.UpsertCountryAsync(
                                countryResponse.Id.Value,
                                countryResponse.Name,
                                countryResponse.Code,
                                cdnFlag
                            );
                        }
                    }

                    System.Console.WriteLine($"✓ Stored {countries.Count} countries in database\n");
                }
                else
                {
                    System.Console.WriteLine("⚠ No countries retrieved from API\n");
                }
            }
            catch (HttpRequestException ex)
            {
                System.Console.WriteLine($"⚠ Error fetching countries: {ex.Message}\n");
            }
        }

        static async Task FetchAndStoreSeasonAsync()
        {
            System.Console.WriteLine("Fetching seasons from API...");
            try
            {
                var incompleteSessions = await _dataService.GetIncompleteSeasonAsync();
                if (incompleteSessions.Count > 0)
                {
                    System.Console.WriteLine($"Found {incompleteSessions.Count} incomplete seasons, skipping API call\n");
                    return;
                }

                var (seasonYears, errorMessage) = await _apiClient.GetSeasonsAsync();

                if (errorMessage != null)
                {
                    System.Console.WriteLine($"⚠ API Error: {errorMessage}\n");
                    return;
                }

                if (seasonYears != null && seasonYears.Count > 0)
                {
                    System.Console.WriteLine($"Retrieved {seasonYears.Count} seasons");

                    foreach (var year in seasonYears)
                    {
                        await _dataService.UpsertSeasonAsync(
                            seasonId: year,
                            year: year,
                            startDate: null,
                            endDate: null,
                            isCurrent: false
                        );
                    }

                    System.Console.WriteLine($"✓ Stored {seasonYears.Count} seasons in database\n");
                }
                else
                {
                    System.Console.WriteLine("⚠ No seasons retrieved from API\n");
                }
            }
            catch (HttpRequestException ex)
            {
                System.Console.WriteLine($"⚠ Error fetching seasons: {ex.Message}\n");
            }
            catch (Exception ex)
            {
                System.Console.WriteLine($"⚠ Error processing seasons: {ex.Message}\n");
            }
        }

        static async Task FetchAndStoreLeaguesAsync()
        {
            System.Console.WriteLine("Fetching leagues from API...");
            try
            {
                var incompleteLeagues = await _dataService.GetIncompleteLeaguesAsync();
                if (incompleteLeagues.Count > 0)
                {
                    System.Console.WriteLine($"Found {incompleteLeagues.Count} incomplete leagues, skipping API call\n");
                    return;
                }

                var (leagues, errorMessage) = await _apiClient.GetLeaguesAsync();

                if (errorMessage != null)
                {
                    System.Console.WriteLine($"⚠ API Error: {errorMessage}\n");
                    return;
                }

                if (leagues != null && leagues.Count > 0)
                {
                    System.Console.WriteLine($"Retrieved {leagues.Count} leagues");

                    foreach (var leagueResponse in leagues)
                    {
                        if (leagueResponse.Id.HasValue && leagueResponse.Name != null)
                        {
                            var cdnLogo = RugbyApiClient.ConvertMediaUrlToCdn(leagueResponse.Logo);
                            var cdnFlag = RugbyApiClient.ConvertMediaUrlToCdn(leagueResponse.Country?.Flag);

                            await _dataService.UpsertLeagueAsync(
                                leagueResponse.Id.Value,
                                leagueResponse.Name,
                                leagueResponse.Type,
                                cdnLogo,
                                leagueResponse.Country?.Name,
                                leagueResponse.Country?.Code,
                                cdnFlag
                            );
                        }
                    }

                    System.Console.WriteLine($"✓ Stored {leagues.Count} leagues in database\n");
                }
                else
                {
                    System.Console.WriteLine("⚠ No leagues retrieved from API\n");
                }
            }
            catch (HttpRequestException ex)
            {
                System.Console.WriteLine($"⚠ Error fetching leagues: {ex.Message}\n");
            }
        }

        static async Task FetchAndStoreGamesForLeagueAndYearsAsync(RugbyApiClient apiClient, DataService dataService, RugbyApiApp.Models.League league, int startYear, int endYear)
        {
            System.Console.WriteLine($"Fetching games for {league.Name} from {startYear} to {endYear}...\n");

            try
            {
                var (games, errorMessage) = await apiClient.GetGamesByLeagueAndSeasonsAsync(league.Id, startYear, endYear);
                
                if (errorMessage != null)
                {
                    System.Console.WriteLine($"⚠ API Error: {errorMessage}\n");
                }

                if (games != null && games.Count > 0)
                {
                    System.Console.WriteLine($"Retrieved {games.Count} games\n");
                    
                    int newTeamsCreated = 0;
                    int gamesStored = 0;

                    foreach (var gameResponse in games)
                    {
                        if (gameResponse.Id.HasValue &&
                            gameResponse.Home?.Id.HasValue == true &&
                            gameResponse.Away?.Id.HasValue == true)
                        {
                            if (!await dataService.TeamExistsAsync(gameResponse.Home.Id.Value))
                            {
                                if (gameResponse.Home.Name != null)
                                {
                                    var cdnLogo = RugbyApiClient.ConvertMediaUrlToCdn(gameResponse.Home.Logo);
                                    var cdnFlag = RugbyApiClient.ConvertMediaUrlToCdn(gameResponse.Home.Flag);
                                    await dataService.UpsertTeamAsync(
                                        gameResponse.Home.Id.Value,
                                        gameResponse.Home.Name,
                                        gameResponse.Home.Code,
                                        cdnFlag,
                                        cdnLogo
                                    );
                                    newTeamsCreated++;
                                }
                            }

                            if (!await dataService.TeamExistsAsync(gameResponse.Away.Id.Value))
                            {
                                if (gameResponse.Away.Name != null)
                                {
                                    var cdnLogo = RugbyApiClient.ConvertMediaUrlToCdn(gameResponse.Away.Logo);
                                    var cdnFlag = RugbyApiClient.ConvertMediaUrlToCdn(gameResponse.Away.Flag);
                                    await dataService.UpsertTeamAsync(
                                        gameResponse.Away.Id.Value,
                                        gameResponse.Away.Name,
                                        gameResponse.Away.Code,
                                        cdnFlag,
                                        cdnLogo
                                    );
                                    newTeamsCreated++;
                                }
                            }

                            await dataService.UpsertGameAsync(
                                gameResponse.Id.Value,
                                gameResponse.LeagueId,
                                gameResponse.Season,
                                gameResponse.Home.Id.Value,
                                gameResponse.Away.Id.Value,
                                gameResponse.Date,
                                gameResponse.Status?.Short,
                                gameResponse.Venue,
                                gameResponse.Scores?.Home,
                                gameResponse.Scores?.Away
                            );
                            gamesStored++;
                        }
                    }

                    System.Console.WriteLine($"✓ Stored {gamesStored} games");
                    System.Console.WriteLine($"✓ Auto-created {newTeamsCreated} new teams\n");
                }
                else
                {
                    System.Console.WriteLine("⚠ No games retrieved from API\n");
                }
            }
            catch (HttpRequestException ex)
            {
                System.Console.WriteLine($"⚠ Error fetching games: {ex.Message}\n");
            }
            catch (Exception ex)
            {
                System.Console.WriteLine($"⚠ Error processing games: {ex.Message}\n");
            }
        }

        static async Task DisplayStoredDataAsync(DataService dataService)
        {
            System.Console.WriteLine("═══ Stored Data ═══\n");

            var seasons = await dataService.GetSeasonsAsync();
            System.Console.WriteLine($"Seasons in database: {seasons.Count}");
            foreach (var season in seasons.Take(3))
            {
                System.Console.WriteLine($"  - Season {season.Year} ({(season.IsCurrent ? "Current" : "Past")})");
            }

            var countries = await dataService.GetCountriesAsync();
            System.Console.WriteLine($"\nCountries in database: {countries.Count}");
            foreach (var country in countries.Take(5))
            {
                var completeness = country.IsDataComplete ? "✓" : "⚠";
                System.Console.WriteLine($"  {completeness} {country.Name} ({country.Code})");
            }

            var leagues = await dataService.GetLeaguesAsync();
            System.Console.WriteLine($"\nLeagues in database: {leagues.Count}");
            foreach (var league in leagues.Take(5))
            {
                System.Console.WriteLine($"  - {league.Name} ({league.CountryCode})");
            }

            var teams = await dataService.GetTeamsAsync();
            System.Console.WriteLine($"\nTeams in database: {teams.Count}");
            foreach (var team in teams.Take(5))
            {
                var completeness = team.IsDataComplete ? "✓" : "⚠";
                System.Console.WriteLine($"  {completeness} {team.Name} ({team.Code})");
            }

            var games = await dataService.GetGamesAsync();
            System.Console.WriteLine($"\nGames in database: {games.Count}");
            foreach (var game in games.Take(5))
            {
                var homeTeam = game.HomeTeam?.Name ?? "Unknown";
                var awayTeam = game.AwayTeam?.Name ?? "Unknown";
                var location = game.Venue ?? "TBD";
                var date = game.Date?.ToString("yyyy-MM-dd") ?? "TBD";
                System.Console.WriteLine($"  - {date} | {homeTeam} vs {awayTeam} @ {location}");
            }

            var stats = await dataService.GetCompletionStatsAsync();
            System.Console.WriteLine(stats);
        }

        static async Task AutoFetchAllIncompleteAsync()
        {
            System.Console.Clear();
            System.Console.WriteLine("╔════════════════════════════════════════════╗");
            System.Console.WriteLine("║    Auto-Fetching All Incomplete Data       ║");
            System.Console.WriteLine("╚════════════════════════════════════════════╝\n");

            System.Console.WriteLine("→ Fetching Countries...");
            await FetchAndStoreCountriesAsync();

            System.Console.WriteLine("→ Fetching Seasons...");
            await FetchAndStoreSeasonAsync();

            System.Console.WriteLine("→ Fetching Leagues...");
            await FetchAndStoreLeaguesAsync();

            System.Console.WriteLine("→ Fetching Games and Teams...");
            await FetchAllGamesAndTeamsAsync();

            System.Console.WriteLine("✓ Auto-fetch complete!");
        }

        static async Task FetchAllGamesAndTeamsAsync()
        {
            var leagues = await _dataService.GetLeaguesAsync();
            if (!leagues.Any())
            {
                System.Console.WriteLine("⚠ No leagues in database. Skipping games and teams fetch.\n");
                return;
            }

            var seasons = await _dataService.GetSeasonsAsync();
            if (!seasons.Any())
            {
                System.Console.WriteLine("⚠ No seasons in database. Skipping games and teams fetch.\n");
                return;
            }

            var yearList = seasons
                .Where(s => s.Year.HasValue)
                .OrderByDescending(s => s.Year)
                .Select(s => s.Year!.Value)
                .Distinct()
                .ToList();

            if (!yearList.Any())
            {
                System.Console.WriteLine("⚠ No valid seasons found. Skipping games and teams fetch.\n");
                return;
            }

            int startYear = yearList.Last();
            int endYear = yearList.First();

            System.Console.WriteLine($"Fetching games for {leagues.Count} leagues from {startYear} to {endYear}...\n");

            int totalLeaguesProcessed = 0;
            int totalGames = 0;
            int totalTeams = 0;

            foreach (var league in leagues)
            {
                try
                {
                    var (games, errorMessage) = await _apiClient.GetGamesByLeagueAndSeasonsAsync(league.Id, startYear, endYear);

                    if (errorMessage != null)
                    {
                        System.Console.WriteLine($"⚠ {league.Name}: API Error - {errorMessage}");
                        continue;
                    }

                    if (games != null && games.Count > 0)
                    {
                        int leagueGames = 0;
                        int leagueTeams = 0;

                        foreach (var gameResponse in games)
                        {
                            if (gameResponse.Id.HasValue &&
                                gameResponse.Home?.Id.HasValue == true &&
                                gameResponse.Away?.Id.HasValue == true)
                            {
                                if (!await _dataService.TeamExistsAsync(gameResponse.Home.Id.Value))
                                {
                                    if (gameResponse.Home.Name != null)
                                    {
                                        var cdnLogo = RugbyApiClient.ConvertMediaUrlToCdn(gameResponse.Home.Logo);
                                        var cdnFlag = RugbyApiClient.ConvertMediaUrlToCdn(gameResponse.Home.Flag);
                                        await _dataService.UpsertTeamAsync(
                                            gameResponse.Home.Id.Value,
                                            gameResponse.Home.Name,
                                            gameResponse.Home.Code,
                                            cdnFlag,
                                            cdnLogo
                                        );
                                        leagueTeams++;
                                    }
                                }

                                if (!await _dataService.TeamExistsAsync(gameResponse.Away.Id.Value))
                                {
                                    if (gameResponse.Away.Name != null)
                                    {
                                        var cdnLogo = RugbyApiClient.ConvertMediaUrlToCdn(gameResponse.Away.Logo);
                                        var cdnFlag = RugbyApiClient.ConvertMediaUrlToCdn(gameResponse.Away.Flag);
                                        await _dataService.UpsertTeamAsync(
                                            gameResponse.Away.Id.Value,
                                            gameResponse.Away.Name,
                                            gameResponse.Away.Code,
                                            cdnFlag,
                                            cdnLogo
                                        );
                                        leagueTeams++;
                                    }
                                }

                                await _dataService.UpsertGameAsync(
                                    gameResponse.Id.Value,
                                    gameResponse.LeagueId,
                                    gameResponse.Season,
                                    gameResponse.Home.Id.Value,
                                    gameResponse.Away.Id.Value,
                                    gameResponse.Date,
                                    gameResponse.Status?.Short,
                                    gameResponse.Venue,
                                    gameResponse.Scores?.Home,
                                    gameResponse.Scores?.Away
                                );
                                leagueGames++;
                            }
                        }

                        System.Console.WriteLine($"  ✓ {league.Name}: {leagueGames} games, {leagueTeams} teams");
                        totalGames += leagueGames;
                        totalTeams += leagueTeams;
                        totalLeaguesProcessed++;
                    }
                    else
                    {
                        System.Console.WriteLine($"  ℹ {league.Name}: No games found");
                    }
                }
                catch (HttpRequestException ex)
                {
                    System.Console.WriteLine($"  ✗ {league.Name}: {ex.Message}");
                }
                catch (Exception ex)
                {
                    System.Console.WriteLine($"  ✗ {league.Name}: {ex.Message}");
                }
            }

            System.Console.WriteLine($"\n✓ Games & Teams: {totalGames} games from {totalLeaguesProcessed} leagues\n");
            System.Console.WriteLine($"✓ New teams created: {totalTeams}\n");
        }

        static async Task ClearAllDataAsync()
        {
            System.Console.Clear();
            System.Console.WriteLine("╔════════════════════════════════════════════╗");
            System.Console.WriteLine("║         Clear All Data                     ║");
            System.Console.WriteLine("╚════════════════════════════════════════════╝\n");

            System.Console.WriteLine("⚠ This will delete ALL data from the database.");
            System.Console.Write("Are you sure? (yes/no): ");
            string? confirm = System.Console.ReadLine()?.ToLower();

            if (confirm == "yes")
            {
                await _dataService.ClearAllDataAsync();
                System.Console.WriteLine("✓ All data cleared successfully.\n");
            }
            else
            {
                System.Console.WriteLine("✗ Clear operation cancelled.\n");
            }
        }

        static void WaitForKeyPress()
        {
            System.Console.WriteLine("\nPress any key to continue...");
            System.Console.ReadKey();
        }
    }
}
