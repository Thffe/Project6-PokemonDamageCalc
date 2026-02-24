# ---- Base runtime ----
FROM mcr.microsoft.com/dotnet/aspnet:8.0 AS base
WORKDIR /app

# Force app to listen on 8080 inside the container
ENV ASPNETCORE_URLS=http://+:8080
EXPOSE 8080

# ---- Build ----
FROM mcr.microsoft.com/dotnet/sdk:8.0 AS build
ARG BUILD_CONFIGURATION=Release
WORKDIR /src

COPY ["Project6-PokemonDamageCalc/Project6-PokemonDamageCalc.csproj", "Project6-PokemonDamageCalc/"]
RUN dotnet restore "./Project6-PokemonDamageCalc/Project6-PokemonDamageCalc.csproj"

COPY . .
WORKDIR "/src/Project6-PokemonDamageCalc"
RUN dotnet build "./Project6-PokemonDamageCalc.csproj" -c $BUILD_CONFIGURATION -o /app/build

# ---- Publish ----
FROM build AS publish
ARG BUILD_CONFIGURATION=Release
RUN dotnet publish "./Project6-PokemonDamageCalc.csproj" -c $BUILD_CONFIGURATION -o /app/publish /p:UseAppHost=false

# ---- Final ----
FROM base AS final
WORKDIR /app
COPY --from=publish /app/publish .
ENTRYPOINT ["dotnet", "Project6-PokemonDamageCalc.dll"]
