# Multi-stage build for smaller image size
FROM mcr.microsoft.com/dotnet/sdk:9.0 AS build
WORKDIR /src

# Copy csproj and restore dependencies
COPY ["InventoryApi.csproj", "./"]
RUN dotnet restore "InventoryApi.csproj"

# Copy everything else and build
COPY . .
RUN dotnet build "InventoryApi.csproj" -c Release -o /app/build

FROM build AS publish
RUN dotnet publish "InventoryApi.csproj" -c Release -o /app/publish /p:UseAppHost=false

# Build runtime image
FROM mcr.microsoft.com/dotnet/aspnet:9.0 AS final
WORKDIR /app
EXPOSE 8080
COPY --from=publish /app/publish .
ENTRYPOINT ["dotnet", "InventoryApi.dll"]
