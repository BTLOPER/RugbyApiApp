using RugbyApiApp.Models;

namespace RugbyApiApp.Extensions
{
    public static class RugbyDataExtensions
    {
        /// <summary>
        /// Check if a team has been fetched recently
        /// </summary>
        public static bool IsRecentlyFetched(this Team team, TimeSpan? threshold = null)
        {
            threshold ??= TimeSpan.FromHours(24);
            return DateTime.UtcNow - team.UpdatedAt < threshold;
        }

        /// <summary>
        /// Get upcoming games
        /// </summary>
        public static List<Game> GetUpcomingGames(this IEnumerable<Game> games)
        {
            return games
                .Where(g => g.Date > DateTime.UtcNow && g.Status == "NS")
                .OrderBy(g => g.Date)
                .ToList();
        }

        /// <summary>
        /// Get completed games
        /// </summary>
        public static List<Game> GetCompletedGames(this IEnumerable<Game> games)
        {
            return games
                .Where(g => g.Status == "FT" || g.Status == "AET")
                .OrderByDescending(g => g.Date)
                .ToList();
        }

        /// <summary>
        /// Get team stats
        /// </summary>
        public static TeamStats GetStats(this Team team, IEnumerable<Game> allGames)
        {
            var teamGames = allGames.Where(g => g.HomeTeamId == team.Id || g.AwayTeamId == team.Id).ToList();

            int wins = 0, losses = 0, draws = 0;
            int pointsFor = 0, pointsAgainst = 0;

            foreach (var game in teamGames.GetCompletedGames())
            {
                bool isHome = game.HomeTeamId == team.Id;
                int teamScore = isHome ? game.HomeTeamScore ?? 0 : game.AwayTeamScore ?? 0;
                int opponentScore = isHome ? game.AwayTeamScore ?? 0 : game.HomeTeamScore ?? 0;

                pointsFor += teamScore;
                pointsAgainst += opponentScore;

                if (teamScore > opponentScore)
                    wins++;
                else if (teamScore < opponentScore)
                    losses++;
                else
                    draws++;
            }

            return new TeamStats
            {
                TeamId = team.Id,
                TeamName = team.Name,
                Wins = wins,
                Losses = losses,
                Draws = draws,
                PointsFor = pointsFor,
                PointsAgainst = pointsAgainst,
                PointDifference = pointsFor - pointsAgainst,
                TotalGames = teamGames.Count
            };
        }

        /// <summary>
        /// Format game result as string
        /// </summary>
        public static string FormatScore(this Game game)
        {
            if (!game.HomeTeamScore.HasValue || !game.AwayTeamScore.HasValue)
                return "- vs -";

            return $"{game.HomeTeamScore} - {game.AwayTeamScore}";
        }
    }

    public class TeamStats
    {
        public int TeamId { get; set; }
        public string? TeamName { get; set; }
        public int Wins { get; set; }
        public int Losses { get; set; }
        public int Draws { get; set; }
        public int PointsFor { get; set; }
        public int PointsAgainst { get; set; }
        public int PointDifference { get; set; }
        public int TotalGames { get; set; }

        public double WinPercentage => TotalGames > 0 ? (double)Wins / TotalGames * 100 : 0;

        public override string ToString()
        {
            return $"{TeamName}: {Wins}W-{Draws}D-{Losses}L | Pts {PointsFor}({PointDifference:+#;-#;0})";
        }
    }
}
