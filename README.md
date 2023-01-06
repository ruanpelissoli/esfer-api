# esfer-api

# JWT Configuration

dotnet user-jwts create

# Migrations

dotnet ef migrations add Identity --context EsferDbContext -o "Shared/Database" --project .\src\Esfer.API\Esfer.API.csproj

dotnet ef database update --context EsferDbContext --project .\src\Esfer.API\Esfer.API.csproj
