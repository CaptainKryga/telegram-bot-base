FROM mcr.microsoft.com/dotnet/runtime:8.0 AS base
USER $APP_UID
WORKDIR /app

FROM mcr.microsoft.com/dotnet/sdk:8.0 AS build
ARG BUILD_CONFIGURATION=Release
WORKDIR /src
COPY ["telegram-bot-base.csproj", "./"]
RUN dotnet restore "telegram-bot-base.csproj"
COPY . .
WORKDIR "/src/"
RUN dotnet build "telegram-bot-base.csproj" -c $BUILD_CONFIGURATION -o /app/build

ENTRYPOINT ["dotnet", "telegram-bot-base.dll"]
FROM build AS publish
ARG BUILD_CONFIGURATION=Release
RUN dotnet publish "telegram-bot-base.csproj" -c $BUILD_CONFIGURATION -o /app/publish /p:UseAppHost=false

FROM base AS final
WORKDIR /app
COPY --from=publish /app/publish .
ENTRYPOINT ["dotnet", "telegram-bot-base.dll"]