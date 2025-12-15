# Icon Replacement Quick Reference

## Quick Copy-Paste Replacements

### Add Namespace at Top of MainWindow.xaml

```xaml
xmlns:iconPacks="http://metro.mahapps.com/winfx/xaml/iconpacks"
```

---

## Header (Rugby Ball Icon)

### BEFORE
```xaml
<TextBlock Text="🏉" FontSize="28" Margin="0,0,12,0"/>
```

### AFTER
```xaml
<iconPacks:MaterialDesignIconExt Kind="Rugby" Width="32" Height="32" Foreground="White" Margin="0,0,12,0" VerticalAlignment="Center"/>
```

---

## Tab Headers

### BEFORE
```xaml
<TabItem Header="📊 Home" Style="{StaticResource ModernTabItem}" x:Name="HomeTab">
```

### AFTER
```xaml
<TabItem Style="{StaticResource ModernTabItem}" x:Name="HomeTab">
    <TabItem.Header>
        <StackPanel Orientation="Horizontal" VerticalAlignment="Center">
            <iconPacks:MaterialDesignIconExt Kind="ChartBox" Width="16" Height="16" Margin="0,0,4,0"/>
            <TextBlock Text="Home"/>
        </StackPanel>
    </TabItem.Header>
```

Repeat for other tabs:
- `📋 Data` → `FormatListBulleted` + "Data"
- `🎬 Watch` → `MoviePlay` + "Watch"  
- `⚙️ Settings` → `Cog` + "Settings"

---

## Dashboard Cards (Countries, Seasons, Leagues, Teams, Games)

### BEFORE
```xaml
<TextBlock Text="🌍 Countries" Style="{StaticResource SubHeadingStyle}" Margin="0,0,0,12"/>
```

### AFTER
```xaml
<StackPanel Orientation="Horizontal" Margin="0,0,0,12">
    <iconPacks:MaterialDesignIconExt Kind="Globe" Width="24" Height="24" Margin="0,0,8,0" VerticalAlignment="Center"/>
    <TextBlock Text="Countries" Style="{StaticResource SubHeadingStyle}" VerticalAlignment="Center"/>
</StackPanel>
```

Repeat for other cards:
- `📅 Seasons` → `Calendar`
- `🏆 Leagues` → `Trophy`
- `🏉 Teams` → `Sports`
- `🎮 Games` → `GamepadVariant`

---

## Quick Action Buttons (Home Tab)

### BEFORE
```xaml
<Button Content="📊 View Full Statistics" Style="{StaticResource PrimaryButtonStyle}" Click="OnViewStatisticsClicked" Height="40" Margin="0,0,0,12"/>
```

### AFTER
```xaml
<Button Style="{StaticResource PrimaryButtonStyle}" Click="OnViewStatisticsClicked" Height="40" Margin="0,0,0,12">
    <StackPanel Orientation="Horizontal" VerticalAlignment="Center" HorizontalAlignment="Center">
        <iconPacks:MaterialDesignIconExt Kind="ChartBox" Width="18" Height="18" Foreground="White" Margin="0,0,8,0"/>
        <TextBlock Text="View Full Statistics" Foreground="White" VerticalAlignment="Center"/>
    </StackPanel>
</Button>
```

Repeat for other buttons:
- `📋 Browse Data` → `FormatListBulleted`
- `🔄 Auto-Fetch All Data` → `Refresh`
- `⚙️ Settings` → `Cog`

---

## Checkbox with Icon (Favorites Only)

### BEFORE
```xaml
<CheckBox x:Name="FavoritesCheckBox" Content="⭐ Favorites Only" VerticalAlignment="Center" Margin="16,0,0,0" Checked="OnFavoritesFilterChanged" Unchecked="OnFavoritesFilterChanged" Foreground="{StaticResource TextBrush}"/>
```

