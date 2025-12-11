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

        protected override void OnConfiguring(DbContextOptionsBuilder optionsBuilder)
        {
            string connectionString = "Data Source=rugby.db";
            optionsBuilder.UseSqlite(connectionString);
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

            // League configuration
            modelBuilder.Entity<League>()
                .HasKey(l => l.Id);

            // Season configuration
            modelBuilder.Entity<Season>()
                .HasKey(s => s.Id);

            // Country configuration
            modelBuilder.Entity<Country>()
                .HasKey(c => c.Id);
        }
    }
}
