# =======================================================
# 1. Build Stage
# Uses the .NET 10 SDK image to restore and compile code
# =======================================================
FROM mcr.microsoft.com/dotnet/sdk:10.0-preview AS build
WORKDIR /src

# Copy all project definition (.csproj) files first to optimize Docker layer caching
COPY ["schoolmanagement.Api/schoolmanagement.Api.csproj", "schoolmanagement.Api/"]
COPY ["Schoolmanagement.Services/Schoolmanagement.Services.csproj", "Schoolmanagement.Services/"]
COPY ["Schoolmangenment.Entities/Schoolmangenment.Entities.csproj", "Schoolmangenment.Entities/"]
COPY ["Schoolmangenment.Repository/Schoolmangenment.Repository.csproj", "Schoolmangenment.Repository/"]

# Restore all NuGet packages
RUN dotnet restore "schoolmanagement.Api/schoolmanagement.Api.csproj"

# Copy all remaining source files
COPY . .

# Build and publish the production release
WORKDIR "/src/schoolmanagement.Api"
RUN dotnet publish "schoolmanagement.Api.csproj" -c Release -o /app/publish /p:UseAppHost=false

# =======================================================
# 2. Runtime Stage
# Lightweight ASP.NET Core runtime to run the compiled API
# =======================================================
FROM mcr.microsoft.com/dotnet/aspnet:10.0-preview AS final
WORKDIR /app

# Copy the compiled binaries from the build stage
COPY --from=build /app/publish .

# Port and Proxy configuration for Railway
ENV ASPNETCORE_HTTP_PORTS=8080
ENV ASPNETCORE_FORWARDEDHEADERS_ENABLED=true
EXPOSE 8080

# Run the API
ENTRYPOINT ["dotnet", "schoolmanagement.Api.dll"]
