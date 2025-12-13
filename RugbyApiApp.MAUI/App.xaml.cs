using System.Windows;
using RugbyApiApp.Data;

namespace RugbyApiApp.MAUI;

public partial class App : Application
{
    protected override void OnStartup(StartupEventArgs e)
    {
        base.OnStartup(e);
        
        // Initialize database when app starts
        try
        {
            using (var context = new RugbyDbContext())
            {
                context.Database.EnsureCreated();
            }
        }
        catch (Exception ex)
        {
            System.Diagnostics.Debug.WriteLine($"Database initialization error: {ex.Message}");
        }
    }
}
