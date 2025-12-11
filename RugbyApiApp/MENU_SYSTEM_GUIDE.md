# Interactive Console Menu System - User Guide

## Overview

The Rugby API Console Application now features an interactive menu system that allows users to browse data, check completion status, and selectively fetch data from the API.

## Main Menu

When you run the application, you'll see the main menu with the following options:

```
??????????????????????????????????????????????
?   Rugby API Data Management System         ?
??????????????????????????????????????????????

?? Current Data Status:
  � Countries:  220/220 (100.0%)
  � Seasons:      5/5 (100.0%)
  � Leagues:     25/25 (100.0%)
  � Teams:       38/38 (100.0%)
  � Players:    450/450 (100.0%)
  � Matches:    321/321 (100.0%)

?? Main Menu:
  [1] Browse & Fetch Countries
  [2] Browse & Fetch Seasons
  [3] Browse & Fetch Leagues
  [4] Browse & Fetch Teams
  [5] Browse & Fetch Matches
  [6] View All Stored Data
  [7] Auto-Fetch All Incomplete Data
  [8] Clear All Data
  [0] Exit
```

### Main Menu Options

| Option | Description |
|--------|-------------|
| **[1] Browse & Fetch Countries** | Navigate countries, see completion status, fetch from API |
| **[2] Browse & Fetch Seasons** | Navigate seasons, see completion status, fetch from API |
| **[3] Browse & Fetch Leagues** | Navigate leagues, see completion status, fetch from API |
| **[4] Browse & Fetch Teams** | Browse teams, see incomplete teams needing players, fetch data |
| **[5] Browse & Fetch Matches** | View matches in database |
| **[6] View All Stored Data** | Display all stored data summary |
| **[7] Auto-Fetch All Incomplete Data** | Automatically fetch all missing data |
| **[8] Clear All Data** | Delete all data from database (with confirmation) |
| **[0] Exit** | Close the application |

---

## Data Status Indicators

Throughout the menu system, you'll see these indicators:

| Indicator | Meaning |
|-----------|---------|
| **?** | Data is complete (IsDataComplete = true) |
| **?** | Data is incomplete (IsDataComplete = false) - needs fetching |
| **??** | Statistics display |
| **??** | Countries data |
| **??** | Seasons data |
| **??** | Leagues data |
| **??** | Teams data |
| **??** | Matches data |

---

## Browsing Countries

### Countries Menu
```
??????????????????????????????????????????????
?         Countries Management               ?
??????????????????????????????????????????????

?? Countries: 220 total
   ? Complete: 220
   ? Incomplete: 0

?? Countries in Database:
  ? England (GB)
  ? France (FR)
  ? Ireland (IE)
  ... (showing 10 of 220)

?? Menu:
  [1] Fetch Countries from API
  [2] View All Countries (paginated)
  [0] Back to Main Menu
```

### Options:
- **[1] Fetch** - Downloads all countries from API and stores with CDN-optimized flags
- **[2] View All** - Paginated view of all countries (10 per page)
- **[0] Back** - Return to main menu

### Paginated View:
```
??????????????????????????????????????????????
?       Countries (Page 1/22)                ?
??????????????????????????????????????????????

  ? England                         (GB)
  ? France                          (FR)
  ? Ireland                         (IE)
  ...

[N] Next | [P] Previous | [0] Back
```

---

## Browsing Seasons

### Seasons Menu
```
?? Seasons: 5 total
   ? Complete: 5
   ? Incomplete: 0

?? Seasons in Database:
  ? Season 2024 (CURRENT)
  ? Season 2023
  ...

?? Menu:
  [1] Fetch Seasons from API
  [2] View All Seasons
  [0] Back to Main Menu
```

### Options:
- **[1] Fetch** - Downloads seasons from API
- **[2] View All** - Shows all seasons with dates
- **[0] Back** - Return to main menu

### Full View:
```
? Season 2024 (CURRENT) 2024-09-01 to 2025-05-31
? Season 2023 2023-09-01 to 2024-05-31
? Season 2022 (incomplete)
```

