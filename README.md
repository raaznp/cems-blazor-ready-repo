# Community Event Management System

A **Blazor Web App** built with **.NET 10 LTS** and **Entity Framework Core** for managing community events, participants, venues, and activities.

**Course:** CET254 - Advanced Programming  
**Institution:** ISMT College (affiliated via Sunderland University, UK)

---

## 🚀 Quick Start

### Prerequisites
- **.NET 10 LTS SDK** → [Download](https://dotnet.microsoft.com/en-us/download/dotnet/10.0)
- **Visual Studio Code** (recommended) or any text editor
- **Git** for cloning the repository

### Clone & Run (3 steps)

```bash
# 1. Clone the repository
git clone https://github.com/raaznp/cems-blazor-ready-repo.git
cd cems-blazor-ready-repo

# 2. Restore dependencies
dotnet restore

# 3. Run with hot reload
dotnet watch --project CommunityEventManagementSystem.App
```

The app will automatically:
- Create the SQLite database on first run
- Apply any pending migrations
- Open in your browser at **https://localhost:5001** (or http://localhost:5000)

---

## 📁 Project Structure

```
CommunityEventManagementSystem.App/
├── Components/              # Blazor components & pages
│   ├── Pages/              # Event, Admin, and Home pages
│   ├── Layout/             # Main layout and navigation
│   └── App.razor           # Root component
├── Models/                 # Data entities (Event, Participant, Venue, Activity)
├── Data/                   # EF Core DbContext
├── Services/               # Business logic (EventService, ParticipantService, etc.)
├── Migrations/             # Database migration files
├── wwwroot/                # Static files (CSS, images, Bootstrap)
├── appsettings.json        # Configuration & connection strings
└── Program.cs              # App startup & dependency injection
```

---

## 🗄️ Database (SQLite)

### Connection String
- **Location:** `CommunityEventManagementSystem.App/appsettings.json`
- **Default:** `Data Source=app.db` (creates SQLite file locally)

### Models
- **Event** — Community events with date, location, description
- **Participant** — Event attendees
- **Venue** — Locations for events
- **Activity** — Event activities/agenda items

### Create & Apply Migrations

```bash
# Create a new migration
dotnet ef migrations add MigrationName --project CommunityEventManagementSystem.App

# Apply migrations to database
dotnet ef database update --project CommunityEventManagementSystem.App

# Remove last migration (if needed)
dotnet ef migrations remove --project CommunityEventManagementSystem.App
```

---

## 🛠️ VS Code Setup

The project includes VS Code configuration:
- **extensions.json** — Recommended extensions (C# Dev Kit, Blazor companion, etc.)
- **tasks.json** — Build & run tasks
- **launch.json** — Debug configurations

**First time in VS Code?** VS Code will suggest installing recommended extensions. Click "Install All" for best experience.

---

## 🎯 Features

- ✅ **Blazor Web App** — Interactive UI with C#
- ✅ **Entity Framework Core** — Type-safe database access
- ✅ **SQLite** — Lightweight, file-based database
- ✅ **Services Layer** — Clean separation of concerns
- ✅ **Admin Pages** — Event, participant, venue management
- ✅ **Public Pages** — Event listing and details
- ✅ **Hot Reload** — See changes instantly during development

---

## 👥 Team Development

### For New Team Members
1. Clone the repo
2. Run `dotnet restore`
3. Run `dotnet watch --project CommunityEventManagementSystem.App`
4. Open **https://localhost:5001** in your browser

### Working with EF Core
- Always apply migrations before pushing changes
- Never commit `*.db` files (they're in `.gitignore`)
- Test migrations locally before committing

---

## 🐛 Troubleshooting

### "dotnet: command not found"
Verify your .NET 10 SDK is installed:
```bash
dotnet --info
```

### HTTPS Certificate Error
Trust the development certificate (one-time setup):
```bash
dotnet dev-certs https --trust
```

### Port Already in Use
Change port in `Properties/launchSettings.json` under the `https` or `http` profile.

### Database Issues
Delete `app.db` and let it regenerate on next run:
```bash
rm CommunityEventManagementSystem.App/app.db
dotnet watch --project CommunityEventManagementSystem.App
```

---

## 📋 Platform-Specific Notes

### Windows
- Use **PowerShell** or **Command Prompt** (both work fine)
- .NET 10 installer handles PATH automatically

### macOS
- Install via `.pkg` from Microsoft's site
- May need to restart terminal after installation

### Linux
- Follow distro-specific instructions (Ubuntu, Fedora, etc.)
- Ensure `dotnet` is in your PATH: `which dotnet`

---

## 📚 Useful Commands

```bash
# Build the project
dotnet build

# Run tests (if added later)
dotnet test

# Clean build artifacts
dotnet clean

# Format code
dotnet format

# Check for dependency updates
dotnet outdated
```

---

## 📝 License

This project is licensed under the **Apache License 2.0** — see [LICENSE](LICENSE) file.

---

## 🔗 Resources

- [Blazor Documentation](https://learn.microsoft.com/en-us/aspnet/core/blazor/)
- [Entity Framework Core](https://learn.microsoft.com/en-us/ef/core/)
- [.NET 10 Download](https://dotnet.microsoft.com/en-us/download/dotnet/10.0)
- [VS Code for C#](https://code.visualstudio.com/docs/languages/csharp)

---

**Happy coding! 🎉**
