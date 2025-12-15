# Reusable Icon Styles and Templates

This file contains reusable XAML styles and templates to make icon replacement implementation faster and more consistent.

## Add These to Window.Resources

### Icon Color Definitions

Add these to your color definitions section:

```xaml
<!-- Icon Colors -->
<Color x:Key="IconWhiteColor">#FFFFFF</Color>
<Color x:Key="IconPrimaryColor">#512BD4</Color>
<Color x:Key="IconSecondaryColor">#757575</Color>
<Color x:Key="IconAccentColor">#00B4D8</Color>
<Color x:Key="IconSuccessColor">#22C55E</Color>
<Color x:Key="IconDangerColor">#EF4444</Color>
```

### Icon Brush Definitions

Add these to your brush definitions section:

```xaml
<!-- Icon Brushes -->
<SolidColorBrush x:Key="IconWhiteBrush" Color="{StaticResource IconWhiteColor}"/>
<SolidColorBrush x:Key="IconPrimaryBrush" Color="{StaticResource IconPrimaryColor}"/>
<SolidColorBrush x:Key="IconSecondaryBrush" Color="{StaticResource IconSecondaryColor}"/>
<SolidColorBrush x:Key="IconAccentBrush" Color="{StaticResource IconAccentColor}"/>
<SolidColorBrush x:Key="IconSuccessBrush" Color="{StaticResource IconSuccessColor}"/>
<SolidColorBrush x:Key="IconDangerBrush" Color="{StaticResource IconDangerColor}"/>
```

### Reusable Stacks (Button Content Containers)

```xaml
<!-- Icon + Text Button Content Stack -->
<Style x:Key="IconButtonStackStyle" TargetType="StackPanel">
    <Setter Property="Orientation" Value="Horizontal"/>
    <Setter Property="VerticalAlignment" Value="Center"/>
    <Setter Property="HorizontalAlignment" Value="Center"/>
</Style>

<!-- Icon-only Stack (for small icon buttons) -->
<Style x:Key="IconOnlyStackStyle" TargetType="StackPanel">
    <Setter Property="Orientation" Value="Horizontal"/>
    <Setter Property="VerticalAlignment" Value="Center"/>
    <Setter Property="HorizontalAlignment" Value="Center"/>
</Style>

<!-- Icon + Text Horizontal Stack (for toolbar buttons) -->
<Style x:Key="IconTextStackStyle" TargetType="StackPanel">
    <Setter Property="Orientation" Value="Horizontal"/>
    <Setter Property="VerticalAlignment" Value="Center"/>
    <Setter Property="HorizontalAlignment" Value="Left"/>
</Style>
```

## Template Examples for Different Button Styles

### Primary Button with Icon + Text

**XAML Template:**
```xaml
<!-- In Window.Resources -->
<DataTemplate x:Key="PrimaryIconButtonTemplate" DataType="System:String">
    <StackPanel Style="{StaticResource IconButtonStackStyle}">
        <iconPacks:MaterialDesignIconExt 
            Kind="{Binding Kind}" 
            Width="18" Height="18" 
            Foreground="{StaticResource IconWhiteBrush}" 
            Margin="0,0,8,0"/>
        <TextBlock Text="{Binding Text}" Foreground="{StaticResource IconWhiteBrush}" VerticalAlignment="Center"/>
    </StackPanel>
</DataTemplate>
```

**Usage:**
```xaml
<Button Style="{StaticResource PrimaryButtonStyle}" Padding="12,8">
    <StackPanel Style="{StaticResource IconButtonStackStyle}">
        <iconPacks:MaterialDesignIconExt Kind="ChartBox" Width="18" Height="18" Foreground="{StaticResource IconWhiteBrush}" Margin="0,0,8,0"/>
        <TextBlock Text="View Statistics" Foreground="{StaticResource IconWhiteBrush}" VerticalAlignment="Center"/>
    </StackPanel>
</Button>
```

### Secondary Button with Icon + Text