### AFTER
```xaml
<CheckBox x:Name="FavoritesCheckBox" VerticalAlignment="Center" Margin="16,0,0,0" Checked="OnFavoritesFilterChanged" Unchecked="OnFavoritesFilterChanged" Foreground="{StaticResource TextBrush}">
    <StackPanel Orientation="Horizontal" VerticalAlignment="Center">
        <iconPacks:MaterialDesignIconExt Kind="Star" Width="14" Height="14" Foreground="{StaticResource PrimaryBrush}" Margin="0,0,4,0"/>
        <TextBlock Text="Favorites Only" VerticalAlignment="Center"/>
    </StackPanel>
</CheckBox>
```

---

## Filter Section Header

### BEFORE
```xaml
<TextBlock Text="🔍 Filters" Style="{StaticResource SubHeadingStyle}" Margin="5,5,0,12" VerticalAlignment="Bottom"/>
```

### AFTER
```xaml
<StackPanel Orientation="Horizontal" Margin="5,5,0,12" VerticalAlignment="Bottom">
    <iconPacks:MaterialDesignIconExt Kind="Magnify" Width="20" Height="20" Margin="0,0,4,0" VerticalAlignment="Center"/>
    <TextBlock Text="Filters" Style="{StaticResource SubHeadingStyle}" VerticalAlignment="Center"/>
</StackPanel>
```

---

## Video Buttons (Add, Edit, Delete)

### BEFORE
```xaml
<Button Content="➕ Add Video" Style="{StaticResource PrimaryButtonStyle}" Padding="12,8" Margin="0,0,0,12" Command="{Binding AddVideoCommand}"/>
<Button Content="✏️ Edit Video" Style="{StaticResource SecondaryButtonStyle}" Padding="12,8" Margin="0,0,0,12" Command="{Binding EditVideoCommand}"/>
<Button Content="🗑️ Delete Video" Style="{StaticResource DangerButtonStyle}" Padding="12,8" Command="{Binding DeleteVideoCommand}"/>
```

### AFTER
```xaml
<Button Style="{StaticResource PrimaryButtonStyle}" Padding="12,8" Margin="0,0,0,12" Command="{Binding AddVideoCommand}">
    <StackPanel Orientation="Horizontal" VerticalAlignment="Center" HorizontalAlignment="Center">
        <iconPacks:MaterialDesignIconExt Kind="Plus" Width="16" Height="16" Foreground="White" Margin="0,0,4,0"/>
        <TextBlock Text="Add Video" Foreground="White" VerticalAlignment="Center"/>
    </StackPanel>
</Button>
<Button Style="{StaticResource SecondaryButtonStyle}" Padding="12,8" Margin="0,0,0,12" Command="{Binding EditVideoCommand}">
    <StackPanel Orientation="Horizontal" VerticalAlignment="Center" HorizontalAlignment="Center">
        <iconPacks:MaterialDesignIconExt Kind="Pencil" Width="16" Height="16" Margin="0,0,4,0"/>
        <TextBlock Text="Edit Video" VerticalAlignment="Center"/>
    </StackPanel>
</Button>
<Button Style="{StaticResource DangerButtonStyle}" Padding="12,8" Command="{Binding DeleteVideoCommand}">
    <StackPanel Orientation="Horizontal" VerticalAlignment="Center" HorizontalAlignment="Center">
        <iconPacks:MaterialDesignIconExt Kind="TrashCan" Width="16" Height="16" Foreground="White" Margin="0,0,4,0"/>
        <TextBlock Text="Delete Video" Foreground="White" VerticalAlignment="Center"/>
    </StackPanel>
</Button>
```

---

## Data Tab Refresh Button

### BEFORE
```xaml
<Button Content="🔄 Refresh" Style="{StaticResource SecondaryButtonStyle}" Click="OnRefreshClicked" Margin="12,0,0,0" Padding="12,8"/>
```

