# Blazor / .NET 10 LTS Dev Container
FROM mcr.microsoft.com/devcontainers/dotnet:1-10-bookworm

# Install SQLite and certificates
USER root
RUN apt-get update \
    && apt-get install -y --no-install-recommends sqlite3 libsqlite3-dev ca-certificates \
    && rm -rf /var/lib/apt/lists/*

# Install EF Core CLI as global tool for vscode user
USER vscode
ENV PATH="/home/vscode/.dotnet/tools:${PATH}"
RUN dotnet tool install --global dotnet-ef

# Switch back to root by default (devcontainers will set remoteUser=vscode)
USER root