---

## Browsing Leagues

### Leagues Menu
```
?? Leagues: 25 total
   ? Complete: 25
   ? Incomplete: 0

?? Leagues in Database:
  ? Top 14 (FR)
  ? Gallagher Premiership (GB)
  ...

?? Menu:
  [1] Fetch Leagues from API
  [2] View All Leagues (paginated)
  [0] Back to Main Menu
```

### Options:
- **[1] Fetch** - Downloads leagues from API with CDN-optimized logos
- **[2] View All** - Paginated view of all leagues
- **[0] Back** - Return to main menu

---

## Browsing Teams

### Teams Menu
```
?? Teams: 38 total
   ? Complete (w/ players): 30
   ? Incomplete (need players): 8

?? Teams Needing Player Data:
  � Team A
  � Team B
  ...

?? Recent Teams:
  ? England
  ? France
  ...

?? Menu:
  [1] Fetch Teams & Matches for Season
  [2] Fetch Players for Incomplete Teams
  [3] View All Teams (paginated)
  [0] Back to Main Menu
```

### Options:
- **[1] Fetch Teams & Matches** - Select season and fetch teams/matches
- **[2] Fetch Players** - Fetch players for teams missing player data
- **[3] View All** - Paginated view of all teams with completion status
- **[0] Back** - Return to main menu

### Selecting Season for Teams Fetch:
```
Available Seasons:
  [1] Season 2024 (CURRENT)
  [2] Season 2023
  [3] Season 2022

Select season (or 0 to cancel): 1
```

---

## Browsing Matches

### Matches Menu
```
?? Matches: 321 total
   ? Complete: 321
   ? Incomplete: 0

?? Recent Matches:
  � England vs France (Completed) [27-24]
  � Scotland vs Wales (Completed) [32-20]
  ...

?? Menu:
  [1] View All Matches (paginated)
  [0] Back to Main Menu
```

### Options:
- **[1] View All** - Paginated view of all matches with scores
- **[0] Back** - Return to main menu

---

## Auto-Fetch Mode

### Auto-Fetch All Data
```
??????????????????????????????????????????????
?    Auto-Fetching All Incomplete Data       ?
??????????????????????????????????????????????

? Fetching Countries...
? Stored 220 countries in database

? Fetching Seasons...
? Stored 5 seasons in database

? Fetching Leagues...
? Stored 25 leagues in database

? Fetching Teams & Matches...
  Retrieved 38 teams
  Retrieved 321 matches

? Fetching Players...
? Complete!
```

This option:
1. Checks for incomplete countries and fetches if needed
2. Checks for incomplete seasons and fetches if needed
3. Checks for incomplete leagues and fetches if needed
4. Checks for incomplete teams/matches and fetches if needed
5. Checks for incomplete players and fetches if needed
6. Saves significant API quota on repeated runs

---

## Clear All Data

```
??????????????????????????????????????????????
?         Clear All Data                     ?
??????????????????????????????????????????????

? This will delete ALL data from the database.
Are you sure? (yes/no): yes

? All data cleared successfully.
```

**Warning**: This deletes all data. You'll need to re-fetch everything on the next run.

---

## Data Navigation Patterns

### Pattern 1: Browse Then Fetch
```
Main Menu
  ? [1] Countries
    ? Shows summary (0 countries)
    ? [1] Fetch Countries
    ? Shows 220 countries fetched
    ? [2] View All to browse
```

### Pattern 2: Check Incomplete and Fetch
```
Main Menu (shows status)
  ? [4] Teams
  ? Shows "8 teams incomplete"
  ? [2] Fetch Players for Incomplete Teams
  ? Fetches only the 8 teams missing players
```

### Pattern 3: Full Auto-Fetch
```
Main Menu
  ? [7] Auto-Fetch All Incomplete Data
  ? Fetches everything in order
  ? Returns to main menu
```

---

## Smart Features

### 1. Completion Tracking
- Each menu shows how many items are complete vs incomplete
- Only incomplete items are fetched when you select "Fetch"
- On second run, fetching is skipped if all items are complete

