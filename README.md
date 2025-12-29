# communityeventmanagementsystem
Community Event Management System in Blazor framework as the project of  CET254 - Advanced Programming - ISMT College, affiliated via Sunderland University, UK.
## Dev Containers (Blazor / .NET 9)
This repository includes a ready-to-use Dev Container for Blazor development. It installs the .NET 9 SDK, SQLite, and the EF Core CLI, and can auto-bootstrap a new Blazor Server app on first open.

### Prerequisites
- Docker (Desktop on Windows/macOS, or Docker Engine on Linux)
- Visual Studio Code
- "Dev Containers" extension (ms-vscode-remote.remote-containers)

### Open in Dev Container
1. Open this folder in VS Code.
2. When prompted, select "Reopen in Container". Or run: Command Palette → "Dev Containers: Rebuild and Reopen in Container".
3. On first start, the container runs a bootstrap script that creates a solution and a Blazor Server app if missing.

What the container does:
- Builds from `.devcontainer/Dockerfile` (based on .NET 9 devcontainer image)
- Installs SQLite and EF Core CLI
- Runs `.devcontainer/bootstrap.sh` to scaffold:
	- `CommunityEventManagementSystem.sln`
	- `CommunityEventManagementSystem.App` (Blazor Server)
	- Adds EF packages for SQLite

### Run the app
From inside the container terminal:
```
dotnet watch --project CommunityEventManagementSystem.App
```
Ports 5000/5001 are forwarded automatically. Open the printed URL in your browser.

### Database (SQLite) quick start
Create a migration and update the dev database:
```
dotnet ef migrations add InitialCreate --project CommunityEventManagementSystem.App
dotnet ef database update --project CommunityEventManagementSystem.App
```
Configure your `DbContext` with a SQLite connection string in the app (to be added when you implement persistence).

### Troubleshooting
- If the container fails to build, ensure Docker is running and you have network access to pull `mcr.microsoft.com` images.
- You can re-run the bootstrap script manually:
```
bash .devcontainer/bootstrap.sh
```
- To start fresh, remove the generated app folder and solution, then rebuild the container.

### Platform notes
- Windows: Use Docker Desktop with WSL2 backend enabled. Ensure virtualization is on. VS Code prompts to reopen in Dev Container; this uses WSL2 automatically.
- macOS: Use Docker Desktop; grant file sharing access to your workspace folder if prompted.
- Linux: Docker Engine and VS Code Dev Containers work natively.
