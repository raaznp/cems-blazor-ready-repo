# communityeventmanagementsystem
Community Event Management System in Blazor framework as the project of  CET254 - Advanced Programming - ISMT College, affiliated via Sunderland University, UK.
## Local Development (Blazor / .NET 10 LTS)
This repo targets local development with the .NET 10 LTS SDK installed. No Docker or Dev Containers required.

### Prerequisites
- Visual Studio Code (or your preferred editor)
- .NET 10 LTS SDK: https://dotnet.microsoft.com/en-us/download/dotnet/10.0

### First-time setup (bootstrap)
Scaffold the solution and Blazor Web App if missing:
```
bash scripts/bootstrap.sh
```

### Run the app with hot reload
```
dotnet watch --project CommunityEventManagementSystem.App
```
Open the printed URL in your browser (defaults to http://localhost:5000 or https).

## Local Development (No Docker)
If you prefer to work without containers and you have the .NET 10 LTS SDK installed:

1. Bootstrap the app and solution (first time only):
```
bash .devcontainer/bootstrap.sh
```
2. Run with hot reload:
```
dotnet watch --project CommunityEventManagementSystem.App
```
3. Optional EF quick start:
```
dotnet ef migrations add InitialCreate --project CommunityEventManagementSystem.App
dotnet ef database update --project CommunityEventManagementSystem.App
```

Notes:
- You do not need the host SDK when using Dev Containers; the SDK is inside the container.
- When working locally, ensure your PATH uses the .NET 10 SDK (`dotnet --info`).

### Blazor + EF Core (SQLite)
- Connection string: `DefaultConnection` in CommunityEventManagementSystem.App/appsettings.json (defaults to `Data Source=app.db`).
- DbContext: `Data/ApplicationDbContext.cs` with a `DbSet<Event>`.
- Model: `Models/Event.cs`.
- Startup: The app ensures a dev database exists on startup (applies pending migrations if any, otherwise creates the schema).

Create a migration and update the dev database:
```
dotnet ef migrations add InitialCreate --project CommunityEventManagementSystem.App
dotnet ef database update --project CommunityEventManagementSystem.App
```
Configure your `DbContext` with a SQLite connection string in the app (to be added when you implement persistence).

### Troubleshooting
- Re-run the bootstrap script any time:
```
bash scripts/bootstrap.sh
```
- If `dotnet watch` fails, verify your SDK with:
```
dotnet --info
```

### Platform notes
- Windows: .NET SDK 10 LTS runs via standard installer; use PowerShell/bash as preferred.
- macOS: Install .NET SDK 10 LTS pkg; use Terminal or VS Code.
- Linux: Install .NET SDK 10 LTS for your distro; ensure `dotnet` is on PATH.
