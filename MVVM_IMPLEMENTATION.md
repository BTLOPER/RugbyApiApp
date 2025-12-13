# MVVM Pattern Implementation ✅

The MAUI application now implements the **Model-View-ViewModel (MVVM)** architectural pattern, providing a clean separation of concerns, improved testability, and maintainability.

## Architecture Overview

### MVVM Pattern Structure

```
View (XAML)
    ↓ (DataContext binding)
ViewModel (Business Logic)
    ↓ (Dependency Injection)
Model (Data & Services)
    ↓ (Data Access)
Database / API
```

## Components

### 1. Views

**Files:**
- `RugbyApiApp.MAUI/MainWindow.xaml` - Main window UI
- `RugbyApiApp.MAUI/MainWindow.xaml.cs` - Code-behind with minimal logic

**Responsibilities:**
- Display UI elements
- Bind to ViewModel properties
- Forward user interactions to ViewModel commands
- Minimal code-behind (only navigation and confirmation dialogs)

### 2. ViewModels

#### BaseViewModel
**File:** `RugbyApiApp.MAUI/ViewModels/BaseViewModel.cs`

```csharp
public abstract class BaseViewModel : INotifyPropertyChanged
{
    protected void OnPropertyChanged([CallerMemberName] string? propertyName = null);
    protected bool SetProperty<T>(ref T backingField, T value, ...);
    protected T GetProperty<T>(ref T? backingField, Func<T> defaultValueFactory, ...);
}
```

**Features:**
- Implements `INotifyPropertyChanged` for property change notifications
- Provides `SetProperty` helper for binding updates
- Provides `OnPropertyChanged` for manual property notifications
- Uses CallerMemberName to reduce string literals

#### HomeViewModel
**File:** `RugbyApiApp.MAUI/ViewModels/HomeViewModel.cs`

```csharp
public class HomeViewModel : BaseViewModel
{
    // Properties for dashboard statistics
    public int CountriesCount { get; set; }
    public double CountriesProgress { get; set; }
    public string CountriesPercent { get; set; }
    // ... similar for other data types
    
    public ICommand RefreshStatsCommand { get; }
    
    // Auto-refresh timer (5 seconds)
    private async Task RefreshStatsAsync();
    private void StartRefreshTimer();
    public void Cleanup();
}
```

**Responsibilities:**
- Display real-time dashboard statistics
- Auto-refresh statistics every 5 seconds
- Manage refresh timer lifecycle
- Expose RefreshStatsCommand for manual refresh

**Data Binding:**
```xaml
<!-- Bind to ViewModel properties -->
<TextBlock Text="{Binding CountriesPercent}" />
<ProgressBar Value="{Binding CountriesProgress}" />
```

#### DataViewModel
**File:** `RugbyApiApp.MAUI/ViewModels/DataViewModel.cs`

```csharp
public class DataViewModel : BaseViewModel
{
    // Properties
    public string? SelectedDataType { get; set; }
    public object? GridData { get; set; }
    public bool IsLoading { get; set; }
    public string StatusMessage { get; set; }
    
    // Commands
    public ICommand RefreshCommand { get; }
    public ICommand LoadCountriesCommand { get; }
    public ICommand FetchCountriesCommand { get; }
    // ... other commands
    
    // Methods
    private async Task LoadDataByTypeAsync();
    private async Task LoadCountriesAsync();
    private async Task FetchCountriesAsync();
    // ... other methods
}
```

**Responsibilities:**
- Handle data type selection and loading
- Load data from database
- Fetch data from API
- Manage UI loading state and status messages
- Support all data types (Countries, Seasons, Leagues, Teams, Games)

#### SettingsViewModel
**File:** `RugbyApiApp.MAUI/ViewModels/SettingsViewModel.cs`

```csharp
public class SettingsViewModel : BaseViewModel
{
    // Properties
    public string ApiKey { get; set; }
    public string StatusMessage { get; set; }
    public bool IsLoading { get; set; }
    public string DatabasePath { get; set; }
    public string StorageInfo { get; set; }
    
    // Commands
    public ICommand SaveApiKeyCommand { get; }
    public ICommand ClearApiKeyCommand { get; }
    public ICommand TestApiKeyCommand { get; }
    public ICommand AutoFetchAllDataCommand { get; }
    public ICommand ShowDatabasePathCommand { get; }
    public ICommand ClearAllDataCommand { get; }
    
    // Methods
    public void SetApiClient(RugbyApiClient? apiClient);
    private async Task SaveApiKeyAsync();
    private async Task TestApiKeyAsync();
    private async Task AutoFetchAllDataAsync();
    // ... other methods
}
```

