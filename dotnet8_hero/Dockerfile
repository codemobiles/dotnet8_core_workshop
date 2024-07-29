FROM mcr.microsoft.com/dotnet/aspnet:8.0 AS base
WORKDIR /app
EXPOSE 8081

ENV ASPNETCORE_URLS=http://+:8081
FROM mcr.microsoft.com/dotnet/sdk:8.0 AS build
WORKDIR /src
COPY ["dotnet8_hero.csproj", "./"]
RUN dotnet restore "dotnet8_hero.csproj"
COPY . .
WORKDIR "/src/."
RUN dotnet build "dotnet8_hero.csproj" -c Release -o /app/build

FROM build AS publish
RUN dotnet publish "dotnet8_hero.csproj" -c Release -o /app/publish /p:UseAppHost=false

FROM base AS final
WORKDIR /app
COPY --from=publish /app/publish .
ENTRYPOINT ["dotnet", "dotnet8_hero.dll"]
