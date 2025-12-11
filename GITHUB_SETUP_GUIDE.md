# How to Push Rugby API App to GitHub

This guide walks you through uploading your Rugby API Console Application to GitHub.

## Step 1: Create a GitHub Repository

### Option A: Using GitHub Web Interface

1. Go to [github.com](https://github.com)
2. Click the **+** icon in the top right → **New repository**
3. Fill in the details:
   - **Repository name**: `RugbyApiApp`
   - **Description**: "A .NET 10 console application for managing rugby data"
   - **Public** or **Private** (your choice)
   - **Do NOT** initialize with README, .gitignore, or license (we have these)
4. Click **Create repository**
5. Copy the HTTPS URL shown (looks like: `https://github.com/yourusername/RugbyApiApp.git`)

## Step 2: Initialize Git Locally

### Option A: Fresh Start (Recommended)

Open PowerShell in your project root (`C:\Users\Brand\source\repos\RugbyApiApp`) and run:

```powershell
# Navigate to the root directory
cd "C:\Users\Brand\source\repos\RugbyApiApp"

# Initialize git
git init

# Add all files
git add .

# Initial commit
git commit -m "Initial commit: Rugby API console application"

# Add remote repository (replace with your GitHub URL)
git remote add origin https://github.com/yourusername/RugbyApiApp.git

# Push to GitHub
git branch -M main
git push -u origin main
```

### Option B: If Git Already Initialized

```powershell
# Check current remote
git remote -v

# If no remote exists, add it
git remote add origin https://github.com/yourusername/RugbyApiApp.git

# If origin already exists but points to wrong URL
git remote set-url origin https://github.com/yourusername/RugbyApiApp.git

# Push to GitHub
git push -u origin main
```

## Step 3: Verify on GitHub

1. Go to your GitHub repository URL
2. You should see all your files listed
3. Your README.md should display automatically

## Step 4: Configure Git (if needed)

If it's your first time using Git, configure your user:

```powershell
git config --global user.name "Your Name"
git config --global user.email "your.email@example.com"
```

## Step 5: Update Your README Files

You now have two README files. You can:

**Option A: Keep README_GITHUB.md and rename it**
```powershell
# Remove old README.md if exists
rm README.md

# Rename README_GITHUB.md to README.md
mv README_GITHUB.md README.md

# Commit the change
git add README.md
git commit -m "Update README for GitHub"
git push
```

**Option B: Replace content**
```powershell
# Copy content from README_GITHUB.md to README.md
# Then commit
git add README.md
git commit -m "Update README for GitHub"
git push
```

## Common Commands for Future Updates

```powershell
# Check status
git status

# Stage changes
git add .

# Commit changes
git commit -m "Your commit message"

# Push to GitHub
git push origin main

# Pull latest changes
git pull origin main

# Create a new branch
git checkout -b feature/your-feature-name

# Switch branches
git checkout main

# Merge a branch
git merge feature/your-feature-name
```

## Troubleshooting

### "fatal: not a git repository"
You're not in the right directory. Navigate to your project root:
```powershell
cd "C:\Users\Brand\source\repos\RugbyApiApp"
```

### "Permission denied (publickey)"
You need to set up SSH keys or use HTTPS with a personal access token:
```powershell
# Use HTTPS instead
git remote set-url origin https://github.com/yourusername/RugbyApiApp.git
```

### "Please tell me who you are"
Configure your Git user:
```powershell
git config --global user.name "Your Name"
git config --global user.email "your.email@example.com"
```

### "fatal: remote origin already exists"
Your remote is already configured. Check it:
```powershell
git remote -v

# Or update it if wrong
git remote set-url origin https://github.com/yourusername/RugbyApiApp.git
```

## What Gets Uploaded

✅ **Included** (in .gitignore):
- All source code (`.cs` files)
- Project configuration (`.csproj`, `.sln`)
- Documentation (README.md, LICENSE, CONTRIBUTING.md)
- Configuration files (`.editorconfig`)

❌ **Excluded** (in .gitignore):
- Build artifacts (`bin/`, `obj/`)
- IDE files (`.vs/`, `.idea/`, `*.user`)
- Database file (`rugby.db`)
- Environment variables (API keys)
- NuGet packages (restored automatically)

## Next Steps

1. ✅ Repository created and uploaded
2. Add topics to your repository:
   - Go to repository settings
   - Add topics like: `csharp`, `dotnet`, `rugby`, `api`, `console-app`
3. Consider adding:
   - GitHub Actions for CI/CD
   - Issues and pull request templates
   - Release notes/CHANGELOG

## Example Repository Structure on GitHub

```
RugbyApiApp/
├── .gitignore
├── LICENSE
├── CONTRIBUTING.md
├── README.md
├── RugbyApiApp.slnx
├── RugbyApiApp/
│   ├── Program.cs
│   ├── RugbyApiApp.csproj
│   ├── Models/
│   ├── DTOs/
│   ├── Services/
│   ├── Data/
│   ├── Examples/
│   └── Documentation/ (optional - for markdown docs)
└── .github/ (optional - for GitHub Actions)
    └── workflows/
```

---

Your Rugby API Console Application is now ready to share with the world! 🚀
