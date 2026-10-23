# Etapa 1: compilar y publicar la API
FROM mcr.microsoft.com/dotnet/sdk:8.0 AS build

WORKDIR /src

# Copiar la solución y los proyectos
COPY . .

# Publicar la API en modo Release
RUN dotnet publish ./PortalProfesores.Api/PortalProfesores.Api.csproj \
    -c Release \
    -o /app/publish \
    /p:UseAppHost=false

# Etapa 2: ejecutar la API
FROM mcr.microsoft.com/dotnet/aspnet:8.0 AS final

WORKDIR /app

COPY --from=build /app/publish .

ENV ASPNETCORE_URLS=http://+:8080
EXPOSE 8080

ENTRYPOINT ["dotnet", "PortalProfesores.Api.dll"]