# Rugby API Console Application

A .NET 10 console application that fetches rugby data from the api-sports.io Rugby API, stores it in SQLite using Entity Framework Core, and integrates with BunnyCDN for optimized image delivery.

## Features

### API Integration
- Countries: Fetch all rugby-playing countries with flags
- Seasons: Get available rugby seasons  
- Leagues: Access rugby league information
- Teams: Retrieve team data with logos and flags
- Games: Get match schedules, results, and detailed information

### Database Management
- SQLite persistence with Entity Framework Core
- Automatic upsert operations (insert or update)
- Relationship management between entities
- Data completion tracking for intelligent caching
- Timestamp tracking (CreatedAt, UpdatedAt)

### CDN Integration
- Automatic image URL conversion to BunnyCDN
- Optimized image delivery for flags, logos, and team photos
- Support for both media.api-sports.io and api-sports.io domains

### Smart Data Fetching
- Auto-fetch all incomplete data with one command
- Fetch data by individual categories
- View paginated results
- Error handling and API response validation
- Comprehensive error messages from API

## Prerequisites

- .NET 10 SDK or later
- Visual Studio 2026+ or Visual Studio Code
- api-sports.io API key (free or paid account)

## Setup Instructions

### 1. Get Your API Key

1. Visit https://api-sports.io/
2. Sign up for a free or paid account
3. Navigate to your dashboard and copy your API key

### 2. Set Environment Variable

Set your API key as an environment variable named RUGBY_API_KEY:

Windows (Command Prompt):
```cmd
setx RUGBY_API_KEY "your-api-key-here"
```

Windows (PowerShell):
```powershell
[Environment]::SetEnvironmentVariable("RUGBY_API_KEY", "your-api-key-here", "User")
```

macOS/Linux:
```bash
export RUGBY_API_KEY="your-api-key-here"
```

After setting the environment variable, restart Visual Studio or your terminal.

### 3. Build and Run

```bash
cd RugbyApiApp
dotnet build
dotnet run
```

Or run directly from Windows Terminal in your project directory:

```powershell
dotnet run
```

## Project Structure

```
RugbyApiApp/
+-- Models/                  Entity Framework domain models
|   +-- Country.cs
|   +-- Season.cs
|   +-- League.cs
|   +-- Team.cs
|   +-- Game.cs
+-- DTOs/                    Data Transfer Objects for API responses
|   +-- CountryResponse.cs
|   +-- LeagueResponse.cs
|   +-- TeamResponse.cs
|   +-- GameResponse.cs
|   +-- ApiResponse.cs
|   +-- SeasonResponse.cs
+-- Data/                    Entity Framework DbContext
|   +-- RugbyDbContext.cs
+-- Services/                Business logic
|   +-- RugbyApiClient.cs    RestSharp client for api-sports.io
|   +-- DataService.cs       Database operations
+-- Examples/                Usage examples
|   +-- SeasonsAndLeaguesExamples.cs
|   +-- RugbyQueryExamples.cs
+-- Configuration/           Application settings
|   +-- RugbyApiSettings.cs
+-- Program.cs               Application entry point
+-- RugbyApiApp.csproj       Project configuration
```

## Main Menu Options

1. Browse and Fetch Countries - View and fetch country data
2. Browse and Fetch Seasons - View and fetch available seasons
3. Browse and Fetch Leagues - View and fetch league information
4. Browse and Fetch Teams - View team data
5. Browse and Fetch Games - Fetch and view game data
6. View All Stored Data - Display summary of database contents
7. Auto-Fetch All Incomplete Data - Fetch all data types automatically
8. Clear All Data - Delete all stored data from database
0. Exit - Close the application

## NuGet Dependencies

| Package | Version | Purpose |
|---------|---------|---------|
| RestSharp | 107.3.0+ | HTTP client for API calls |
| Microsoft.EntityFrameworkCore | 8.0.0+ | ORM framework |
| Microsoft.EntityFrameworkCore.Sqlite | 8.0.0+ | SQLite database provider |
| Microsoft.EntityFrameworkCore.Design | 8.0.0+ | EF Core tooling |

## Database

The application automatically creates an SQLite database file named rugby.db in the application directory on first run.

### Tables

| Table | Purpose |
|-------|---------|
| Countries | Rugby-playing countries with flags |
| Seasons | Available rugby seasons (years) |
| Leagues | Rugby leagues with country information |
| Teams | Rugby teams with logos and codes |
| Games | Match results and schedules |

## API Documentation

- api-sports.io Rugby API: https://api-sports.io/documentation/rugby/v1
- RestSharp Documentation: https://restsharp.dev/
- Entity Framework Core: https://docs.microsoft.com/en-us/ef/core/
- BunnyCDN Documentation: https://bunnycdn.com/docs

## Error Handling

The application provides comprehensive error handling:

- API Errors: Displays API error messages
- Network Errors: Handles connection failures gracefully
- Validation Errors: Checks data integrity before database operations
- Database Errors: Manages SQLite transaction failures

## Running in Windows Terminal

For best display of output and formatting, run the application in Windows Terminal:

1. Open Windows Terminal (Windows Key + T)
2. Navigate to your project directory
3. Run: dotnet run

Or right-click in File Explorer and select "Open in Terminal" (Windows 11)

## Tips

### Free API Plan
- The free tier of api-sports.io has rate limits
- Start with fetching basic data (countries, seasons, leagues)
- Use specific year ranges when fetching games to manage API calls

### Optimizing Database Queries
- Use the Data Completion Statistics to track progress
- Only incomplete data is fetched on subsequent runs
- Clear data periodically to reset the database

### Image Handling
- Media URLs are automatically converted to BunnyCDN for faster delivery
- Ensure you have a BunnyCDN account for image hosting (optional)

## Contributing

Contributions are welcome! Please feel free to submit a Pull Request. For contributions, please:

1. Fork the repository
2. Create your feature branch (git checkout -b feature/AmazingFeature)
3. Commit your changes (git commit -m 'Add some AmazingFeature')
4. Push to the branch (git push origin feature/AmazingFeature)
5. Open a Pull Request

## License

This project is licensed under the MIT License - see the LICENSE file for details.

## Support

If you encounter issues:

1. Check that your RUGBY_API_KEY environment variable is set correctly
2. Verify your internet connection
3. Check the api-sports.io API documentation
4. Open an issue on GitHub with details about the error and steps to reproduce

## Acknowledgments

- api-sports.io for the Rugby API
- BunnyCDN for CDN services
- RestSharp for HTTP client functionality
- Entity Framework Core for ORM capabilities