### AFTER
```xaml
<Button Style="{StaticResource SecondaryButtonStyle}" Click="OnRefreshClicked" Margin="12,0,0,0" Padding="12,8">
    <StackPanel Orientation="Horizontal" VerticalAlignment="Center" HorizontalAlignment="Center">
        <iconPacks:MaterialDesignIconExt Kind="Refresh" Width="16" Height="16" Margin="0,0,4,0"/>
        <TextBlock Text="Refresh" VerticalAlignment="Center"/>
    </StackPanel>
</Button>
```

---

## Settings Buttons

### BEFORE
```xaml
<Button Content="💾 Save API Key" Style="{StaticResource PrimaryButtonStyle}" Command="{Binding SaveApiKeyCommand}" Padding="16,10" Margin="0,0,8,0"/>
<Button Content="🔑 Clear API Key" Style="{StaticResource DangerButtonStyle}" Command="{Binding ClearApiKeyCommand}" Padding="16,10" Margin="0,0,8,0"/>
<Button Content="🧪 Test API Key" Style="{StaticResource SecondaryButtonStyle}" Command="{Binding TestApiKeyCommand}" Padding="16,10"/>
```

### AFTER
```xaml
<Button Style="{StaticResource PrimaryButtonStyle}" Command="{Binding SaveApiKeyCommand}" Padding="16,10" Margin="0,0,8,0">
    <StackPanel Orientation="Horizontal" VerticalAlignment="Center" HorizontalAlignment="Center">
        <iconPacks:MaterialDesignIconExt Kind="ContentSave" Width="16" Height="16" Foreground="White" Margin="0,0,4,0"/>
        <TextBlock Text="Save API Key" Foreground="White" VerticalAlignment="Center"/>
    </StackPanel>
</Button>
<Button Style="{StaticResource DangerButtonStyle}" Command="{Binding ClearApiKeyCommand}" Padding="16,10" Margin="0,0,8,0">
    <StackPanel Orientation="Horizontal" VerticalAlignment="Center" HorizontalAlignment="Center">
        <iconPacks:MaterialDesignIconExt Kind="Key" Width="16" Height="16" Foreground="White" Margin="0,0,4,0"/>
        <TextBlock Text="Clear API Key" Foreground="White" VerticalAlignment="Center"/>
    </StackPanel>
</Button>
<Button Style="{StaticResource SecondaryButtonStyle}" Command="{Binding TestApiKeyCommand}" Padding="16,10">
    <StackPanel Orientation="Horizontal" VerticalAlignment="Center" HorizontalAlignment="Center">
        <iconPacks:MaterialDesignIconExt Kind="Beaker" Width="16" Height="16" Margin="0,0,4,0"/>
        <TextBlock Text="Test API Key" VerticalAlignment="Center"/>
    </StackPanel>
</Button>
```

Repeat for database buttons:
- `🔄 Auto-Fetch All Data` → `Refresh`
- `📂 Show Database Path` → `FolderOpen`
- `🗑️ Clear All Data` → `TrashCan`

---

## Info Box Header

### BEFORE
```xaml
<TextBlock Text="💡 Dashboard Updates" FontWeight="Bold" Margin="0,0,0,8"/>
```

### AFTER
```xaml
<StackPanel Orientation="Horizontal" Margin="0,0,0,8">
    <iconPacks:MaterialDesignIconExt Kind="LightbulbOn" Width="16" Height="16" Margin="0,0,4,0" VerticalAlignment="Center"/>
    <TextBlock Text="Dashboard Updates" FontWeight="Bold" VerticalAlignment="Center"/>
</StackPanel>
```

---

## DataGrid Column Headers (Watch Tab)

### BEFORE
```xaml
<DataGridCheckBoxColumn Header="👁️" Binding="{Binding IsWatched}" IsReadOnly="False" Width="40"/>
<DataGridCheckBoxColumn Header="⭐" Binding="{Binding IsFavorite}" IsReadOnly="False" Width="40"/>
```

