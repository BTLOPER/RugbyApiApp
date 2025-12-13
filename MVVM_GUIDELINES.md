# MVVM Implementation Guidelines

## Quick Reference Guide for MAUI Application

### File Structure

```
RugbyApiApp.MAUI/
├── MainWindow.xaml          # View (UI)
├── MainWindow.xaml.cs       # Code-behind (minimal logic)
├── App.xaml                 # Application resources
├── App.xaml.cs              # Application startup
└── ViewModels/
    ├── BaseViewModel.cs     # Base class for all ViewModels
    ├── RelayCommand.cs      # Command implementations
    ├── MainViewModel.cs     # Main coordinator ViewModel
    ├── HomeViewModel.cs     # Home tab ViewModel
    ├── DataViewModel.cs     # Data tab ViewModel
    └── SettingsViewModel.cs # Settings tab ViewModel
```

### Creating a New Tab/Feature

#### Step 1: Create ViewModel Class

```csharp
// FeatureViewModel.cs
namespace RugbyApiApp.MAUI.ViewModels
{
    public class FeatureViewModel : BaseViewModel
    {
        private readonly DataService _dataService;
        
        // Properties with change notification
        private string _message = "";
        public string Message
        {
            get => _message;
            set => SetProperty(ref _message, value);
        }
        
        // Commands
        public ICommand LoadCommand { get; }
        public ICommand SaveCommand { get; }
        
        public FeatureViewModel(DataService dataService)
        {
            _dataService = dataService;
            
            // Initialize commands
            LoadCommand = new AsyncRelayCommand(_ => LoadAsync());
            SaveCommand = new AsyncRelayCommand(_ => SaveAsync());
        }
        
        private async Task LoadAsync()
        {
            try
            {
                // Load logic here
                Message = "Loaded successfully";
            }
            catch (Exception ex)
            {
                Message = $"Error: {ex.Message}";
            }
        }
        
        private async Task SaveAsync()
        {
            try
            {
                // Save logic here
                Message = "Saved successfully";
            }
            catch (Exception ex)
            {
                Message = $"Error: {ex.Message}";
            }
        }
    }
}
```

#### Step 2: Update MainViewModel

```csharp
public class MainViewModel : BaseViewModel
{
    public HomeViewModel HomeViewModel { get; }
    public DataViewModel DataViewModel { get; }
    public SettingsViewModel SettingsViewModel { get; }
    public FeatureViewModel FeatureViewModel { get; }  // Add this
    
    public MainViewModel(DataService dataService, SecretsService secretsService, IConfiguration configuration)
    {
        // ... existing code ...
        
        // Create new ViewModel
        FeatureViewModel = new FeatureViewModel(dataService);
    }
}
```

#### Step 3: Add XAML Tab to MainWindow

```xaml
<!-- FeatureTab -->
<TabItem Header="✨ Feature" Style="{StaticResource ModernTabItem}" x:Name="FeatureTab">
    <Grid>
        <!-- Your UI here -->
        <StackPanel>
            <TextBlock Text="{Binding Message}" />
            <Button Content="Load" Command="{Binding LoadCommand}" />
            <Button Content="Save" Command="{Binding SaveCommand}" />
        </StackPanel>
    </Grid>
</TabItem>
```

#### Step 4: Update MainWindow.xaml.cs

```csharp
public MainWindow()
{
    InitializeComponent();
    
    // ... existing initialization code ...
    
    FeatureTab.DataContext = _viewModel.FeatureViewModel;
}
```

### Property Binding Patterns

#### Read-Only Display
```xaml
<TextBlock Text="{Binding PropertyName}" />
<ProgressBar Value="{Binding ProgressValue}" />
```

#### Two-Way Binding
```xaml
<TextBox Text="{Binding EditableText, UpdateSourceTrigger=PropertyChanged}" />
<CheckBox IsChecked="{Binding IsEnabled}" />
```

#### Conditional Display
```xaml
<TextBlock Visibility="{Binding IsLoading, Converter={StaticResource BoolToVisibility}}" Text="Loading..." />
```

### Command Binding Patterns

#### Basic Command
```xaml
<Button Command="{Binding RefreshCommand}" Content="Refresh" />
```

#### Command with Parameter
```xaml
<Button Command="{Binding FetchCommand}" CommandParameter="Countries" Content="Fetch" />
```

#### Command with Condition
```csharp
// In ViewModel
public ICommand SaveCommand { get; }

public SampleViewModel()
{
    // Command can only execute if API key is set
    SaveCommand = new AsyncRelayCommand(
        _ => SaveAsync(),
        _ => !string.IsNullOrEmpty(ApiKey)
    );
}
```

## Property Implementation Checklist

When adding a property to a ViewModel:

✅ **DO:**
- [ ] Create private backing field: `private string _name = "";`
- [ ] Create public property with getter/setter
- [ ] Use `SetProperty` in setter: `SetProperty(ref _name, value);`
- [ ] Bind in XAML: `Text="{Binding Name}"`
- [ ] Initialize in constructor or field declaration

```csharp
private string _name = "";
public string Name
{
    get => _name;
    set => SetProperty(ref _name, value);
}
```

