# User Secrets Implementation - Secure API Key Storage ✅

The application now uses **User Secrets** for secure storage of the API key, replacing direct environment variables. This is the recommended approach for development environments.

## What Changed

### 1. Project Files Updated
- **RugbyApiApp.MAUI.csproj**: Added `UserSecretsId: RugbyApiApp-WPF-2024`
- **RugbyApiApp.Console.csproj**: Added `UserSecretsId: RugbyApiApp-Console-2024`
- Both now include `Microsoft.Extensions.Configuration.UserSecrets` NuGet package

### 2. New SecretsService Created
- **File**: `RugbyApiApp/Services/SecretsService.cs`
- Centralized API key management
- Handles both User Secrets and environment variables
- Provides storage information and status

### 3. Code Updates
- **MainWindow.xaml.cs**: Uses SecretsService with User Secrets configuration
- **Console Program.cs**: Uses SecretsService with User Secrets configuration

## How User Secrets Work

### What Are User Secrets?

User Secrets is a secure way to store sensitive data during development:

✅ **NOT checked into version control** (.gitignore automatically added)
✅ **Stored locally** on your machine in a secure location
✅ **Per-user, per-machine** storage
✅ **Development only** - for production use environment variables

### Storage Locations

**Windows**:
```
%APPDATA%\Microsoft\UserSecrets\<UserSecretsId>\secrets.json
```

**Linux/macOS**:
```
~/.microsoft/usersecrets/<UserSecretsId>/secrets.json
```

## Setting Up User Secrets

### For MAUI (WPF) Application

**Option 1: Command Line**

Open PowerShell/Terminal in the `RugbyApiApp.MAUI` directory:

```powershell
# Set the API key
dotnet user-secrets set RugbyApiKey "your-actual-api-key-here"

# Verify it was set
dotnet user-secrets list
```

**Option 2: Visual Studio**

1. Right-click project `RugbyApiApp.MAUI`
2. Select "Manage User Secrets"
3. Edit the `secrets.json` file that opens:
```json
{
  "RugbyApiKey": "your-actual-api-key-here"
}
```

### For Console Application

**Option 1: Command Line**

Open PowerShell/Terminal in the `RugbyApiApp.Console` directory:

```powershell
# Set the API key
dotnet user-secrets set RugbyApiKey "your-actual-api-key-here"

# Verify it was set
dotnet user-secrets list
```

**Option 2: Visual Studio**

1. Right-click project `RugbyApiApp.Console`
2. Select "Manage User Secrets"
3. Edit the `secrets.json` file:
```json
{
  "RugbyApiKey": "your-actual-api-key-here"
}
```

## API Key Priority Order

The application loads the API key in this order (first match wins):

1. **User Secrets** (development - recommended)
2. **Environment Variable** (production/fallback)

```csharp
string? apiKey = _secretsService.GetApiKey();
// Checks User Secrets first, then environment variable
```

## Using the Application

### WPF Application

**With User Secrets Configured**:
- API key is automatically loaded on startup
- No need to manually enter it unless you want to update it
- Settings screen shows where key is stored

**To Save a New Key**:
1. Go to Settings tab
2. Enter API key in text field
3. Click "💾 Save API Key"
- Saves to environment variable
- Shows confirmation with storage location

**To Clear Key**:
1. Go to Settings tab
2. Click "🔑 Clear API Key"
- Dialog shows command to remove from User Secrets
- Clears environment variable

### Console Application

**Automatic Startup**:
- Loads API key from User Secrets or environment variable
- Shows key source on startup
- If not found, displays setup instructions

**Helpful Error Message**:
```
ERROR: No API key found.
Please set your api-sports.io API key using User Secrets:
  dotnet user-secrets set RugbyApiKey "your-api-key"

Or set the RUGBY_API_KEY environment variable.
```

## Security Benefits

✅ **Safer Development**
- No API keys in source code
- No API keys in environment variables (public)
- Private storage on your machine

