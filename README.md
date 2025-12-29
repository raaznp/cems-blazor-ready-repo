# communityeventmanagementsystem
Community Event Management System in Blazor framework as the project of  CET254 - Advanced Programming - ISMT College, affiliated via Sunderland University, UK.
## Local Development (Blazor / .NET 10 LTS)
This repo targets local development with the .NET 10 LTS SDK installed.

### Prerequisites
- Visual Studio Code (or your preferred editor)
- .NET 10 LTS SDK: https://dotnet.microsoft.com/en-us/download/dotnet/10.0

### Setup
The solution and Blazor Web App are pre-scaffolded. Just restore and run:
```
dotnet restore
dotnet watch --project CommunityEventManagementSystem.App
```

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
- If `dotnet watch` fails, verify your SDK with:
```
dotnet --info
```

### Platform notes
- **Windows**: .NET SDK 10 LTS runs via standard installer; use PowerShell/cmd as preferred.
- **macOS**: Install .NET SDK 10 LTS pkg; use Terminal or VS Code.
- **Linux**: Install .NET SDK 10 LTS for your distro; ensure `dotnet` is on PATH.
