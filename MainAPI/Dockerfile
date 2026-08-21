FROM mcr.microsoft.com/dotnet/sdk:10.0 AS build
WORKDIR /src
COPY ["MainAPI.csproj", "./"]
RUN dotnet restore "./MainAPI.csproj"
COPY . .
RUN dotnet publish "MainAPI.csproj" -c Release -o /app/publish

# El Runtime (10.0)
FROM mcr.microsoft.com/dotnet/aspnet:10.0 AS final
WORKDIR /app
COPY --from=build /app/publish .
ENTRYPOINT ["dotnet", "MainAPI.dll"]