**Responsibilities:**
- Manage API key configuration
- Test API connections
- Manage auto-fetch operations
- Display database path information
- Clear data operations

#### MainViewModel
**File:** `RugbyApiApp.MAUI/ViewModels/MainViewModel.cs`

```csharp
public class MainViewModel : BaseViewModel
{
    public HomeViewModel HomeViewModel { get; }
    public DataViewModel DataViewModel { get; }
    public SettingsViewModel SettingsViewModel { get; }
    
    public void UpdateApiClient(RugbyApiClient? apiClient);
    public void Cleanup();
}
```

**Responsibilities:**
- Coordinate all child ViewModels
- Initialize services
- Manage application lifecycle
- Handle API client updates across all tabs

### 3. Commands

#### RelayCommand Classes
**File:** `RugbyApiApp.MAUI/ViewModels/RelayCommand.cs`

```csharp
// Synchronous command
public class RelayCommand : ICommand
{
    public RelayCommand(Action<object?> execute, Predicate<object?>? canExecute = null);
}

// Generic synchronous command
public class RelayCommand<T> : ICommand
{
    public RelayCommand(Action<T?> execute, Predicate<T?>? canExecute = null);
}

// Async command
public class AsyncRelayCommand : ICommand
{
    public AsyncRelayCommand(Func<object?, Task> execute, Predicate<object?>? canExecute = null);
}

// Generic async command
public class AsyncRelayCommand<T> : ICommand
{
    public AsyncRelayCommand(Func<T?, Task> execute, Predicate<T?>? canExecute = null);
}
```

**Features:**
- Support both sync and async operations
- Generic versions for type-safe parameter passing
- Can execute predicate for enabling/disabling
- Manages execution state (prevents multiple concurrent executions)

**Usage:**
```csharp
public ICommand SaveApiKeyCommand { get; }

public SettingsViewModel()
{
    SaveApiKeyCommand = new AsyncRelayCommand(_ => SaveApiKeyAsync());
}

// In XAML
<Button Command="{Binding SaveApiKeyCommand}" Content="Save" />
```

### 4. Data Flow

#### Property Binding
```
View (XAML) → ViewModel Property
   ↓ (INotifyPropertyChanged)
Database/API
   ↓ (SetProperty)
Updates UI automatically
```

#### Command Execution
```
User Click → Button.Command
   ↓ (ICommand.Execute)
ViewModel.Command
   ↓ (Async operation)
Update ViewModel Properties
   ↓ (INotifyPropertyChanged)
View automatically updates
```

## Benefits of MVVM

### 1. **Separation of Concerns**
- Views (UI) separate from ViewModels (Logic)
- ViewModels separate from Models (Data)
- Each component has single responsibility

### 2. **Testability**
- ViewModels can be unit tested without UI
- Commands can be tested independently
- Data services can be mocked

### 3. **Code Reusability**
- ViewModels can be reused with different Views
- Commands can be shared across views
- Business logic is centralized

### 4. **Maintainability**
- Clear structure makes code easy to navigate
- Changes to UI don't affect logic
- Logic changes don't require UI updates

### 5. **Binding**
- Automatic UI updates when data changes
- Two-way binding support
- Strongly-typed command parameters

## Component Relationships

```
MainWindow (View)
    ↓ DataContext
MainViewModel
    ├─→ HomeViewModel (HomeTab DataContext)
    ├─→ DataViewModel (DataTab DataContext)  
    └─→ SettingsViewModel (SettingsTab DataContext)
        ↓ Dependency Injection
        ├─→ DataService
        ├─→ SecretsService
        └─→ RugbyApiClient
```

## Lifecycle Management