❌ **DON'T:**
- [ ] Use auto-properties (they don't notify changes)
- [ ] Directly access backing field from XAML
- [ ] Forget to raise PropertyChanged
- [ ] Make properties that shouldn't change

## Command Implementation Checklist

When adding a command to a ViewModel:

✅ **DO:**
- [ ] Create public `ICommand` property: `public ICommand MyCommand { get; }`
- [ ] Initialize in constructor: `MyCommand = new AsyncRelayCommand(...)`
- [ ] Pass async method to command: `new AsyncRelayCommand(_ => MyMethodAsync())`
- [ ] Bind in XAML: `Command="{Binding MyCommand}"`
- [ ] Use `async Task` for async operations
- [ ] Handle exceptions in async methods

```csharp
public ICommand SaveCommand { get; }

public FeatureViewModel()
{
    SaveCommand = new AsyncRelayCommand(_ => SaveAsync());
}

private async Task SaveAsync()
{
    try
    {
        // Do work
    }
    catch (Exception ex)
    {
        // Handle error
    }
}
```

❌ **DON'T:**
- [ ] Use click handlers in code-behind
- [ ] Direct method calls from XAML
- [ ] Blocking operations in commands
- [ ] Forget to await async calls
- [ ] Hide exceptions in async methods

## Testing ViewModels

### Unit Test Example

```csharp
using Xunit;
using Moq;

public class HomeViewModelTests
{
    [Fact]
    public async Task RefreshStats_ShouldUpdateProperties()
    {
        // Arrange
        var mockDataService = new Mock<DataService>();
        mockDataService
            .Setup(x => x.GetCompletionStatsAsync())
            .ReturnsAsync(new DataCompletionStats 
            { 
                CompleteCountries = 5,
                TotalCountries = 10
            });
        
        var viewModel = new HomeViewModel(mockDataService.Object);
        
        // Act
        await viewModel.RefreshCommand.Execute(null);
        
        // Assert
        Assert.Equal(5, viewModel.CountriesCount);
    }
}
```

## Common Patterns

### Loading State
```csharp
private bool _isLoading;
public bool IsLoading
{
    get => _isLoading;
    set => SetProperty(ref _isLoading, value);
}

private async Task LoadAsync()
{
    IsLoading = true;
    try
    {
        // Do work
    }
    finally
    {
        IsLoading = false;
    }
}
```

### Status Messages
```csharp
private string _statusMessage = "";
public string StatusMessage
{
    get => _statusMessage;
    set => SetProperty(ref _statusMessage, value);
}

private async Task OperationAsync()
{
    try
    {
        StatusMessage = "Working...";
        // Do work
        StatusMessage = "✅ Success!";
    }
    catch (Exception ex)
    {
        StatusMessage = $"❌ Error: {ex.Message}";
    }
}
```

### Conditional Command Execution
```csharp
public ICommand FetchCommand { get; }

public DataViewModel(DataService dataService, RugbyApiClient? apiClient)
{
    _dataService = dataService;
    _apiClient = apiClient;
    
    // Command can only execute if API client is configured
    FetchCommand = new AsyncRelayCommand(
        _ => FetchAsync(),
        _ => _apiClient != null
    );
}
```

### Command Parameter
```csharp
// ViewModel
public ICommand LoadDataCommand { get; }

public DataViewModel()
{
    LoadDataCommand = new AsyncRelayCommand<string>(async (dataType) =>
    {
        await LoadAsync(dataType);
    });
}

private async Task LoadAsync(string dataType)
{
    // Use dataType parameter
}

// XAML
<Button Command="{Binding LoadDataCommand}" CommandParameter="Countries" />
```

## Debugging Tips

### View Model Not Updating?
1. Check property uses `SetProperty` in setter
2. Verify XAML binding path is correct
3. Check DataContext is set properly
4. Use `UpdateSourceTrigger=PropertyChanged` for TextBox

### Command Not Executing?
1. Check command is initialized in constructor
2. Verify XAML binding syntax: `Command="{Binding CommandName}"`
3. Check `CanExecute` predicate (if used)
4. Ensure event handler is removed (if used)

### Memory Leaks?
1. Unsubscribe from events in `Cleanup()` method
2. Stop timers/tasks properly
3. Dispose resources in `OnClosed` event
4. Call `_viewModel?.Cleanup()` in MainWindow.OnClosed

## Performance Tips

1. **Lazy Loading** - Don't load all data at startup
2. **Virtual Lists** - Use virtualization for large lists
3. **Async Operations** - Always use async for I/O operations
4. **Debouncing** - Debounce frequently-called operations
5. **Caching** - Cache expensive operations

```csharp
// Example: Debounced search
private DateTime _lastSearch;

public ICommand SearchCommand { get; }

public ViewModel()
{
    SearchCommand = new AsyncRelayCommand(_ => SearchAsync());
}

private async Task SearchAsync()
{
    // Debounce: wait 300ms since last search
    if ((DateTime.Now - _lastSearch).TotalMilliseconds < 300)
        return;
    
    _lastSearch = DateTime.Now;
    // Do search
}
```

## Summary Checklist

When implementing MVVM:

- [ ] Create ViewModel class inheriting from `BaseViewModel`
- [ ] Add properties with `SetProperty` helper
- [ ] Add commands as `ICommand` properties
- [ ] Initialize commands in constructor
- [ ] Create async methods for operations
- [ ] Implement error handling
- [ ] Bind properties in XAML
- [ ] Bind commands in XAML
- [ ] Set DataContext in MainWindow
- [ ] Implement cleanup for resources
- [ ] Add ViewModel to MainViewModel
- [ ] Test ViewModel independently

Following these patterns ensures a clean, maintainable, and professional MVVM implementation! 🚀
