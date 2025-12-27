# ✅ YouTube Search Button - Troubleshooting Guide

## Problem
The video search button in the Watch tab doesn't work (appears disabled).

## Root Cause
The `SearchYouTubeCommand` is disabled because the `YoutubeVideoService` is `null`. This happens when:

**Missing YouTube API Key** ← **Most Common**
```csharp
// YoutubeVideoService constructor throws:
if (string.IsNullOrEmpty(apiKey))
{
    throw new ArgumentException("YouTubeApiKey not found in configuration.");
}
```

The exception is caught silently in `MainViewModel`, leaving `youtubeService` as `null`.

## Solution

### Step 1: Get a YouTube API Key
1. Go to [Google Cloud Console](https://console.cloud.google.com/)
2. Create a new project (or select existing)
3. Enable the **YouTube Data API v3**
4. Create an **API Key** credential
5. Copy the API key

### Step 2: Configure the YouTube API Key

**Option A: User Secrets (Recommended for Development)**
```bash
cd C:\Users\Brand\source\repos\RugbyApiApp\RugbyApiApp.MAUI

dotnet user-secrets init
dotnet user-secrets set "YouTubeApiKey" "YOUR_API_KEY_HERE"
```

**Option B: App Settings (Local Machine)**
Add to `appsettings.json`:
```json
{
  "YouTubeApiKey": "YOUR_API_KEY_HERE"
}
```

**Option C: Environment Variable**
```bash
setx YouTubeApiKey "YOUR_API_KEY_HERE"
```

### Step 3: Verify Configuration
The YouTube service should now initialize successfully.

## Testing the Fix

After configuring the YouTube API key:

1. **Restart the application** (full restart, not just hot reload)
2. Go to the **Watch** tab
3. Select a game from the list
4. The **Search** button should now be **enabled** (not grayed out)
5. Click **Search** to find YouTube videos

## Button Enabled Requirements

The Search button is only enabled when **ALL** conditions are met:
```csharp
_ => _youtubeService is not null     // ← Requires YouTube API key configured
  && SelectedGame != null             // ← A game must be selected
  && !IsSearching                     // ← Search not already in progress
```

## Debug the Issue

To see if the YouTube service failed to initialize:

**Check Debug Output in Visual Studio:**
1. Open Debug Output window (Debug → Windows → Output)
2. Look for: `"Warning: YouTube service initialization failed:"`
3. The error message will tell you what's wrong

**Or Add Logging:**
Modify `MainViewModel.cs`:
```csharp
try
{
    youtubeService = new YoutubeVideoService(configuration);
}
catch (Exception ex)
{
    System.Diagnostics.Debug.WriteLine($"❌ YouTube service FAILED: {ex.Message}");
    // Display error to user
}
```

## Quick Checklist

- [ ] YouTube API key obtained from Google Cloud Console
- [ ] API key added to user secrets OR appsettings.json OR environment variable
- [ ] Application restarted
- [ ] A game is selected in the Watch tab
- [ ] Search button is now enabled
- [ ] Click Search button successfully searches YouTube

## Still Not Working?

1. **Verify the API key is correct**
   - Test it directly: https://www.googleapis.com/youtube/v3/search?key=YOUR_KEY&part=snippet&q=rugby

2. **Check YouTube Data API is enabled**
   - Go to Google Cloud Console
   - Verify YouTube Data API v3 shows as "ENABLED"

3. **Check quota/rate limits**
   - Google Cloud Console → APIs & Services → Quotas
   - Verify you have available quota

4. **Restart Visual Studio**
   - Sometimes configuration caching requires a full IDE restart

## Configuration Priority Order

The app checks for YouTubeApiKey in this order:
1. `appsettings.json` (checked first)
2. User Secrets (Development)
3. Environment Variables
4. None (results in null service, button disabled)

## Files That Need to Know About This

- `RugbyApiApp.MAUI/ViewModels/MainViewModel.cs` - Initializes YouTube service
- `RugbyApiApp.YouTubeService/YouTubeVideoService.cs` - Requires YouTube API key
- `RugbyApiApp.MAUI/Views/MainWindow.xaml` - Search button uses SearchYouTubeCommand
- `RugbyApiApp.MAUI/ViewModels/WatchViewModel.cs` - SearchYouTubeCommand canExecute condition

## Status
If you've configured the YouTube API key and restarted the app, the search button should now work! 🎉
