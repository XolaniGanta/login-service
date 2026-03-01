# Build stage
FROM mcr.microsoft.com/dotnet/sdk:10.0 AS build
WORKDIR /src
# Copy csproj and restore
COPY Login-Service.csproj .
RUN dotnet restore
# Copy everything else
COPY . .
RUN dotnet publish -c Release -o /app/publish /p:UseAppHost=false
# Runtime stage
FROM mcr.microsoft.com/dotnet/aspnet:10.0 AS final
WORKDIR /app
COPY --from=build /app/publish .
ENV ASPNETCORE_URLS=http://+:8080
ENTRYPOINT ["dotnet", "Login-Service.dll"]