# ===== Etapa 1: Build =====
FROM mcr.microsoft.com/dotnet/sdk:10.0 AS build
WORKDIR /src

# Copiamos solo los archivos de proyecto primero para aprovechar la cache de Docker
COPY vital-trek-platform/vital-trek-platform.sln ./vital-trek-platform/
COPY vital-trek-platform/NexumDevs.VitalTrek.Platform/NexumDevs.VitalTrek.Platform.csproj ./vital-trek-platform/NexumDevs.VitalTrek.Platform/

WORKDIR /src/vital-trek-platform
RUN dotnet restore NexumDevs.VitalTrek.Platform/NexumDevs.VitalTrek.Platform.csproj

# Ahora copiamos el resto del código fuente
WORKDIR /src
COPY vital-trek-platform/ ./vital-trek-platform/

WORKDIR /src/vital-trek-platform/NexumDevs.VitalTrek.Platform
RUN dotnet publish NexumDevs.VitalTrek.Platform.csproj -c Release -o /app/publish --no-restore

# ===== Etapa 2: Runtime =====
FROM mcr.microsoft.com/dotnet/aspnet:10.0 AS final
WORKDIR /app

# Railway inyecta la variable PORT; ASP.NET debe escuchar en ese puerto
ENV ASPNETCORE_URLS=http://+:8080
EXPOSE 8080

COPY --from=build /app/publish .

ENTRYPOINT ["dotnet", "NexumDevs.VitalTrek.Platform.dll"]