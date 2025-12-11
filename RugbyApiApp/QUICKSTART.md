# Quick Start Guide

## First Run Setup

### 1. Get Your API Key
- Go to https://api-sports.io/
- Sign up for an account (free tier available)
- Copy your API key from the dashboard

### 2. Set Environment Variable

**Windows Command Prompt:**
```cmd
setx RUGBY_API_KEY "YOUR_API_KEY_HERE"
```

Then restart your terminal or Visual Studio.

**Verify it works:**
```cmd
echo %RUGBY_API_KEY%
```

### 3. Run the Application
```bash
cd RugbyApiApp
dotnet run
```

## What the App Does

On first run, the application will:
1. ? Create `rugby.db` SQLite database
2. ? Fetch teams from api-sports.io
3. ? Fetch matches from the 2024 season
4. ? Convert image URLs to your BunnyCDN domain
5. ? Store all data in the local database
6. ? Display a summary of the data

## Sample Output

```
=== Rugby API Console Application ===

? Database initialized

Fetching teams from API...
Retrieved 20 teams
? Stored teams in database

Fetching matches for season 2024...
Retrieved 15 matches
? Stored matches in database

=== Stored Data ===

Teams in database: 20
  - England (ENG)
  - France (FRA)
  - New Zealand (NZL)
  - ...

Matches in database: 15
  - England vs France (FT)
    Score: 42 - 37
  - New Zealand vs South Africa (FT)
    Score: 12 - 11
  - ...

=== Application finished ===
```

## Common Customizations

### Change Season
Edit `Program.cs` in the `Main` method:

```csharp
// Change from 2024 to another year
await FetchAndStoreMatchesAsync(apiClient, dataService, 2023);
```

### Fetch Player Data
Add to `Program.cs`:

```csharp
// Fetch players for each team
var teams = await dataService.GetTeamsAsync();
foreach (var team in teams)
{
    var players = await apiClient.GetTeamPlayersAsync(team.Id);
    // Process players...
}
```

### Query Stored Data
```csharp
using (var context = new RugbyDbContext())
{
    var allTeams = context.Teams.ToList();
    var matches = context.Matches
        .Include(m => m.HomeTeam)
        .Include(m => m.AwayTeam)
        .ToList();
}
```

## API Rate Limits

The free tier of api-sports.io has rate limits:
- Free: ~100 requests/day
- Premium: Higher limits

Monitor your usage at https://api-sports.io/dashboard

## Troubleshooting

### "RUGBY_API_KEY environment variable not set"
- Make sure you've set the environment variable correctly
- Restart Visual Studio after setting it
- Check: `setx RUGBY_API_KEY` (Windows) or `echo $RUGBY_API_KEY` (Linux/Mac)

### "No teams retrieved from API"
- Check your API key is correct
- Verify internet connection
- Check api-sports.io service status
- May be hitting rate limit

### Database Issues
- Delete `rugby.db` to start fresh
- Database is recreated on next run

### Image URLs Not Converting
- BunnyCDN URL conversion is automatic
- Check your BunnyCDN hostname is correct in `Configuration/RugbyApiSettings.cs`

## Next Steps

1. **Display Team Statistics**
   - Use `RugbyDataExtensions.GetStats()` to calculate team records
   - Query upcoming vs completed matches

2. **Add Web Interface**
   - Create an ASP.NET Core API on top of this console app
   - Expose data via REST endpoints

3. **Scheduled Updates**
   - Use a Windows Service or background task
   - Auto-update match results daily

4. **Advanced Filtering**
   - Query matches by league, season, team
   - Generate standings tables

## Resources

- [api-sports.io Rugby API Docs](https://api-sports.io/documentation/rugby/v1)
- [RestSharp Docs](https://restsharp.dev/)
- [Entity Framework Core](https://learn.microsoft.com/en-us/ef/core/)
- [BunnyCDN Docs](https://bunnycdn.com/docs)
