# Interactive Menu System - Quick Start

## Running the Application

```bash
dotnet run
```

You'll see the main menu with data statistics and 9 options.

---

## Main Menu at a Glance

```
[1] Browse & Fetch Countries        ? Manage countries
[2] Browse & Fetch Seasons          ? Manage seasons  
[3] Browse & Fetch Leagues          ? Manage leagues
[4] Browse & Fetch Teams            ? Manage teams & players
[5] Browse & Fetch Matches          ? View matches
[6] View All Stored Data            ? Summary view
[7] Auto-Fetch All Incomplete Data  ? Fetch everything
[8] Clear All Data                  ? Delete everything
[0] Exit                            ? Close app
```

---

## Quick Navigation Guide

### To Fetch Everything:
```
Main Menu ? [7] Auto-Fetch ? Wait ? Done
```

### To Browse Countries:
```
Main Menu ? [1] Countries
  � Shows 10 countries
  � [1] Fetch ? Downloads all
  � [2] View All ? Paginated view (N/P to navigate)
  � [0] Back to main menu
```

### To Fetch Missing Teams/Players:
```
Main Menu ? [4] Teams
  � Shows "X teams incomplete"
  � [2] Fetch Players ? Fetches only incomplete teams
  � [3] View All ? See teams (? = complete, ? = incomplete)
```

### To See All Data:
```
Main Menu ? [6] View All Stored Data ? Summary of everything
```

---

## Status Indicators

| Symbol | Meaning |
|--------|---------|
| **?** | Complete (fetched) |
| **?** | Incomplete (needs fetch) |
| **??** | Statistics |

---

## Example Session

```
???????????????????????????????????
1. App starts ? Main Menu
   Countries:  0/220 (0.0%)
   Seasons:    0/5 (0.0%)
   Leagues:    0/25 (0.0%)
   Teams:      0/38 (0.0%)
   Players:    0/450 (0.0%)
   Matches:    0/321 (0.0%)

2. Select [7] Auto-Fetch All
   ? Fetching countries...
   ? Fetching seasons...
   ? Fetching leagues...
   ? Fetching teams...
   ? Fetching players...

3. Back to Main Menu
   Countries:  220/220 (100.0%) ?
   Seasons:    5/5 (100.0%) ?
   Leagues:    25/25 (100.0%) ?
   Teams:      38/38 (100.0%) ?
   Players:    450/450 (100.0%) ?
   Matches:    321/321 (100.0%) ?

4. Run again next time
   ? All shows complete
   ? No API calls made
   ? 95% quota saved!
```

---

## Common Tasks

### View Countries
```
[1] ? [2] ? Browse pages with N/P ? [0] back
```

### Fetch Missing Players for Teams
```
[4] ? Shows incomplete count ? [2] Fetch Players
```

### See Everything
```
[6] View All Data
```

### Start Over
```
[8] Clear All Data ? Type "yes"
```

---

## Key Features

? **Live Status** - See completion % on main menu  
? **Smart Fetch** - Only fetches missing data  
? **Pagination** - Browse large datasets easily  
? **Clear Menus** - Easy to understand options  
? **Visual Indicators** - ?/? shows status  
? **Safe Reset** - Confirmation before clearing  

---

## Tips

- **First time?** Select [7] Auto-Fetch All
- **Quick browse?** Select [1-5] then [2] View All
- **Check status?** Main menu shows everything
- **Run again?** Auto-skip of complete data saves API quota

---

## Menu Keys

| Key | Action |
|-----|--------|
| 0-8 | Select menu option |
| N | Next page (in views) |
| P | Previous page (in views) |
| Any | Continue after message |

---

**Start with [7] Auto-Fetch to download all data in one go!**