### AFTER
```xaml
<DataGridTemplateColumn Header="👁️" Width="40">
    <DataGridTemplateColumn.CellTemplate>
        <DataTemplate>
            <CheckBox IsChecked="{Binding IsWatched}" VerticalAlignment="Center" HorizontalAlignment="Center"/>
        </DataTemplate>
    </DataGridTemplateColumn.CellTemplate>
    <DataGridTemplateColumn.CellEditingTemplate>
        <DataTemplate>
            <CheckBox IsChecked="{Binding IsWatched}" VerticalAlignment="Center" HorizontalAlignment="Center"/>
        </DataTemplate>
    </DataGridTemplateColumn.CellEditingTemplate>
</DataGridTemplateColumn>
<DataGridTemplateColumn Header="⭐" Width="40">
    <DataGridTemplateColumn.CellTemplate>
        <DataTemplate>
            <CheckBox IsChecked="{Binding IsFavorite}" VerticalAlignment="Center" HorizontalAlignment="Center"/>
        </DataTemplate>
    </DataGridTemplateColumn.CellTemplate>
    <DataGridTemplateColumn.CellEditingTemplate>
        <DataTemplate>
            <CheckBox IsChecked="{Binding IsFavorite}" VerticalAlignment="Center" HorizontalAlignment="Center"/>
        </DataTemplate>
    </DataGridTemplateColumn.CellEditingTemplate>
</DataGridTemplateColumn>
```

---

## Pagination Buttons

### BEFORE
```xaml
<Button Content="⏮ First" Command="{Binding FirstPageCommand}" Style="{StaticResource SecondaryButtonStyle}" Padding="10,6" FontSize="11" Margin="0,0,4,0"/>
<Button Content="◀ Previous" Command="{Binding PreviousPageCommand}" Style="{StaticResource SecondaryButtonStyle}" Padding="10,6" FontSize="11" Margin="0,0,4,0"/>
<Button Content="Next ▶" Command="{Binding NextPageCommand}" Style="{StaticResource SecondaryButtonStyle}" Padding="10,6" FontSize="11" Margin="8,0,4,0"/>
<Button Content="Last ⏭" Command="{Binding LastPageCommand}" Style="{StaticResource SecondaryButtonStyle}" Padding="10,6" FontSize="11"/>
```

### AFTER
```xaml
<Button Command="{Binding FirstPageCommand}" Style="{StaticResource SecondaryButtonStyle}" Padding="10,6" FontSize="11" Margin="0,0,4,0">
    <iconPacks:MaterialDesignIconExt Kind="SkipPrevious" Width="14" Height="14"/>
</Button>
<Button Command="{Binding PreviousPageCommand}" Style="{StaticResource SecondaryButtonStyle}" Padding="10,6" FontSize="11" Margin="0,0,4,0">
    <iconPacks:MaterialDesignIconExt Kind="ChevronLeft" Width="14" Height="14"/>
</Button>
<Button Command="{Binding NextPageCommand}" Style="{StaticResource SecondaryButtonStyle}" Padding="10,6" FontSize="11" Margin="8,0,4,0">
    <iconPacks:MaterialDesignIconExt Kind="ChevronRight" Width="14" Height="14"/>
</Button>
<Button Command="{Binding LastPageCommand}" Style="{StaticResource SecondaryButtonStyle}" Padding="10,6" FontSize="11">
    <iconPacks:MaterialDesignIconExt Kind="SkipNext" Width="14" Height="14"/>
</Button>
```

---

## Implementation Tips

1. **Copy the namespace first** at the top of MainWindow.xaml
2. **Start with high-visibility items** (header, tab headers, main buttons)
3. **Test each section** after replacement
4. **Use consistent sizing** based on context (see sizing guide in main guide)
5. **Maintain alignment** with text using `VerticalAlignment="Center"`
6. **Build after changes** to verify icons render correctly

## Status

✅ All replacements use Material Design icons  
✅ Colors coordinated with existing design  
✅ Sizing optimized for each context  
✅ Ready to implement incrementally
