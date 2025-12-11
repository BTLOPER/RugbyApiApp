# Contributing to Rugby API Console Application

Thanks for taking the time to contribute!

## Code of Conduct

This project and everyone participating in it is governed by our Code of Conduct. By participating, you are expected to uphold this code.

## How Can I Contribute?

### Reporting Bugs

Before creating bug reports, check the issue list first. When creating a bug report, please include:

- Use a clear and descriptive title
- Describe the exact steps to reproduce the problem
- Provide specific examples to demonstrate the steps
- Describe the behavior you observed
- Explain what behavior you expected to see and why
- Include your environment:
  - .NET version (dotnet --version)
  - Visual Studio version
  - Operating System and version

### Suggesting Enhancements

Enhancement suggestions are tracked as GitHub issues. Please include:

- A clear and descriptive title
- Step-by-step description of the suggested enhancement
- Specific examples to demonstrate the steps
- Current vs. expected behavior
- Explanation of why this enhancement would be useful

### Pull Requests

- Follow the C# style guide
- Include appropriate comments
- Document new code with XML comments
- Update documentation as needed
- Test your changes before submitting

## Development Setup

1. Fork the repository:
```bash
git clone https://github.com/yourusername/RugbyApiApp.git
cd RugbyApiApp
```

2. Create a feature branch:
```bash
git checkout -b feature/your-feature-name
```

3. Install dependencies:
```bash
dotnet restore
```

4. Build the project:
```bash
dotnet build
```

5. Set up your API key:
```powershell
[Environment]::SetEnvironmentVariable("RUGBY_API_KEY", "your-key-here", "User")
```

6. Test your changes:
```bash
dotnet run
```

## Style Guide

### C# Code Style

- Use 4 spaces for indentation
- Use var for obvious types, explicit types for complex types
- Use meaningful variable and method names
- Add XML comments for public methods:

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

- Use present tense ("Add feature" not "Added feature")
- Use imperative mood ("Move cursor to..." not "Moves cursor to...")
- Limit first line to 72 characters or less
- Reference issues and pull requests after the first line

Example:
```
Add support for fetching teams by league

- Implement GetTeamsByLeagueAsync method
- Add corresponding DTO mapping
- Update UI to display teams by league

Fixes #123
```

## Project Structure

When adding new features:

1. Models - Add new Entity Framework models here
2. DTOs - Add API response DTOs here
3. Services - Add new service methods here
4. Examples - Add usage examples here
5. Program.cs - Update main menu and handler methods

## Testing

Before submitting your PR:

1. Build the project: dotnet build
2. Test your changes manually
3. Check for any compilation warnings
4. Verify the application still runs correctly

## Documentation

- Update README.md if adding new features
- Add comments for complex logic
- Update CONTRIBUTING.md if you change the development process

## Questions?

Feel free to open an issue with the 'question' label or contact the maintainers.

## Recognition

Contributors will be recognized in:
- The README.md file
- Release notes for new versions

Thank you for contributing!
