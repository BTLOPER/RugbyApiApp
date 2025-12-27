# 🔍 Video Search Button - Root Cause & Solution

## The Issue
The video search button in the Watch tab is **disabled (grayed out)** and doesn't work.

## Why It's Disabled

The `SearchYouTubeCommand` has a **canExecute condition** that requires three things:

```csharp
SearchYouTubeCommand = new AsyncRelayCommand(
    async _ => await SearchYouTubeAsync(),
    _ => _youtubeService is not null      // ← This is failing!
      && SelectedGame != null
      && !IsSearching
);
```

**The problem:** `_youtubeService` is `null` because the YouTube API key is missing from configuration.

## The Root Cause

In `YoutubeVideoService.cs`:
```csharp
public YoutubeVideoService(IConfiguration configuration)
{
    var apiKey = _configuration["YouTubeApiKey"] ?? "";
    
    if (string.IsNullOrEmpty(apiKey))  // ← This condition is TRUE
    {
        throw new ArgumentException("YouTubeApiKey not found in configuration.");
    }
    // ... rest of initialization
}
```

In `MainViewModel.cs`:
```csharp
try
{
    youtubeService = new YoutubeVideoService(configuration);  // ← Throws exception
}
catch (Exception ex)
{
    System.Diagnostics.Debug.WriteLine($"Warning: YouTube service failed: {ex.Message}");
    // youtubeService stays null
}
```

**Result:** YouTube service stays `null` → button stays disabled

## The Solution

You need a **YouTube API Key** from Google Cloud.

### Get Your YouTube API Key

1. Go to [Google Cloud Console](https://console.cloud.google.com/)
2. Create a new project (or use existing)
3. Enable **YouTube Data API v3**:
   - Click "Enable APIs and Services"
   - Search for "YouTube Data API"
   - Click "Enable"
4. Create an **API Key credential**:
   - Go to "Credentials"
   - Click "Create Credentials"
   - Select "API Key"
   - Copy the key

### Add It to Your Application

**Best Option: Use User Secrets** (Development)

```bash
# Navigate to the MAUI project
cd C:\Users\Brand\source\repos\RugbyApiApp\RugbyApiApp.MAUI

# Initialize secrets
dotnet user-secrets init

# Set the YouTube API key
dotnet user-secrets set "YouTubeApiKey" "YOUR_ACTUAL_API_KEY_HERE"
```

**Alternative: Use appsettings.json**

Edit `RugbyApiApp.MAUI/appsettings.json`:
```json
{
  "YouTubeApiKey": "YOUR_ACTUAL_API_KEY_HERE"
}
```

**Alternative: Use Environment Variable**

```bash
# Windows Command Prompt (permanent)
setx YouTubeApiKey "YOUR_ACTUAL_API_KEY_HERE"

# PowerShell (current session)
$env:YouTubeApiKey = "YOUR_ACTUAL_API_KEY_HERE"
```

## After Configuration

1. **Close and restart the application** (full restart, not hot reload)
2. Go to **Watch tab**
3. Select a game
4. **Search button should now be enabled!** ✅
5. Click **Search** to find YouTube videos for that game

## Verification

To confirm the YouTube service is working:

1. Open **Debug Output** in Visual Studio (Debug → Windows → Output)
2. Run the app
3. Look for one of these messages:
   - ✅ No warning/error = YouTube service initialized successfully
   - ⚠️ "YouTube service initialization: YouTubeApiKey not found..." = API key missing
   - ❌ "YouTube service initialization failed: ..." = Other configuration error

## Improved Error Messages

I've updated `MainViewModel.cs` to provide clearer debug messages:

```csharp
catch (ArgumentException argEx)
{
    // Shows clearly that API key is missing
    System.Diagnostics.Debug.WriteLine($"⚠️ YouTube service initialization: {argEx.Message}");
}
```

When you open the Debug Output window, you'll see exactly what's wrong.

## Files Modified

- ✅ `MainViewModel.cs` - Better error messages for debugging

## Quick Checklist

- [ ] YouTube API Key obtained from Google Cloud
- [ ] API Key added to user secrets or appsettings.json  
- [ ] Application fully restarted
- [ ] Watch tab opened
- [ ] Game selected from list
- [ ] Search button is NOW ENABLED ✅
- [ ] Click Search button works!

## If It Still Doesn't Work

Check these things:

1. **Verify API key is correct**
   - Test in browser: `https://www.googleapis.com/youtube/v3/search?key=YOUR_KEY&part=snippet&q=rugby`
   - Should return JSON, not an error

2. **Verify YouTube Data API v3 is enabled**
   - Google Cloud Console → APIs & Services
   - Look for "YouTube Data API v3"
   - Should show "ENABLED"

3. **Check API Quotas**
   - Google Cloud Console → APIs & Services → Quotas
   - YouTube Data API should have available quota

4. **Check Debug Output**
   - Visual Studio → Debug → Windows → Output
   - Shows the actual error message if something went wrong

5. **Restart Everything**
   - Close Visual Studio completely
   - Delete `bin` and `obj` folders
   - Reopen and rebuild

## Status

Once you configure the YouTube API key and restart the application, the search button will work! 🎉

**Your issue: Video search button disabled**  
**Root cause: Missing YouTube API key**  
**Solution: Add YouTubeApiKey to configuration (user secrets recommended)**  
**Time to fix: ~5 minutes**
