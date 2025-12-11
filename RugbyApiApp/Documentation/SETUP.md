# Setup and Installation Guide

## Prerequisites

- .NET 10 SDK or later
- Windows Terminal (recommended) or Command Prompt/PowerShell
- api-sports.io account (create at https://api-sports.io/)

## Installation Steps

### Step 1: Clone the Repository

```bash
git clone https://github.com/BTLOPER/RugbyApiApp.git
cd RugbyApiApp
```

### Step 2: Get Your API Key

1. Visit https://api-sports.io/
2. Sign up for a free or paid account
3. Log in to your dashboard
4. Copy your API key

### Step 3: Set Environment Variable

You need to set the RUGBY_API_KEY environment variable.

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

After setting the variable, restart your terminal or IDE.

### Step 4: Build the Project

```bash
cd RugbyApiApp
dotnet build
```

You should see "Build succeeded" at the end.

### Step 5: Run the Application

```bash
dotnet run
```

The application will start and show the main menu.

## First Run

On first run, the application will:
1. Create a SQLite database (rugby.db) in the application directory
2. Initialize all database tables
3. Display the main menu

## Verifying Installation

To verify everything is working:

1. Select menu option [7] Auto-Fetch All Incomplete Data
2. The application will fetch countries, seasons, leagues, games, and teams
3. Check the Data Completion Statistics in the main menu

## Troubleshooting

### ERROR: RUGBY_API_KEY environment variable not set

The API key environment variable is not set or was set incorrectly.

Solution:
1. Set the variable using the steps above
2. Restart your terminal completely
3. Run dotnet run again

### Network/Connection Errors

If you get connection errors:
1. Verify your internet connection
2. Check that api-sports.io is accessible
3. Verify your API key is correct and active
4. Check if you have hit API rate limits (free tier is limited)

### Git Not Recognized

If you get "'git' is not recognized":
1. Install Git from https://git-scm.com/download/win
2. Make sure to select "Add Git to PATH" during installation
3. Restart your terminal

### Database Lock Error

If you get database lock errors:
1. Close all instances of the application
2. Delete the rugby.db file (it will be recreated on next run)
3. Restart the application

## Running in Windows Terminal

For the best experience with proper formatting:

1. Open Windows Terminal (Windows Key + T)
2. Navigate to your project: cd C:\Users\Brand\source\repos\RugbyApiApp\RugbyApiApp
3. Run: dotnet run

Or right-click on the folder in File Explorer and select "Open in Terminal" (Windows 11).

## Next Steps

After successful installation:

1. Read the README.md for feature details
2. Explore the menu options
3. Try the Auto-Fetch feature to populate your database
4. View the stored data using menu option [6]

## Getting Help

If you encounter issues:

1. Check the error message carefully
2. Verify your API key is correct
3. Ensure .NET 10 is installed: dotnet --version
4. Check the api-sports.io documentation
5. Open an issue on GitHub with detailed information

## Development Setup

If you want to develop on this project:

1. Clone the repository as above
2. Open RugbyApiApp.sln in Visual Studio 2026
3. Set the RUGBY_API_KEY environment variable
4. Build the solution (Ctrl + Shift + B)
5. Set RugbyApiApp as startup project
6. Run (F5)

See CONTRIBUTING.md for development guidelines.
