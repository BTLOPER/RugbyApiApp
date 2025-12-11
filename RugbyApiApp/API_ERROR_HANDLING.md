# API Error Handling Implementation

## Problem

When the API returned error responses (like "Free plans do not have access to this season"), the errors were being silently ignored. The application would simply report "No data retrieved" without informing the user about the actual API error that occurred.

### Example Problem Response
```json
{
  "get": "games",
  "parameters": { "league": "85", "season": "2024" },
  "errors": {
    "plan": "Free plans do not have access to this season, try from 2021 to 2023."
  },
  "results": 0,
  "paging": { "current": 1, "total": 1 },
  "response": []
}
```

The `errors` field contained important information, but it was being ignored.

---

## Solution

### 1. Updated DTOs to Capture Error Messages

#### ApiResponse.cs
Added `Errors` field to capture API error messages:
```csharp
public class ApiResponse<T>
{
    public string? Get { get; set; }
    public ApiPaging? Paging { get; set; }
    public Dictionary<string, string>? Errors { get; set; }  // NEW
    public List<T>? Response { get; set; }
}
```

#### SeasonResponse.cs
Added `Errors` field to SeasonsApiResponse:
```csharp
public class SeasonsApiResponse
{
    public string? Get { get; set; }
    public List<object>? Parameters { get; set; }
    public Dictionary<string, string>? Errors { get; set; }  // NEW
    public int? Results { get; set; }
    public List<int>? Response { get; set; }
}
```

---

### 2. Updated RugbyApiClient to Return Errors

All API methods now return tuples with both data AND error messages:

#### Before
```csharp
public async Task<List<CountryResponse>?> GetCountriesAsync()
{
    var request = new RestRequest("/countries");
    var response = await _client.ExecuteAsync<ApiResponse<CountryResponse>>(request);
    return response.Data?.Response;  // Errors ignored!
}
```

#### After
```csharp
public async Task<(List<CountryResponse>? Countries, string? ErrorMessage)> GetCountriesAsync()
{
    var request = new RestRequest("/countries");
    var response = await _client.ExecuteAsync<ApiResponse<CountryResponse>>(request);
    var errorMessage = GetErrorMessage(response.Data?.Errors);
    return (response.Data?.Response, errorMessage);  // Both data and errors returned
}
```

#### Helper Method
```csharp
private static string? GetErrorMessage(Dictionary<string, string>? errors)
{
    if (errors == null || errors.Count == 0)
        return null;

    return string.Join("; ", errors.Values);
}
```

---

### 3. Updated All API Methods to Return Tuples

All methods in RugbyApiClient now return `(Data, ErrorMessage)`:
- `GetSeasonsAsync()`
- `GetCountriesAsync()`
- `GetLeaguesAsync()`
- `GetTeamsAsync()`
- `GetTeamsByLeagueAsync()`
- `GetGamesAsync()`
- `GetGamesBySeasonAsync()`
- `GetGamesByLeagueAndSeasonAsync()`
- `GetGamesByLeagueAsync()`
- `GetGamesByLeagueAndSeasonsAsync()`

---

### 4. Updated Program.cs to Display Errors

#### Before
```csharp
var countries = await _apiClient.GetCountriesAsync();
if (countries != null && countries.Count > 0)
{
    // Process countries
}
else
{
    Console.WriteLine("⚠ No countries retrieved from API\n");  // Silent failure
}
```

#### After
```csharp
var (countries, errorMessage) = await _apiClient.GetCountriesAsync();

if (errorMessage != null)
{
    Console.WriteLine($"⚠ API Error: {errorMessage}\n");  // Display error!
    return;
}

if (countries != null && countries.Count > 0)
{
    // Process countries
}
else
{
    Console.WriteLine("⚠ No countries retrieved from API\n");
}
```

---

## Files Modified

| File | Changes |
|------|---------|
| `DTOs/ApiResponse.cs` | Added `Errors` field to capture API errors |
| `DTOs/SeasonResponse.cs` | Added `Errors` field to SeasonsApiResponse |
| `Services/RugbyApiClient.cs` | Changed all methods to return `(Data, ErrorMessage)` tuples |
| `Program.cs` | Updated all API call handlers to check for and display error messages |
| `Examples/SeasonsAndLeaguesExamples.cs` | Updated example to handle new tuple return type |

---

## Benefits

✅ **Error Visibility**: API errors are now clearly displayed to the user  
✅ **Better Debugging**: Error messages help identify why data fetching failed  
✅ **Plan Limitations**: Users can see "Free plans do not have access" errors  
✅ **Rate Limiting**: Future errors about rate limits will be visible  
✅ **Type Safety**: Tuple returns are type-safe and clear  

---

## Example Output

### Before (Silent Failure)
```
Fetching games from API...
⚠ No games retrieved from API
```

### After (Clear Error)
```
Fetching games from API...
⚠ API Error: Free plans do not have access to this season, try from 2021 to 2023.
```

---

## Usage Pattern

```csharp
// Call API method
var (data, errorMessage) = await apiClient.GetGamesAsync();

// Check for errors first
if (errorMessage != null)
{
    Console.WriteLine($"⚠ API Error: {errorMessage}");
    return;
}

// Process data
if (data != null && data.Count > 0)
{
    // Use data
}
```

---

## Build Status

✅ **Compilation**: Successful  
✅ **All Tests**: Pass  
✅ **Ready**: Production  

---

## Summary

The application now properly captures and displays API error messages, giving users clear feedback about why data retrieval may have failed. This is especially important for free API plans that have limitations on certain seasons or endpoints.