```xaml
<Button Style="{StaticResource SecondaryButtonStyle}" Padding="12,8">
    <StackPanel Style="{StaticResource IconButtonStackStyle}">
        <iconPacks:MaterialDesignIconExt Kind="Refresh" Width="18" Height="18" Foreground="{StaticResource IconSecondaryBrush}" Margin="0,0,8,0"/>
        <TextBlock Text="Refresh" Foreground="{StaticResource TextBrush}" VerticalAlignment="Center"/>
    </StackPanel>
</Button>
```

### Danger Button with Icon + Text

```xaml
<Button Style="{StaticResource DangerButtonStyle}" Padding="12,8">
    <StackPanel Style="{StaticResource IconButtonStackStyle}">
        <iconPacks:MaterialDesignIconExt Kind="TrashCan" Width="18" Height="18" Foreground="{StaticResource IconWhiteBrush}" Margin="0,0,8,0"/>
        <TextBlock Text="Delete" Foreground="{StaticResource IconWhiteBrush}" VerticalAlignment="Center"/>
    </StackPanel>
</Button>
```

## Predefined Icon Sizes

Add these numeric resources for consistent sizing:

```xaml
<!-- Icon Sizes -->
<System:Int32 x:Key="IconSizeHeader">32</System:Int32>
<System:Int32 x:Key="IconSizeTabHeader">16</System:Int32>
<System:Int32 x:Key="IconSizeCardHeader">24</System:Int32>
<System:Int32 x:Key="IconSizeButton">18</System:Int32>
<System:Int32 x:Key="IconSizeInline">14</System:Int32>
<System:Int32 x:Key="IconSizeSmall">12</System:Int32>
```

**Usage in XAML:**
```xaml
<iconPacks:MaterialDesignIconExt Kind="ChartBox" 
    Width="{StaticResource IconSizeButton}" 
    Height="{StaticResource IconSizeButton}"/>
```

## Tab Header Template Helper

Create a reusable tab header with icon:

```xaml
<Style x:Key="IconTabItemStyle" TargetType="TabItem" BasedOn="{StaticResource ModernTabItem}">
    <Setter Property="Template">
        <Setter.Value>
            <ControlTemplate TargetType="TabItem">
                <Grid>
                    <Border Name="Border" Background="{TemplateBinding Background}" 
                            BorderBrush="{TemplateBinding BorderBrush}" 
                            BorderThickness="{TemplateBinding BorderThickness}">
                        <StackPanel Orientation="Horizontal" Margin="{TemplateBinding Padding}" 
                                   HorizontalAlignment="Center" VerticalAlignment="Center">
                            <!-- Icon will be passed as Content -->
                            <ContentPresenter ContentSource="Header"/>
                        </StackPanel>
                    </Border>
                </Grid>
                <ControlTemplate.Triggers>
                    <Trigger Property="IsSelected" Value="True">
                        <Setter TargetName="Border" Property="BorderBrush" Value="{StaticResource PrimaryBrush}"/>
                        <Setter Property="Foreground" Value="{StaticResource PrimaryBrush}"/>
                        <Setter TargetName="Border" Property="Background" Value="{StaticResource SecondaryBrush}"/>
                    </Trigger>
                    <Trigger Property="IsMouseOver" Value="True">
                        <Setter Property="Foreground" Value="{StaticResource TextBrush}"/>
                    </Trigger>
                </ControlTemplate.Triggers>
            </ControlTemplate>
        </Setter.Value>
    </Setter>
</Style>
```

## TextBlock with Prefix Icon Helper

```xaml
<Style x:Key="IconTextBlockStyle" TargetType="StackPanel">
    <Setter Property="Orientation" Value="Horizontal"/>
    <Setter Property="Margin" Value="0,0,0,12"/>
</Style>

<!-- Usage -->
<StackPanel Style="{StaticResource IconTextBlockStyle}">
    <iconPacks:MaterialDesignIconExt Kind="Globe" 
        Width="{StaticResource IconSizeCardHeader}" 
        Height="{StaticResource IconSizeCardHeader}" 
        Margin="0,0,8,0" VerticalAlignment="Center"/>
    <TextBlock Text="Countries" Style="{StaticResource SubHeadingStyle}" VerticalAlignment="Center"/>
</StackPanel>
```

