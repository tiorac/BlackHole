#See https://aka.ms/customizecontainer to learn how to customize your debug container and how Visual Studio uses this Dockerfile to build your images for faster debugging.

FROM mcr.microsoft.com/dotnet/aspnet:8.0 AS base
USER app
WORKDIR /app
EXPOSE 8080
EXPOSE 8081

FROM mcr.microsoft.com/dotnet/sdk:8.0 AS build
ARG BUILD_CONFIGURATION=Release
WORKDIR /src
COPY ["BlackHole/Server/BlackHole.Server.csproj", "BlackHole/Server/"]
COPY ["BlackHole/Client/BlackHole.Client.csproj", "BlackHole/Client/"]
COPY ["BlackHole/Shared/BlackHole.Shared.csproj", "BlackHole/Shared/"]
RUN dotnet restore "./BlackHole/Server/./BlackHole.Server.csproj"
COPY . .
WORKDIR "/src/BlackHole/Server"
RUN dotnet build "./BlackHole.Server.csproj" -c $BUILD_CONFIGURATION -o /app/build

FROM build AS publish
ARG BUILD_CONFIGURATION=Release
RUN dotnet publish "./BlackHole.Server.csproj" -c $BUILD_CONFIGURATION -o /app/publish /p:UseAppHost=false

FROM base AS final
WORKDIR /app
COPY --from=publish /app/publish .
VOLUME /app/config
ENTRYPOINT ["dotnet", "BlackHole.Server.dll"]