# Rugby API Console Application

A .NET 10 console application that fetches rugby data from the api-sports.io API, stores it in SQLite using Entity Framework Core, and integrates with BunnyCDN for optimized image delivery.

## Prerequisites

- Visual Studio 2026 or later
- .NET 10 SDK
- api-sports.io API key (from https://api-sports.io/)
- BunnyCDN account (optional, for CDN image hosting)

## Setup Instructions

### 1. Get API Key

1. Visit [api-sports.io](https://api-sports.io/)
2. Sign up for a free or paid account
3. Navigate to your dashboard to get your API key

### 2. Set Environment Variable

Set your API key as an environment variable named `RUGBY_API_KEY`:

**Windows (Command Prompt):**
```cmd
setx RUGBY_API_KEY "your-api-key-here"
```

**Windows (PowerShell):**
```powershell
[Environment]::SetEnvironmentVariable("RUGBY_API_KEY", "your-api-key-here", "User")
```

**macOS/Linux:**
```bash
export RUGBY_API_KEY="your-api-key-here"
```

Restart Visual Studio or your terminal after setting the environment variable.

### 3. Build and Run

```bash
dotnet build
dotnet run
```

## Project Structure

```
RugbyApiApp/
??? Models/              # Entity Framework domain models
?   ??? Team.cs
?   ??? Player.cs
?   ??? Match.cs
?   ??? League.cs
??? DTOs/               # Data Transfer Objects for API responses
?   ??? TeamResponse.cs
?   ??? PlayerResponse.cs
?   ??? MatchResponse.cs
?   ??? ApiResponse.cs
??? Data/               # Entity Framework DbContext
?   ??? RugbyDbContext.cs
??? Services/           # Business logic
?   ??? RugbyApiClient.cs    # RestSharp client for api-sports.io
?   ??? DataService.cs       # Database operations
??? Program.cs          # Application entry point
??? RugbyApiApp.csproj  # Project configuration
```

## Features

### API Integration
- **Teams**: Fetch all teams with logos and flags (served via BunnyCDN)
- **Players**: Retrieve player information including statistics and photos
- **Matches**: Get match schedules, results, and scores
- **Leagues**: Access league information and classifications

### Database Operations
- SQLite persistence with Entity Framework Core
- Automatic upsert operations (insert or update)
- Relationship management (Teams, Players, Matches)
- Timestamp tracking (CreatedAt, UpdatedAt)

### CDN Integration
- Automatic image URL conversion from api-sports.io to BunnyCDN
- Optimized image delivery using your BunnyCDN account
- Support for team logos, player photos, and league emblems

## NuGet Packages

- **RestSharp 107.3.0** - HTTP client for API calls
- **Microsoft.EntityFrameworkCore 8.0.0** - ORM framework
- **Microsoft.EntityFrameworkCore.Sqlite 8.0.0** - SQLite provider
- **Microsoft.EntityFrameworkCore.Design 8.0.0** - EF Core design tools

## Usage Examples

### Fetch Teams
```csharp
var apiClient = new RugbyApiClient(apiKey);
var teams = await apiClient.GetTeamsAsync(page: 1);
```

### Fetch Matches by Season
```csharp
var matches = await apiClient.GetMatchesBySeasonAsync(2024);
```

### Access Stored Data
```csharp
var dataService = new DataService(new RugbyDbContext());
var storedTeams = await dataService.GetTeamsAsync();
var matchesByseason = await dataService.GetMatchesBySeasonAsync(2024);
```

### Convert Images to CDN
```csharp
string cdnUrl = RugbyApiClient.ConvertToCdnUrl(logoUrl);
```

## Database

The application automatically creates an SQLite database file named `rugby.db` in the application directory.

### Tables
- **Teams** - Rugby teams with logos and codes
- **Players** - Player information linked to teams
- **Matches** - Match results and schedules
- **Leagues** - League information

## API Documentation

- [api-sports.io Rugby API](https://api-sports.io/documentation/rugby/v1)
- [RestSharp Documentation](https://restsharp.dev/)
- [BunnyCDN Documentation](https://bunnycdn.com/docs)

## Notes

- The free tier of api-sports.io has rate limits - adjust page parameters accordingly
- Image URLs are automatically converted to your BunnyCDN domain
- Entity Framework automatically manages timestamps
- All API calls are async to prevent blocking

## License

This is a sample application for educational purposes.
