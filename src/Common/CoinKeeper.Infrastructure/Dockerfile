# Сборка ef bundle для применения миграций для основного api
# Оптимизированный Dockerfile для ARM64/Raspberry Pi

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

# Build stage: сборка EF Bundle
FROM restore AS build-bundle
WORKDIR /src

# Копируем весь исходный код
COPY src/ .

# Устанавливаем dotnet-ef глобально с оптимизацией
RUN dotnet tool install --global dotnet-ef --version 9.0.4 \
    --verbosity minimal

# Добавляем путь к глобальным инструментам в PATH
ENV PATH="$PATH:/root/.dotnet/tools"

# Возвращаемся в корневую директорию приложения
WORKDIR /src

# Собираем EF Bundle с оптимизацией для ARM64
RUN dotnet ef migrations bundle \
    -s ./CoinKeeper.Backend.Api/CoinKeeper.Backend.Api.csproj \
    -p ./Common/CoinKeeper.Infrastructure/CoinKeeper.Infrastructure.csproj \
    -c DataContext \
    --self-contained \
    -r linux-arm64 \
    --force \
    --verbosity minimal \
    -o /efbundle

# Production stage: минимальный образ для запуска
FROM mcr.microsoft.com/dotnet/runtime:9.0 AS runtime

# Настройки для ARM64 runtime
ENV DOTNET_RUNNING_IN_CONTAINER=true \
    DOTNET_USE_POLLING_FILE_WATCHER=true

# Создаем пользователя для безопасности
RUN groupadd -r migrationgroup && useradd -r -g migrationgroup migrationuser

# Устанавливаем рабочую директорию
WORKDIR /app

# Копируем только efbundle из предыдущего этапа
COPY --from=build-bundle /efbundle ./efbundle

# Делаем файл исполняемым и меняем владельца
RUN chmod +x ./efbundle && \
    chown migrationuser:migrationgroup ./efbundle

# Переключаемся на непривилегированного пользователя
USER migrationuser

# Запускаем EF Bundle с передачей строки подключения
ENTRYPOINT ["sh", "-c", "echo 'Starting migration with connection:' && echo $ConnectionStrings__DataContext && ./efbundle --connection \"$ConnectionStrings__DataContext\""]
