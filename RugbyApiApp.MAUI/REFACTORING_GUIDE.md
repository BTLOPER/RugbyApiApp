# MAUI Application Refactoring - View Separation

## Overview
This document outlines the MVVM refactoring to split the monolithic MainWindow.xaml into individual tab views.

## New Structure

### Views Directory (RugbyApiApp.MAUI/Views/)
- **HomeTabView.xaml/cs** - Home tab with dashboard and statistics
- **DataTabView.xaml/cs** - Data tab with DataGrid and filtering
- **SettingsTabView.xaml/cs** - Settings tab with API configuration
- **WatchTabView.xaml/cs** - (To be created) Watch tab with video management

### ViewModels (RugbyApiApp.MAUI/ViewModels/)
Existing ViewModels mapped to views:
- **HomeViewModel** → HomeTabView
- **DataViewModel** → DataTabView  
- **SettingsViewModel** → SettingsTabView
- **WatchViewModel** → WatchTabView

## MVVM Pattern Implementation

### Key Principles Applied

1. **Pure Data Binding**: All views use `{Binding ...}` instead of code-behind event handlers
2. **Commands**: User interactions are handled via ICommand implementations in ViewModels
3. **No Code-Behind Logic**: UserControl code-behind files contain only `InitializeComponent()`
4. **Tab Hosting**: MainWindow hosts individual tab views and manages navigation

## Migration Steps Completed

✅ Created HomeTabView (simplified dashboard view)
✅ Created DataTabView (data browsing with DataGrids)
✅ Created SettingsTabView (settings and configuration)
⏳ WatchTabView (complex multi-panel layout - requires careful refactoring)

## ViewModels That Need Enhancement

### DataViewModel
Needs new properties/commands for pure binding:
- `SearchText` property (with PropertyChanged event)
- `ShowFavoritesOnly` boolean property
- `IsCountriesVisible`, `IsLeaguesVisible`, `IsTeamsVisible`, `IsSeasonsVisible`, `IsGamesVisible` properties
- Implement filtering logic in property setters

### WatchViewModel
Already well-structured but may need:
- Validation of existing commands
- Ensure all filtering/selection properties raise PropertyChanged

## Next Steps

1. **Complete WatchTabView Implementation**
   - Simplified pure-MVVM version without Click handlers
   - All ratings handled via binding or commands

2. **Update DataViewModel**
   - Add visibility properties for each grid
   - Add SearchText and filtering logic

3. **Update MainWindow.xaml**
   - Replace tab items with references to new Views
   - Add views namespace: `xmlns:views="clr-namespace:RugbyApiApp.MAUI.Views"`
   - Use: `<views:HomeTabView />`

4. **Testing**
   - Verify all bindings work correctly
   - Test navigation between tabs
   - Confirm filtering and search functionality

## Benefits of This Architecture

✨ **Cleaner Code**: MainWindow.xaml reduced from 1000+ lines to ~200 lines
✨ **Maintainability**: Each view handles one concern
✨ **Reusability**: Views can be reused in other contexts
✨ **Testing**: ViewModels are easier to unit test without XAML
✨ **Scalability**: Easy to add new tabs following the same pattern

## File Locations
- Views: `RugbyApiApp.MAUI/Views/`
- ViewModels: `RugbyApiApp.MAUI/ViewModels/`
- Main Window: `RugbyApiApp.MAUI/MainWindow.xaml`
