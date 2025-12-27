# View Refactoring - Resolution Action Plan

## Issue #1: UserControl Type Recognition

### Error Message
```
XDG0008: The name "HomeTabView" does not exist in the namespace "clr-namespace:RugbyApiApp.MAUI.Views".
XLS0414: The type 'RugbyApiApp.MAUI.Views.HomeTabView' was not found.
```

### Why This Happens
The XAML designer looks for compiled types. The `HomeTabView` code-behind (`.xaml.cs` file) creates the partial class, but it must be compiled first.

### Resolution

**Step 1**: Clean Build
```bash
cd C:\Users\Brand\source\repos\RugbyApiApp\RugbyApiApp.MAUI
dotnet clean
```

**Step 2**: Rebuild
```bash
dotnet build
```

**Step 3**: Visual Studio Cache (if still failing)
- Close Visual Studio completely
- Delete `bin` and `obj` folders
- Reopen solution
- Rebuild

---

## Issue #2: MainWindow.xaml.cs References Missing Controls

### Problem
These methods reference controls that are no longer in MainWindow.xaml:

```csharp
// Line 140-142: DataTypeCombo no longer in MainWindow
OnDataTypeChanged() // event handler for ComboBox

// Line 156-189: DataGrids moved to DataTabView
UpdateDataGridVisibility() // method that hides/shows DataGrids

// Line 243-245: SearchBox and FavoritesCheckBox moved
OnSearchTextChanged() // handles TextBox changes
OnFavoritesFilterChanged() // handles CheckBox changes

// In Watch tab: RatingStar_Click() // handles star rating clicks
```

### Solution

Remove or comment out these methods in `MainWindow.xaml.cs`. They're now handled by the individual view ViewModels.

**Affected Methods to Remove/Stub**:
1. `OnDataTypeChanged` - Now handled by DataViewModel
2. `OnRefreshClicked` - Now handled by DataViewModel
3. `OnSearchTextChanged` - Now handled by DataViewModel
4. `OnFavoritesFilterChanged` - Now handled by DataViewModel
5. `DataGrid_CellEditEnding` - Now handled by DataViewModel
6. `RatingStar_Click` - Now handled by WatchViewModel (when WatchTabView is created)

---

## Issue #3: DataViewModel Property Errors

### Error Messages
```
CS1061: 'Country' does not contain a definition for 'IsFavorite'
CS1061: 'Season' does not contain a definition for 'Name'
```

### Root Cause
Lines 500 and 512 in `DataViewModel.cs` reference properties that don't exist in the domain models.

### Solution A: Add Missing Properties (PREFERRED)
Check your domain models and add properties if needed:

**Country Model**:
```csharp
public class Country
{
    public int Id { get; set; }
    public string Name { get; set; }
    public string Code { get; set; }
    // ADD THIS if missing:
    public bool IsFavorite { get; set; }
}
```

**Season Model**:
```csharp
public class Season
{
    public int Id { get; set; }
    public int Year { get; set; }
    // ADD THIS if missing:
    public string Name { get; set; } // or remove from filtering
    public bool Current { get; set; }
}
```

### Solution B: Fix Filtering Logic (If Properties Shouldn't Exist)
If the properties shouldn't exist, adjust the LINQ query:

```csharp
// BEFORE (line 500):
.Where(c => !ShowFavoritesOnly || c.IsFavorite)

// AFTER (if Country doesn't have IsFavorite):
.Where(c => !ShowFavoritesOnly) // remove IsFavorite check
```

---

## Issue #4: Watch Tab Not Yet Separated

### Current State
The Watch tab remains in `MainWindow.xaml` with all its content because:
1. It's complex with multi-panel layout
2. It has interactive rating stars with Click handlers
3. It needs careful refactoring to maintain functionality

### When to Tackle
This can be done **in a future commit** after the Home, Data, and Settings tabs are working.

---

## Recommended Order of Execution

### ✅ Step 1: Force Rebuild (Execute Now)
```bash
# In PowerShell/Command Prompt
cd C:\Users\Brand\source\repos\RugbyApiApp
dotnet clean
dotnet build --verbose 2>&1 | Tee-Object -FilePath build.log
```

### ⏳ Step 2: If Step 1 Fails - Fix MainWindow.xaml.cs (Conditional)
If the build still fails with "Control XYZ not found" errors:
- Open `RugbyApiApp.MAUI\MainWindow.xaml.cs`
- Find and remove/comment out event handlers that reference moved controls
- Example:

```csharp
// OLD - REMOVE OR COMMENT OUT:
public void OnDataTypeChanged(object sender, SelectionChangedEventArgs e)
{
    // This method is no longer needed
    // Logic moved to DataViewModel
}

// NEW - Keep:
public void OnViewStatisticsClicked(object sender, RoutedEventArgs e)
{
    // Buttons in Home tab still need these for now
}
```

### ⏳ Step 3: Fix DataViewModel Issues (Conditional)
If build fails with "Property not found" errors:
- Option A: Add missing properties to domain models
- Option B: Fix LINQ filtering in DataViewModel.cs lines 500, 512

---

## Testing Checklist

After resolving all issues, verify:

- [ ] Solution builds without errors
- [ ] Solution builds without warnings  
- [ ] Home tab displays statistics correctly
- [ ] Data tab shows all 5 data grids
- [ ] Data tab filtering works (search, favorites)
- [ ] Settings tab displays and functions
- [ ] Watch tab still works (not yet refactored)
- [ ] All buttons navigate correctly

---

## Quick Reference: Files Modified

| File | Changes |
|------|---------|
| MainWindow.xaml | Added `xmlns:views`, replaced 3 tab contents with `<views:*TabView />` |
| MainWindow.xaml.cs | Needs orphaned event handlers removed |
| Views/HomeTabView.xaml | ✅ NEW - Dashboard content |
| Views/HomeTabView.xaml.cs | ✅ NEW - Simple code-behind |
| Views/DataTabView.xaml | ✅ NEW - Data browsing content |
| Views/DataTabView.xaml.cs | ✅ NEW - Simple code-behind |
| Views/SettingsTabView.xaml | ✅ NEW - Settings content |
| Views/SettingsTabView.xaml.cs | ✅ NEW - Simple code-behind |
| DataViewModel.cs | ⚠️ NEEDS UPDATE - Fix property errors |

---

## Support Resources

- **MVVM Pattern**: https://docs.microsoft.com/en-us/dotnet/maui/concepts/mvvm
- **UserControl**: https://docs.microsoft.com/en-us/dotnet/desktop/wpf/controls/usercontrol-overview
- **WPF Binding**: https://docs.microsoft.com/en-us/dotnet/desktop/wpf/data/binding-overview