### Initialization
```csharp
// MainWindow Constructor
public MainWindow()
{
    InitializeComponent();
    
    // Create services
    var config = new ConfigurationBuilder()
        .AddUserSecrets<MainWindow>()
        .Build();
    var secretsService = new SecretsService(config);
    var dataService = new DataService(new RugbyDbContext());
    
    // Create MainViewModel
    _viewModel = new MainViewModel(dataService, secretsService, config);
    
    // Set DataContext
    DataContext = _viewModel;
    
    // Set individual tab DataContexts
    HomeTab.DataContext = _viewModel.HomeViewModel;
    DataTab.DataContext = _viewModel.DataViewModel;
    SettingsTab.DataContext = _viewModel.SettingsViewModel;
}
```

### Cleanup
```csharp
// MainWindow OnClosed
protected override void OnClosed(EventArgs e)
{
    _viewModel?.Cleanup();
    base.OnClosed(e);
}

// HomeViewModel Cleanup
public void Cleanup()
{
    _refreshTimer?.Stop();
    _refreshTimer = null;
}
```

## Command Examples

### Basic Async Command
```csharp
public ICommand RefreshCommand { get; }

public HomeViewModel(DataService dataService)
{
    RefreshCommand = new AsyncRelayCommand(_ => RefreshStatsAsync());
}
```

### Async Command with Validation
```csharp
public ICommand SaveApiKeyCommand { get; }

public SettingsViewModel(...)
{
    SaveApiKeyCommand = new AsyncRelayCommand(
        _ => SaveApiKeyAsync(),
        _ => !string.IsNullOrWhiteSpace(ApiKey) // Can execute predicate
    );
}
```

### Generic Command
```csharp
public ICommand FetchCommand { get; }

public DataViewModel(...)
{
    FetchCommand = new AsyncRelayCommand<string>(
        async (dataType) => await FetchDataAsync(dataType)
    );
}

// Usage: Command="{Binding FetchCommand}" CommandParameter="Countries"
```

## Property Change Notification

### Using SetProperty Helper
```csharp
private string _statusMessage = "";
public string StatusMessage
{
    get => _statusMessage;
    set => SetProperty(ref _statusMessage, value);
}

// Usage - automatically raises PropertyChanged
StatusMessage = "Operation complete";
```

### Manual Notification
```csharp
// For computed properties or complex updates
private void UpdateAllStats()
{
    // ... update logic
    OnPropertyChanged(nameof(CountriesPercent));
    OnPropertyChanged(nameof(LeaguesPercent));
}
```

## Best Practices

✅ **DO:**
- Keep ViewModels focused on single responsibility
- Use ICommand for all user interactions
- Implement INotifyPropertyChanged in ViewModels
- Use async/await for long-running operations
- Bind UI to ViewModel properties
- Use dependency injection for services

❌ **DON'T:**
- Put UI logic in ViewModels
- Directly access UI controls from ViewModel
- Use code-behind for business logic
- Create circular dependencies between ViewModels
- Block UI thread with synchronous operations
- Hardcode services in ViewModels

## Migration Notes

### From Old Code-Behind Approach
**Before:**
```csharp
private async Task LoadDashboardAsync()
{
    var stats = await _dataService.GetCompletionStatsAsync();
    CountriesCount.Text = $"{stats.CompleteCountries} / {stats.TotalCountries}";
}
```

**After:**
```csharp
// ViewModel
public class HomeViewModel : BaseViewModel
{
    public string CountriesCount
    {
        get => _countriesCount;
        set => SetProperty(ref _countriesCount, value);
    }
    
    private async Task RefreshStatsAsync()
    {
        var stats = await _dataService.GetCompletionStatsAsync();
        CountriesCount = $"{stats.CompleteCountries} / {stats.TotalCountries}";
    }
}

// XAML
<TextBlock Text="{Binding CountriesCount}" />
```

## Future Enhancements

Possible improvements to MVVM implementation:
1. **IView interface** - Decouple Views from code-behind
2. **Navigation service** - Centralized tab/window navigation
3. **Message service** - Publish-subscribe for cross-ViewModel communication
4. **Validation service** - Centralized input validation
5. **Logger service** - Dependency-injected logging
6. **Unit tests** - Test ViewModels independently

## Summary

The MVVM implementation provides:

✅ Clean separation of concerns
✅ Fully testable ViewModels
✅ Reactive UI bindings
✅ Centralized business logic
✅ Reduced code-behind
✅ Professional application structure
✅ Scalable architecture for future features

The application now follows industry best practices for WPF/XAML applications! 🎉
