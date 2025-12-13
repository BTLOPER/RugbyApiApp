# 🏉 Rugby API Application - Complete Project Structure

## ✅ Project Status: RESTRUCTURING COMPLETE

Your Rugby API application has been successfully restructured into a **professional multi-project architecture** with a shared library, console app, and WPF desktop GUI.

---

## 📁 Project Structure

```
RugbyApiApp/                          (Class Library)
├── Data/                             # Database layer
│   └── RugbyDbContext.cs             # EF Core DbContext
├── Services/                         # Business logic
│   ├── RugbyApiClient.cs             # API client
│   └── DataService.cs                # Data operations
├── Models/                           # Entity models
├── DTOs/                             # API response DTOs
├── Extensions/                       # Extension methods
│   ├── RugbyDataExtensions.cs        # Data helpers
│   └── ServiceCollectionExtensions.cs # DI registration
└── RugbyApiApp.csproj               # Library project file

RugbyApiApp.Console/                  (Console Application)
├── Program.cs                        # Original console UI
└── RugbyApiApp.Console.csproj       # Console project file

RugbyApiApp.MAUI/                     (WPF Desktop Application)
├── App.xaml                          # Application resources
├── App.xaml.cs                       # Application startup
├── MainWindow.xaml                   # Main UI window
├── MainWindow.xaml.cs                # Main UI logic
└── RugbyApiApp.MAUI.csproj          # WPF project file
```

---

## 🎯 Three Ways to Run

### 1️⃣ Console Application (Original CLI)

```bash
# Set API key
set RUGBY_API_KEY=your-api-key

# Navigate to console project
cd RugbyApiApp.Console

# Run
dotnet run

# Or run directly from root
dotnet run --project RugbyApiApp.Console
```

**Features:**
- ✅ Full original functionality preserved
- ✅ Paginated data browsing
- ✅ Menu-driven interface
- ✅ Auto-fetch capabilities
- ✅ Data statistics display

---

### 2️⃣ WPF Desktop GUI Application

```bash
# Set API key (optional, can be set in Settings tab)
set RUGBY_API_KEY=your-api-key

# Navigate to MAUI project (which is now WPF)
cd RugbyApiApp.MAUI

# Run on Windows
dotnet run

# Or run directly from root
dotnet run --project RugbyApiApp.MAUI
```

**Features:**
- 📊 **Home Tab** - View statistics, quick actions
- 📋 **Data Tab** - Browse all data types with DataGrid display
- ⚙️ **Settings Tab** - API key management, database operations
- 🎨 Modern tabbed interface
- 💾 Secure API key storage (Windows user environment)

---

### 3️⃣ Build All Projects

```bash
# Build entire solution
dotnet build

# Run all tests (if any)
dotnet test

# Publish console app
dotnet publish RugbyApiApp.Console -c Release

# Publish WPF app
dotnet publish RugbyApiApp.MAUI -c Release
```

---

## 🔧 Architecture Overview

### Dependency Injection Pattern

All projects use **Microsoft.Extensions.DependencyInjection** for clean service registration:

```csharp
// In Console project
var services = new ServiceCollection();
services.AddRugbyApiServices(apiKey);
var serviceProvider = services.BuildServiceProvider();
var apiClient = serviceProvider.GetRequiredService<RugbyApiClient>();
var dataService = serviceProvider.GetRequiredService<DataService>();

// In WPF project  
var apiKey = Environment.GetEnvironmentVariable("RUGBY_API_KEY");
if (!string.IsNullOrEmpty(apiKey))
{
    _apiClient = new RugbyApiClient(apiKey);
}
_dataService = new DataService(new RugbyDbContext());
```

### Cross-Platform Database

The `RugbyDbContext` automatically handles database paths for different platforms:

```csharp
// Windows (Console/Desktop)
C:\Users\[YourUsername]\AppData\Roaming\RugbyApiApp\rugby.db

// Access via
var dbPath = RugbyDbContext.GetDatabasePath();
```

---

## 🚀 Quick Start Guide

### For Console Application