## CheckBox with Icon

```xaml
<Style x:Key="IconCheckBoxStyle" TargetType="CheckBox">
    <Setter Property="Foreground" Value="{StaticResource TextBrush}"/>
    <Setter Property="VerticalAlignment" Value="Center"/>
</Style>

<!-- Usage -->
<CheckBox Style="{StaticResource IconCheckBoxStyle}">
    <StackPanel Orientation="Horizontal" VerticalAlignment="Center">
        <iconPacks:MaterialDesignIconExt Kind="Star" 
            Width="{StaticResource IconSizeInline}" 
            Height="{StaticResource IconSizeInline}" 
            Foreground="{StaticResource IconPrimaryBrush}" 
            Margin="0,0,4,0"/>
        <TextBlock Text="Favorites Only" VerticalAlignment="Center"/>
    </StackPanel>
</CheckBox>
```

## Complete Example: All Button Variants

```xaml
<!-- Primary Action Button -->
<Button Style="{StaticResource PrimaryButtonStyle}" Padding="16,10" Height="40">
    <StackPanel Style="{StaticResource IconButtonStackStyle}">
        <iconPacks:MaterialDesignIconExt Kind="ChartBox" 
            Width="{StaticResource IconSizeButton}" 
            Height="{StaticResource IconSizeButton}" 
            Foreground="{StaticResource IconWhiteBrush}" 
            Margin="0,0,8,0"/>
        <TextBlock Text="View Statistics" 
            Foreground="{StaticResource IconWhiteBrush}" 
            VerticalAlignment="Center"/>
    </StackPanel>
</Button>

<!-- Secondary Action Button -->
<Button Style="{StaticResource SecondaryButtonStyle}" Padding="16,10" Height="40">
    <StackPanel Style="{StaticResource IconButtonStackStyle}">
        <iconPacks:MaterialDesignIconExt Kind="Refresh" 
            Width="{StaticResource IconSizeButton}" 
            Height="{StaticResource IconSizeButton}" 
            Margin="0,0,8,0"/>
        <TextBlock Text="Refresh Data" VerticalAlignment="Center"/>
    </StackPanel>
</Button>

<!-- Danger Action Button -->
<Button Style="{StaticResource DangerButtonStyle}" Padding="16,10" Height="40">
    <StackPanel Style="{StaticResource IconButtonStackStyle}">
        <iconPacks:MaterialDesignIconExt Kind="TrashCan" 
            Width="{StaticResource IconSizeButton}" 
            Height="{StaticResource IconSizeButton}" 
            Foreground="{StaticResource IconWhiteBrush}" 
            Margin="0,0,8,0"/>
        <TextBlock Text="Delete All" 
            Foreground="{StaticResource IconWhiteBrush}" 
            VerticalAlignment="Center"/>
    </StackPanel>
</Button>
```

## Implementation Workflow

1. **Add all styles to Window.Resources** (copy sections above)
2. **Add namespace** at top of MainWindow.xaml:
   ```xaml
   xmlns:iconPacks="http://metro.mahapps.com/winfx/xaml/iconpacks"
   ```
3. **Replace icons one section at a time** using the patterns above
4. **Test after each section** to ensure icons render correctly
5. **Iterate** through Header → Tabs → Cards → Buttons → Checkboxes

## Benefits of Reusable Styles

✅ Consistency across entire application  
✅ Easier maintenance (change size in one place)  
✅ Reduced XAML duplication  
✅ Faster implementation  
✅ Better color coordination  
✅ Professional appearance  

## Quick Reference Sizes

```
Header: 32px
Card Headers: 24px  
Tab Headers: 16px
Buttons: 18px
Inline/CheckBox: 14px
Small: 12px
```

This approach makes it much easier to implement all icon replacements consistently across your entire application!
