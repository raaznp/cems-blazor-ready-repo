#!/usr/bin/env bash
set -euo pipefail

# Local bootstrap: scaffold Blazor Server app and solution if missing.
APP_DIR="CommunityEventManagementSystem.App"
SLN_NAME="CommunityEventManagementSystem"

if [ -d "$APP_DIR" ]; then
  echo "[bootstrap] Existing Blazor app found at $APP_DIR. Skipping creation."
else
  echo "[bootstrap] Creating solution and Blazor Web App (.NET 10)..."
  if [ -f "$SLN_NAME.sln" ] || [ -f "$SLN_NAME.slnx" ]; then
    echo "[bootstrap] Existing solution found ($SLN_NAME.sln/slnx). Skipping creation."
  else
    dotnet new sln -n "$SLN_NAME"
  fi
  # Use the unified Blazor template available in .NET 8+ / 10
  # Older 'blazorserver' template is no longer available on .NET 10
  dotnet new blazor -n "$APP_DIR"
  if [ -f "$SLN_NAME.sln" ]; then
    dotnet sln "$SLN_NAME.sln" add "$APP_DIR/$APP_DIR.csproj"
  elif [ -f "$SLN_NAME.slnx" ]; then
    dotnet sln "$SLN_NAME.slnx" add "$APP_DIR/$APP_DIR.csproj"
  else
    echo "[bootstrap] Warning: No solution file found to add project to."
  fi

  echo "[bootstrap] Adding EF Core packages (SQLite + Design)..."
  dotnet add "$APP_DIR" package Microsoft.EntityFrameworkCore.Sqlite
  dotnet add "$APP_DIR" package Microsoft.EntityFrameworkCore.Design
fi

echo "[bootstrap] Restoring packages..."
dotnet restore

echo "[bootstrap] Done. Try: 'dotnet watch --project $APP_DIR'"
