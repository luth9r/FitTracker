FROM mcr.microsoft.com/dotnet/sdk:10.0 AS build
WORKDIR /src

# Copy project files for dependency resolution
COPY ["src/FitTracker.Api/FitTracker.Api.csproj", "FitTracker.Api/"]
COPY ["src/FitTracker.Application/FitTracker.Application.csproj", "FitTracker.Application/"]
COPY ["src/FitTracker.Infrastructure/FitTracker.Infrastructure.csproj", "FitTracker.Infrastructure/"]
COPY ["src/FitTracker.Domain/FitTracker.Domain.csproj", "FitTracker.Domain/"]

# Restore dependencies
RUN dotnet restore "FitTracker.Api/FitTracker.Api.csproj"

# Copy only src/ folder with all source code
COPY src/ .

# Build the application
RUN dotnet build "FitTracker.Api/FitTracker.Api.csproj" -c Release -o /app/build

FROM build AS publish
RUN dotnet publish "FitTracker.Api/FitTracker.Api.csproj" -c Release -o /app/publish

FROM mcr.microsoft.com/dotnet/aspnet:10.0
WORKDIR /app
COPY --from=publish /app/publish .
EXPOSE 5000 5001
ENTRYPOINT ["dotnet", "FitTracker.Api.dll"]
