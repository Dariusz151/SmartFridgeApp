# ---- Build stage ----
FROM mcr.microsoft.com/dotnet/sdk:10.0 AS build
WORKDIR /repo

# Copy solution-level build props first (for better layer caching)
COPY .build/ .build/

# Copy source and restore dependencies
COPY src/ src/
RUN dotnet restore src/SmartFridgeApp.API/SmartFridgeApp.API.csproj

# Publish
RUN dotnet publish src/SmartFridgeApp.API/SmartFridgeApp.API.csproj \
    --configuration Release \
    --runtime linux-x64 \
    --no-restore \
    --output /app/publish

# ---- Runtime stage ----
FROM mcr.microsoft.com/dotnet/aspnet:10.0 AS runtime
RUN apt-get update && apt-get install -y --no-install-recommends libc-dev \
    && rm -rf /var/lib/apt/lists/*

WORKDIR /app
COPY --from=build /app/publish .

# Cloud Run injects PORT env var; default to 8080
ENV ASPNETCORE_URLS=http://+:8080
EXPOSE 8080

ENTRYPOINT ["./SmartFridgeApp.API"]
