#Контекст - корень репы CoinKeeper
# Оптимизированный Dockerfile для ARM64/Raspberry Pi

# Base stage: установим базовый образ для запуска приложения
FROM mcr.microsoft.com/dotnet/aspnet:9.0 AS base
USER $APP_UID
WORKDIR /app
EXPOSE 80

# Build stage: образ SDK для сборки приложения
FROM mcr.microsoft.com/dotnet/sdk:9.0 AS build
ARG BUILD_CONFIGURATION=Release
ARG TARGETARCH

# Настройки для ARM64 и стабилизации сборки
ENV DOCKER_BUILD=true \
    DOTNET_CLI_TELEMETRY_OPTOUT=1 \
    DOTNET_SKIP_FIRST_TIME_EXPERIENCE=1 \
    DOTNET_NOLOGO=1 \
    NUGET_XMLDOC_MODE=skip \
    DOTNET_USE_POLLING_FILE_WATCHER=true

# Установка зависимостей с оптимизацией для ARM64
RUN apt-get update && apt-get -y install --no-install-recommends \
    gss-ntlmssp \
    && rm -rf /var/lib/apt/lists/*

# Restore stage: отдельный этап для кэширования зависимостей
FROM build AS restore
WORKDIR /src

# Копируем только файлы проектов для лучшего кэширования
COPY ["src/CoinKeeper.Backend.Api/CoinKeeper.Backend.Api.csproj", "CoinKeeper.Backend.Api/"]
COPY ["src/Authentication/CoinKeeper.Authentication/CoinKeeper.Authentication.csproj", "Authentication/CoinKeeper.Authentication/"]
COPY ["src/Authentication/CoinKeeper.Authentication.Domain/CoinKeeper.Authentication.Domain.csproj", "Authentication/CoinKeeper.Authentication.Domain/"]
COPY ["src/CoinKeeper.Finance/CoinKeeper.Finance.csproj", "CoinKeeper.Finance/"]
COPY ["src/CoinKeeper.Logging/CoinKeeper.Logging.csproj", "CoinKeeper.Logging/"]
COPY ["src/Common/CoinKeeper.Common/CoinKeeper.Common.csproj", "Common/CoinKeeper.Common/"]
COPY ["src/Common/CoinKeeper.Common.Domain/CoinKeeper.Common.Domain.csproj", "Common/CoinKeeper.Common.Domain/"]
COPY ["src/Common/CoinKeeper.Infrastructure/CoinKeeper.Infrastructure.csproj", "Common/CoinKeeper.Infrastructure/"]
COPY ["src/Hangfire/CoinKeeper.Hangfire.AspNetCore/CoinKeeper.Hangfire.AspNetCore.csproj", "Hangfire/CoinKeeper.Hangfire.AspNetCore/"]
COPY ["global.json", "./"]

# Восстановление зависимостей с оптимизацией для ARM64
WORKDIR /src/CoinKeeper.Backend.Api
RUN dotnet restore "CoinKeeper.Backend.Api.csproj" \
    --verbosity minimal \
    --disable-parallel \
    --no-cache \
    --runtime linux-arm64

# Build stage: сборка приложения
FROM restore AS build-app
WORKDIR /src

# Копируем весь исходный код
COPY src/ .

WORKDIR /src/CoinKeeper.Backend.Api
RUN dotnet build "CoinKeeper.Backend.Api.csproj" \
    -c $BUILD_CONFIGURATION \
    -o /app/build \
    --no-restore \
    --verbosity minimal

# Publish stage: публикация артефактов сборки
FROM build-app AS publish
ARG BUILD_CONFIGURATION=Release
RUN dotnet publish "CoinKeeper.Backend.Api.csproj" \
    -c $BUILD_CONFIGURATION \
    -o /app/publish \
    --no-restore \
    --no-build \
    --runtime linux-arm64 \
    --self-contained false \
    /p:UseAppHost=false \
    /p:PublishTrimmed=false

# Final stage: создание финального образа на основе base и копирование опубликованных файлов
FROM base AS final

ARG VERSION
ARG BUILD_NUMBER
ENV APP_VERSION=$VERSION \
    BUILD_NUMBER=$BUILD_NUMBER \
    ASPNETCORE_URLS=http://+:80 \
    DOTNET_RUNNING_IN_CONTAINER=true \
    DOTNET_USE_POLLING_FILE_WATCHER=true

WORKDIR /app
COPY --from=publish /app/publish .

# Оптимизация для Raspberry Pi
RUN groupadd -r appgroup && useradd -r -g appgroup appuser
USER appuser

ENTRYPOINT ["dotnet", "CoinKeeper.Backend.Api.dll"]
