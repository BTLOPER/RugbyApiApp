using Microsoft.EntityFrameworkCore;
using RugbyApiApp.Models;

namespace RugbyApiApp.Data
{
    public class RugbyDbContext : DbContext
    {
        public DbSet<Team> Teams { get; set; }
        public DbSet<Game> Games { get; set; }
        public DbSet<League> Leagues { get; set; }
        public DbSet<Season> Seasons { get; set; }
        public DbSet<Country> Countries { get; set; }
        public DbSet<Video> Videos { get; set; }
        public DbSet<YouTubeVideoSearchResult> YouTubeVideoSearchResults { get; set; }

        protected override void OnConfiguring(DbContextOptionsBuilder optionsBuilder)
        {
            string dbPath = GetDatabasePath();
            string connectionString = $"Data Source={dbPath}";
            optionsBuilder.UseSqlite(connectionString);
            optionsBuilder.ConfigureWarnings(w => w.Ignore(Microsoft.EntityFrameworkCore.Diagnostics.RelationalEventId.PendingModelChangesWarning));
        }

        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {
            base.OnModelCreating(modelBuilder);

            // Team configuration
            modelBuilder.Entity<Team>()
                .HasKey(t => t.Id);
            
            modelBuilder.Entity<Team>()
                .Property(t => t.Name)
                .IsRequired();

            // Game configuration
            modelBuilder.Entity<Game>()
                .HasKey(g => g.Id);
            
            modelBuilder.Entity<Game>()
                .HasOne(g => g.HomeTeam)
                .WithMany()
                .HasForeignKey(g => g.HomeTeamId)
                .OnDelete(DeleteBehavior.Restrict);

            modelBuilder.Entity<Game>()
                .HasOne(g => g.AwayTeam)
                .WithMany()
                .HasForeignKey(g => g.AwayTeamId)
                .OnDelete(DeleteBehavior.Restrict);

            // Video configuration
            modelBuilder.Entity<Video>()
                .HasKey(v => v.Id);

            modelBuilder.Entity<Video>()
                .HasOne(v => v.Game)
                .WithMany(g => g.Videos)
                .HasForeignKey(v => v.GameId)
                .OnDelete(DeleteBehavior.Cascade);

            // League configuration
            modelBuilder.Entity<League>()
                .HasKey(l => l.Id);

            // Season configuration
            modelBuilder.Entity<Season>()
                .HasKey(s => s.Id);

            // Country configuration
            modelBuilder.Entity<Country>()
                .HasKey(c => c.Id);

            // YouTubeVideoSearchResult configuration
            modelBuilder.Entity<YouTubeVideoSearchResult>()
                .HasKey(y => y.Id);

            modelBuilder.Entity<YouTubeVideoSearchResult>()
                .Property(y => y.VideoId)
                .IsRequired();

            modelBuilder.Entity<YouTubeVideoSearchResult>()
                .Property(y => y.Title)
                .IsRequired();

            modelBuilder.Entity<YouTubeVideoSearchResult>()
                .Property(y => y.Description)
                .IsRequired();

            modelBuilder.Entity<YouTubeVideoSearchResult>()
                .Property(y => y.ThumbnailUrl)
                .IsRequired();

            modelBuilder.Entity<YouTubeVideoSearchResult>()
                .Property(y => y.ChannelTitle)
                .IsRequired();

            modelBuilder.Entity<YouTubeVideoSearchResult>()
                .HasOne(y => y.Game)
                .WithMany()
                .HasForeignKey(y => y.GameId)
                .OnDelete(DeleteBehavior.Cascade);
        }

        /// <summary>
        /// Get the database path based on the platform
        /// Supports both console apps and MAUI apps
        /// </summary>
        public static string GetDatabasePath()
        {
            // Check if we're in a MAUI context
            if (IsMauiPlatform())
            {
                // MAUI mobile app - use FileSystem.AppDataDirectory
                var appDataPath = GetMauiAppDataPath();
                return Path.Combine(appDataPath, "rugby.db");
            }
            else
            {
                // Console or Desktop app
                var appDataPath = Path.Combine(
                    Environment.GetFolderPath(Environment.SpecialFolder.ApplicationData),
                    "RugbyApiApp"
                );
                
                if (!Directory.Exists(appDataPath))
                {
                    Directory.CreateDirectory(appDataPath);
                }
                
                return Path.Combine(appDataPath, "rugby.db");
            }
        }

        /// <summary>
        /// Check if we're running in a MAUI context
        /// </summary>
        private static bool IsMauiPlatform()
        {
            try
            {
                // Try to access MAUI-specific types
                var type = Type.GetType("Microsoft.Maui.ApplicationModel.FileSystem, Microsoft.Maui");
                return type != null;
            }
            catch
            {
                return false;
            }
        }

        /// <summary>
        /// Get MAUI AppDataDirectory safely via reflection
        /// </summary>
        private static string GetMauiAppDataPath()
        {
            try
            {
                // Access FileSystem.AppDataDirectory via reflection
                var fileSystemType = Type.GetType("Microsoft.Maui.ApplicationModel.FileSystem, Microsoft.Maui");
                if (fileSystemType != null)
                {
                    var property = fileSystemType.GetProperty("AppDataDirectory", System.Reflection.BindingFlags.Static | System.Reflection.BindingFlags.Public);
                    if (property != null)
                    {
                        return property.GetValue(null)?.ToString() ?? Path.GetTempPath();
                    }
                }
            }
            catch { }

            return Path.GetTempPath();
        }
    }
}