✅ **Team Security**
- Each developer has their own secrets
- Doesn't interfere with colleagues
- Easy to revoke access

✅ **Production Safe**
- Fallback to environment variables for CI/CD
- Can use different secrets for different environments
- Follows security best practices

## Managing User Secrets

### View All Secrets

```powershell
# In project directory
dotnet user-secrets list
```

### Remove a Secret

```powershell
# Remove specific secret
dotnet user-secrets remove RugbyApiKey

# Or clear all secrets for this project
dotnet user-secrets clear
```

### List Secrets from Visual Studio

1. Right-click project
2. Select "Manage User Secrets"
3. View/edit in the `secrets.json` file that opens

## SecretsService API

The `SecretsService` class provides these methods:

```csharp
// Get API key (User Secrets first, then env var)
string? apiKey = _secretsService.GetApiKey();

// Set API key (to environment variable)
_secretsService.SetApiKey("api-key-value");

// Clear API key (from environment variable)
_secretsService.ClearApiKey();

// Get information about where key is stored
string info = _secretsService.GetStorageInfo();
// Returns: "API key found in: User Secrets, Environment Variable"
```

## Troubleshooting

### "API key not found" Error

**Solution 1**: Set User Secret
```powershell
cd RugbyApiApp.MAUI
dotnet user-secrets set RugbyApiKey "your-api-key"
```

**Solution 2**: Set Environment Variable
```powershell
# Windows PowerShell
[Environment]::SetEnvironmentVariable("RUGBY_API_KEY", "your-api-key", "User")

# Or use Console/Terminal
set RUGBY_API_KEY=your-api-key
```

### "Secrets ID not found" Error

This means the project's `UserSecretsId` wasn't set properly:

1. Verify the `.csproj` file contains:
```xml
<UserSecretsId>RugbyApiApp-WPF-2024</UserSecretsId>
```

2. Clean and rebuild:
```powershell
dotnet clean
dotnet build
```

### Secrets Not Being Read

1. Ensure secrets are set in correct project:
```powershell
cd RugbyApiApp.MAUI
dotnet user-secrets list
```

2. Rebuild the project:
```powershell
dotnet clean
dotnet build
```

3. Restart the application

## Environment-Specific Setup

### Development

Use User Secrets:
```powershell
dotnet user-secrets set RugbyApiKey "dev-api-key"
```

### Production/Docker/CI-CD

Use environment variable:
```bash
export RUGBY_API_KEY="prod-api-key"
# or
set RUGBY_API_KEY=prod-api-key
```

### Testing

Use User Secrets:
```powershell
dotnet user-secrets set RugbyApiKey "test-api-key"
```

## Migration from Old Method

If you were previously using only environment variables:

1. **Set User Secret** (recommended):
```powershell
cd RugbyApiApp.MAUI
dotnet user-secrets set RugbyApiKey "your-key"
```

2. **Optional**: Clear old environment variable
```powershell
# Windows PowerShell
[Environment]::SetEnvironmentVariable("RUGBY_API_KEY", $null, "User")
```

The application will now prefer User Secrets but still falls back to the environment variable if needed.

## Best Practices

✅ **DO**:
- Use User Secrets for development
- Use environment variables for production
- Store API keys in secure secrets manager (Azure Key Vault, AWS Secrets Manager, etc.)
- Rotate API keys regularly
- Use different keys for different environments

❌ **DON'T**:
- Commit `secrets.json` to version control
- Share API keys in chat/email
- Use same key for dev/prod/test
- Store secrets as plain text in code
- Leave secrets in environment variables permanently

## Additional Resources

- [Microsoft Docs - User Secrets](https://learn.microsoft.com/en-us/aspnet/core/security/app-secrets)
- [Safe Storage of App Secrets in Development](https://docs.microsoft.com/en-us/aspnet/core/security/app-secrets)
- [Azure Key Vault for Production Secrets](https://azure.microsoft.com/en-us/services/key-vault/)

---

API key storage is now secure and follows development best practices! 🔐
