# Сборка ef bundle для применения миграция для основного api

# Base stage: установим базовый образ для запуска приложения
FROM mcr.microsoft.com/dotnet/aspnet:9.0 AS base
USER $APP_UID
WORKDIR /app
EXPOSE 80

# Build stage: образ SDK для сборки приложения
FROM mcr.microsoft.com/dotnet/sdk:9.0 AS build
ARG BUILD_CONFIGURATION=Release
ENV DOCKER_BUILD=true
RUN apt-get update && apt-get -y install gss-ntlmssp

# Скопируем всю корневую директорию проекта в контейнер
WORKDIR /
COPY . .

# Восстанавливаем зависимости
WORKDIR /src/CoinKeeper.Backend.Api
RUN dotnet restore "CoinKeeper.Backend.Api.csproj" --verbosity n

# Устанавливаем dotnet-ef глобально
RUN dotnet tool install --global dotnet-ef --version 9.0.4

# Добавляем путь к глобальным инструментам в PATH
ENV PATH="$PATH:/root/.dotnet/tools"

# Возвращаемся в корневую директорию приложения
WORKDIR /

# Собираем EF Bundle
RUN dotnet ef migrations bundle -s ./src/CoinKeeper.Backend.Api/CoinKeeper.Backend.Api.csproj \
    -p ./src/Common/CoinKeeper.Infrastructure/CoinKeeper.Infrastructure.csproj \
    -c DataContext --self-contained --force

# Production stage: минимальный образ для запуска
FROM mcr.microsoft.com/dotnet/runtime:9.0 AS runtime

# Устанавливаем рабочую директорию
WORKDIR /

# Копируем только efbundle из предыдущего этапа
COPY --from=build /efbundle .

# Запускаем EF Bundle с передачей строки подключения
ENTRYPOINT ["sh", "-c", "echo $ConnectionStrings__DataContext && ./efbundle --connection \"$ConnectionStrings__DataContext\""]
