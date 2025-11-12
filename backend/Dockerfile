# Build stage - compile and publish the application
FROM mcr.microsoft.com/dotnet/sdk:10.0 AS build
WORKDIR /src

# Copy project files for dependency resolution
COPY ["FitTracker.Api/FitTracker.Api.csproj", "FitTracker.Api/"]
COPY ["FitTracker.Application/FitTracker.Application.csproj", "FitTracker.Application/"]
COPY ["FitTracker.Infrastructure/FitTracker.Infrastructure.csproj", "FitTracker.Infrastructure/"]
COPY ["FitTracker.Domain/FitTracker.Domain.csproj", "FitTracker.Domain/"]

# Restore dependencies
RUN dotnet restore "FitTracker.Api/FitTracker.Api.csproj"

# Copy all source code
COPY . .

# Build the application
RUN dotnet build "FitTracker.Api/FitTracker.Api.csproj" -c Release -o /app/build

# Publish stage - create production-ready output
FROM build AS publish
RUN dotnet publish "FitTracker.Api/FitTracker.Api.csproj" -c Release -o /app/publish

# Runtime stage - minimal image for running the application
FROM mcr.microsoft.com/dotnet/aspnet:9.0
WORKDIR /app

# Copy published application from publish stage
COPY --from=publish /app/publish .

# Expose HTTP and HTTPS ports
EXPOSE 5000 5001

# Start the application
ENTRYPOINT ["dotnet", "FitTracker.Api.dll"]