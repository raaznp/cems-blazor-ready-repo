#!/usr/bin/env bash
set -euo pipefail

# Bootstrap a Blazor Server app and solution if not present.

APP_DIR="CommunityEventManagementSystem.App"
SLN_NAME="CommunityEventManagementSystem"

if [ -d "$APP_DIR" ]; then
  echo "[bootstrap] Existing Blazor app found at $APP_DIR. Skipping creation."
else
  echo "[bootstrap] Creating solution and Blazor Server app..."
  dotnet new sln -n "$SLN_NAME"
  dotnet new blazorserver -n "$APP_DIR"
  dotnet sln "$SLN_NAME.sln" add "$APP_DIR/$APP_DIR.csproj"

  echo "[bootstrap] Adding EF Core packages (SQLite + Design)..."
  dotnet add "$APP_DIR" package Microsoft.EntityFrameworkCore.Sqlite
  dotnet add "$APP_DIR" package Microsoft.EntityFrameworkCore.Design
fi

echo "[bootstrap] Restoring packages..."
dotnet restore

echo "[bootstrap] Done. Try: 'dotnet watch --project $APP_DIR'"
