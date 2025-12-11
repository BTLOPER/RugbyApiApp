# Rugby API Console Application

[![.NET 10](https://img.shields.io/badge/.NET-10.0-purple)](https://dotnet.microsoft.com/)
[![C#](https://img.shields.io/badge/C%23-14.0-green)](https://docs.microsoft.com/en-us/dotnet/csharp/)
[![License: MIT](https://img.shields.io/badge/License-MIT-yellow.svg)](LICENSE)

A .NET 10 console application that fetches rugby data from the [api-sports.io](https://api-sports.io/) Rugby API, stores it in SQLite using Entity Framework Core, and integrates with BunnyCDN for optimized image delivery.

## Features

### 📊 API Integration
- **Countries**: Fetch all rugby-playing countries with flags
- **Seasons**: Get available rugby seasons
- **Leagues**: Access rugby league information
- **Teams**: Retrieve team data with logos and flags
- **Games**: Get match schedules, results, and detailed information

### 💾 Database Management
- SQLite persistence with Entity Framework Core
- Automatic upsert operations (insert or update)
- Relationship management between entities
- Data completion tracking for intelligent caching
- Timestamp tracking (CreatedAt, UpdatedAt)

### 🖼️ CDN Integration
- Automatic image URL conversion to BunnyCDN
- Optimized image delivery for flags, logos, and team photos
- Support for both `media.api-sports.io` and `api-sports.io` domains

### 🎯 Smart Data Fetching
- Auto-fetch all incomplete data with one command
- Fetch data by individual categories
- View paginated results
- Error handling and API response validation
- Comprehensive error messages from API

## Prerequisites

- **.NET 10 SDK** or later
- **Visual Studio 2026+** or Visual Studio Code
- **api-sports.io API key** (free or paid account)
- **Git** (for cloning and pushing to GitHub)

## Quick Start

### 1. Clone the Repository

```bash
git clone https://github.com/yourusername/RugbyApiApp.git
cd RugbyApiApp
```

### 2. Get Your API Key

1. Visit [api-sports.io](https://api-sports.io/)
2. Sign up for a free or paid account
3. Navigate to your dashboard and copy your API key

### 3. Set Environment Variable

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

> **Important**: Restart Visual Studio or your terminal after setting the environment variable.

### 4. Build and Run

```bash
cd RugbyApiApp
dotnet build
dotnet run
```

## Project Structure

```
RugbyApiApp/
├── Models/                  # Entity Framework domain models
│   ├── Country.cs
│   ├── Season.cs
│   ├── League.cs
│   ├── Team.cs
│   └── Game.cs
├── DTOs/                    # Data Transfer Objects for API responses
│   ├── CountryResponse.cs
│   ├── LeagueResponse.cs
│   ├── TeamResponse.cs
│   ├── GameResponse.cs
│   ├── ApiResponse.cs
│   └── SeasonResponse.cs
├── Data/                    # Entity Framework DbContext
│   └── RugbyDbContext.cs
├── Services/                # Business logic
│   ├── RugbyApiClient.cs    # RestSharp client for api-sports.io
│   └── DataService.cs       # Database operations
├── Examples/                # Usage examples
│   ├── SeasonsAndLeaguesExamples.cs
│   └── RugbyQueryExamples.cs
├── Program.cs               # Application entry point
├── .gitignore              # Git ignore rules
├── RugbyApiApp.csproj      # Project configuration
└── README.md               # This file
```

## Usage

### Main Menu Options

1. **Browse & Fetch Countries** - View and fetch country data
2. **Browse & Fetch Seasons** - View and fetch available seasons
3. **Browse & Fetch Leagues** - View and fetch league information
4. **Browse & Fetch Teams** - View team data
5. **Browse & Fetch Games** - Fetch and view game data
6. **View All Stored Data** - Display summary of database contents
7. **Auto-Fetch All Incomplete Data** - Fetch countries, seasons, leagues, games, and teams
8. **Clear All Data** - Delete all stored data from database
9. **Exit** - Close the application

### Example: Auto-Fetch All Data

```
Main Menu:
[7] Auto-Fetch All Incomplete Data

→ Fetching Countries...
→ Fetching Seasons...
→ Fetching Leagues...
→ Fetching Games and Teams...
✓ Auto-fetch complete!
```

## NuGet Dependencies

| Package | Version | Purpose |
|---------|---------|---------|
| RestSharp | 107.3.0+ | HTTP client for API calls |
| Microsoft.EntityFrameworkCore | 8.0.0+ | ORM framework |
| Microsoft.EntityFrameworkCore.Sqlite | 8.0.0+ | SQLite database provider |
| Microsoft.EntityFrameworkCore.Design | 8.0.0+ | EF Core tooling |

## API Documentation

- [api-sports.io Rugby API](https://api-sports.io/documentation/rugby/v1)
- [RestSharp Documentation](https://restsharp.dev/)
- [Entity Framework Core](https://docs.microsoft.com/en-us/ef/core/)
- [BunnyCDN Documentation](https://bunnycdn.com/docs)

## Database

The application automatically creates an SQLite database file named `rugby.db` in the application directory on first run.

### Tables

| Table | Purpose |
|-------|---------|
| **Countries** | Rugby-playing countries with flags |
| **Seasons** | Available rugby seasons (years) |
| **Leagues** | Rugby leagues with country information |
| **Teams** | Rugby teams with logos and codes |
| **Games** | Match results and schedules |

## Error Handling

The application provides comprehensive error handling:

- **API Errors**: Displays API error messages (e.g., "Free plans do not have access to this season")
- **Network Errors**: Handles connection failures gracefully
- **Validation Errors**: Checks data integrity before database operations
- **Database Errors**: Manages SQLite transaction failures

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

Contributions are welcome! Please feel free to submit a Pull Request.

### Development Workflow

1. Fork the repository
2. Create your feature branch (`git checkout -b feature/AmazingFeature`)
3. Commit your changes (`git commit -m 'Add some AmazingFeature'`)
4. Push to the branch (`git push origin feature/AmazingFeature`)
5. Open a Pull Request

## License

This project is licensed under the MIT License - see the [LICENSE](LICENSE) file for details.

## Author

- **Your Name** - *Initial work* - [YourGithub](https://github.com/yourusername)

## Acknowledgments

- [api-sports.io](https://api-sports.io/) for the Rugby API
- [BunnyCDN](https://bunnycdn.com/) for CDN services
- [RestSharp](https://restsharp.dev/) for HTTP client
- [Entity Framework Core](https://docs.microsoft.com/en-us/ef/core/) for ORM

## Support

If you encounter issues:

1. Check that your `RUGBY_API_KEY` environment variable is set correctly
2. Verify your internet connection
3. Check the [api-sports.io API documentation](https://api-sports.io/documentation/rugby/v1)
4. Open an issue on GitHub with:
   - Error message
   - Steps to reproduce
   - Your .NET version (`dotnet --version`)

---

**Happy coding!** 🏉
