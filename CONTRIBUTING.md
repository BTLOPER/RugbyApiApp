# Contributing to Rugby API Console Application

First off, thanks for taking the time to contribute! 🎉

The following is a set of guidelines for contributing to this project.

## Code of Conduct

This project and everyone participating in it is governed by our Code of Conduct. By participating, you are expected to uphold this code.

## How Can I Contribute?

### Reporting Bugs

Before creating bug reports, please check the issue list as you might find out that you don't need to create one. When you are creating a bug report, please include as many details as possible:

* **Use a clear and descriptive title**
* **Describe the exact steps which reproduce the problem**
* **Provide specific examples to demonstrate the steps**
* **Describe the behavior you observed after following the steps**
* **Explain which behavior you expected to see instead and why**
* **Include screenshots if possible**
* **Include your environment**:
  - .NET version (`dotnet --version`)
  - Visual Studio version
  - Operating System and version

### Suggesting Enhancements

Enhancement suggestions are tracked as GitHub issues. When creating an enhancement suggestion, please include:

* **Use a clear and descriptive title**
* **Provide a step-by-step description of the suggested enhancement**
* **Provide specific examples to demonstrate the steps**
* **Describe the current behavior and the expected behavior**
* **Explain why this enhancement would be useful**

### Pull Requests

* Fill in the required template
* Follow the C# style guide
* Include appropriate test cases
* Document new code with XML comments
* Update documentation as needed

## Development Setup

1. **Fork the repository**
   ```bash
   git clone https://github.com/yourusername/RugbyApiApp.git
   cd RugbyApiApp
   ```

2. **Create a feature branch**
   ```bash
   git checkout -b feature/your-feature-name
   ```

3. **Install dependencies**
   ```bash
   dotnet restore
   ```

4. **Build the project**
   ```bash
   dotnet build
   ```

5. **Set up your API key**
   ```powershell
   [Environment]::SetEnvironmentVariable("RUGBY_API_KEY", "your-key-here", "User")
   ```

6. **Make your changes and test**
   ```bash
   dotnet run
   ```

## Style Guide

### C# Code Style

* Use 4 spaces for indentation
* Use `var` for obvious types, explicit types for complex types
* Use meaningful variable and method names
* Add XML comments for public methods:
  ```csharp
  /// <summary>
  /// Brief description of what this method does
  /// </summary>
  /// <param name="paramName">Description of parameter</param>
  /// <returns>Description of return value</returns>
  public async Task<string> MyMethodAsync(string paramName)
  {
      // implementation
  }
  ```

### Commit Messages

* Use the present tense ("Add feature" not "Added feature")
* Use the imperative mood ("Move cursor to..." not "Moves cursor to...")
* Limit the first line to 72 characters or less
* Reference issues and pull requests liberally after the first line
* Example:
  ```
  Add support for fetching teams by league
  
  - Implement GetTeamsByLeagueAsync method
  - Add corresponding DTO mapping
  - Update UI to display teams by league
  
  Fixes #123
  ```

## Project Structure Guidelines

When adding new features:

1. **Models** - Add new Entity Framework models here
2. **DTOs** - Add API response DTOs here
3. **Services** - Add new service methods here
4. **Examples** - Add usage examples here
5. **Program.cs** - Update main menu and handler methods here

## Testing

Before submitting your PR:

1. Build the project: `dotnet build`
2. Test your changes manually
3. Check for any compilation warnings
4. Verify the application still runs correctly

## Documentation

* Update README.md if adding new features
* Add comments for complex logic
* Update this CONTRIBUTING.md if you change the development process

## Questions?

Feel free to open an issue with the `question` label or contact the maintainers.

## Recognition

Contributors will be recognized in:
* The README.md file
* Release notes for new versions

Thank you for contributing! 🏉