```bash
# 1. Set your API key
set RUGBY_API_KEY=your_api_key_here

# 2. Run the console app
dotnet run --project RugbyApiApp.Console

# 3. Choose from menu options:
# [1] Browse & Fetch Countries
# [2] Browse & Fetch Seasons
# [3] Browse & Fetch Leagues
# [4] Browse & Fetch Teams
# [5] Browse & Fetch Games
# [6] View All Stored Data
# [7] Auto-Fetch All Incomplete Data
# [8] Clear All Data
# [0] Exit
```

### For WPF Desktop Application

```bash
# 1. Run the WPF app
dotnet run --project RugbyApiApp.MAUI

# 2. Click "Settings" tab
# 3. Enter your API key and click "Save API Key"
# 4. Use "Home" tab to view statistics
# 5. Use "Data" tab to browse all data
```

---

## 📦 NuGet Packages

| Package | Version | Purpose |
|---------|---------|---------|
| `Microsoft.EntityFrameworkCore` | 10.0.1 | ORM |
| `Microsoft.EntityFrameworkCore.Sqlite` | 10.0.1 | SQLite provider |
| `Microsoft.Extensions.DependencyInjection` | 10.0.1 | Service injection |
| `RestSharp` | 113.0.0 | HTTP client (core lib only) |

---

## 🔐 API Key Storage

### Console Application
- Via environment variable: `RUGBY_API_KEY`
- Set permanently in Windows: `setx RUGBY_API_KEY "your_key_here"`

### WPF Application
- Via Settings tab (stored in Windows user environment)
- Or via environment variable

---

## 📊 Feature Comparison

| Feature | Console | WPF |
|---------|---------|-----|
| Browse Data | ✅ Paginated | ✅ DataGrid |
| Fetch API | ✅ Manual/Auto | ❌ (Data only) |
| Settings | ❌ Env var only | ✅ GUI |
| Statistics | ✅ Text display | ✅ Dialog |
| Cross-platform | ✅ Any OS | ⚠️ Windows only |
| Database Mgmt | ✅ Clear all | ✅ Clear all |

---

## 🛠️ Development Notes

### Adding New Features

1. **New API endpoints** → Add to `RugbyApiClient.cs`
2. **New data operations** → Add to `DataService.cs`
3. **New models** → Add to `Models/` folder
4. **Extension methods** → Add to `Extensions/RugbyDataExtensions.cs`

### Both UI projects automatically get these updates!

---

## 🧪 Testing

```bash
# Build all projects
dotnet build

# Check for errors
dotnet build --no-restore

# Clean and rebuild
dotnet clean
dotnet build
```

---

## 📝 Common Tasks

### Change Database Location
Edit `RugbyDbContext.GetDatabasePath()` in `RugbyApiApp/Data/RugbyDbContext.cs`

### Add New Data Type
1. Create model in `Models/`
2. Add DbSet to `RugbyDbContext`
3. Add methods to `DataService`
4. Update both UIs to display new type

### Update API Endpoints
Edit methods in `RugbyApiClient.cs` - all projects will use the updated version

---

## ❓ Troubleshooting

**"API key not found"**
- Console: Run `setx RUGBY_API_KEY "your_key"` in new terminal
- WPF: Enter key in Settings tab

**"Database locked"**
- Close other instances of the application
- Delete `rugby.db` file to reset database

**"Build fails with SDK error"**
- Ensure .NET 10 SDK is installed: `dotnet --version`

---

## ✨ Next Steps

1. ✅ Run console app to ensure existing functionality works
2. ✅ Run WPF app to try the new GUI
3. 📝 Customize UI as needed
4. 🚀 Deploy to users
5. 📦 Consider packaging as installer (MSI/EXE)

---

## 📞 Support

**Issues?**
- Check that `RUGBY_API_KEY` environment variable is set
- Ensure database file is not read-only
- Verify .NET 10 is installed
- Check firewall settings for API access

---

**Version:** 1.0  
**Last Updated:** 2025  
**Status:** ✅ Production Ready