### 2. Hierarchical Navigation
- Main menu shows overall status
- Sub-menus show category-specific status
- Can navigate between categories easily

### 3. Pagination
- Large datasets are paginated (10 items per page)
- Easy navigation with [N]ext, [P]revious
- Shows progress (Page 1/22)

### 4. Visual Indicators
- ? = Complete (green in some terminals)
- ? = Incomplete (yellow/red in some terminals)
- Emojis help identify data types

### 5. Breadcrumb Navigation
- Always option [0] to go back
- Clear titles showing current menu
- Easy to navigate back to main menu

---

## Typical Usage Scenarios

### Scenario 1: First Time Setup
```
1. Run application
2. Main menu shows all data at 0%
3. Select [7] Auto-Fetch All Incomplete Data
4. Wait for all data to fetch
5. View results in [6] View All Stored Data
6. Exit
```

### Scenario 2: Browse Teams and Fetch Players
```
1. Run application
2. Main menu shows teams at 80% (20 need players)
3. Select [4] Browse & Fetch Teams
4. Shows 8 incomplete teams
5. Select [2] Fetch Players for Incomplete Teams
6. Fetches only those 8 teams
7. Select [3] View All Teams to see updated status
8. Return and select [0] Exit
```

### Scenario 3: Browse All Seasons
```
1. Run application
2. Select [2] Browse & Fetch Seasons
3. Shows 5 seasons (all complete)
4. Select [2] View All Seasons
5. See full list with dates
6. Select [0] Back
7. Select [0] Back again to main menu
```

### Scenario 4: Check Data Before Processing
```
1. Run application
2. Main menu shows current status
3. If anything is incomplete:
   - Select [7] Auto-Fetch
   - Complete the fetch
4. Select [6] View All Stored Data
5. Review what was fetched
6. Exit
```

---

## Status Summary

At any point, you can see:

- **Main Menu**: Overall completion percentages for all data types
- **Sub-menus**: Specific count of complete vs incomplete items
- **Fetch operations**: Progress messages as data is retrieved
- **View screens**: Visual indicators of which items are complete

---

## Tips & Tricks

### Tip 1: Check Status First
Always start by checking the main menu to see what needs fetching.

### Tip 2: Selective Fetching
You can fetch individual categories rather than auto-fetching all:
```
Main Menu ? [1] Countries ? [1] Fetch
```

### Tip 3: Browse While Incomplete
You can view all data even if it's incomplete:
```
Main Menu ? [1] Countries ? [2] View All
```
The view will show completion status with ?/?.

### Tip 4: API Quota Savings
On subsequent runs, incomplete items are not re-fetched:
```
Run 1: Fetches 220 countries (1 API call)
Run 2: Shows "All countries complete, skipping API call"
```

### Tip 5: Reset If Needed
To start fresh:
```
Main Menu ? [8] Clear All Data ? [yes]
```

---

## Error Handling

If an error occurs during fetch:
```
? Error fetching countries: Network timeout

[Continue with partial data or retry]
```

Items that fail remain incomplete and will be retried on the next fetch attempt.

---

## Keyboard Navigation

- **Number keys (0-9)**: Select menu options
- **N/P keys**: Navigate paginated views (case-insensitive)
- **Any key**: Continue after messages
- **Ctrl+C**: Force exit (use [0] instead)

---

## Performance Notes

- First run: All data is fetched
- Subsequent runs: Only missing data is fetched
- Menu operations: All local (very fast)
- Data fetches: Depends on API speed and rate limits
- Large pagination: Works smoothly up to 1000+ items

---

## Summary

The interactive menu system provides:

? **Easy Navigation** - Browse all data types  
? **Status Visibility** - See completion percentages  
? **Smart Fetching** - Only fetch missing data  
? **Flexible Options** - Auto or selective fetching  
? **User-Friendly** - Clear menus and indicators  
? **Safe Operations** - Confirmation for destructive actions  

Enjoy exploring your Rugby API data!
