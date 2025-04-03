FROM mcr.microsoft.com/dotnet/runtime:8.0 AS base
USER $APP_UID
WORKDIR /app

FROM mcr.microsoft.com/dotnet/sdk:8.0 AS build
ARG BUILD_CONFIGURATION=Release
WORKDIR /src
COPY ["TelegramBot.Service.csproj", "./"]
RUN dotnet restore "TelegramBot.Service.csproj"
COPY . .
WORKDIR "/src/"
RUN dotnet build "TelegramBot.Service.csproj" -c $BUILD_CONFIGURATION -o /app/build

ENTRYPOINT ["dotnet", "TelegramBot.Service.dll"]
FROM build AS publish
ARG BUILD_CONFIGURATION=Release
RUN dotnet publish "TelegramBot.Service.csproj" -c $BUILD_CONFIGURATION -o /app/publish /p:UseAppHost=false

FROM base AS final
WORKDIR /app
COPY --from=publish /app/publish .
ENTRYPOINT ["dotnet", "TelegramBot.Service.dll"]