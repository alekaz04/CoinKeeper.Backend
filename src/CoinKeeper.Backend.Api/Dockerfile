#Контекст - корень репы CoinKeeper

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

WORKDIR /src/CoinKeeper.Backend.Api
RUN dotnet restore "CoinKeeper.Backend.Api.csproj" --verbosity n

RUN dotnet build "CoinKeeper.Backend.Api.csproj" -c $BUILD_CONFIGURATION -o /app/build

# Publish stage: публикация артефактов сборки
FROM build AS publish
ARG BUILD_CONFIGURATION=Release
RUN dotnet publish "CoinKeeper.Backend.Api.csproj" -c $BUILD_CONFIGURATION -o /app/publish /p:UseAppHost=false

# Final stage: создание финального образа на основе base и копирование опубликованных файлов
FROM base AS final

ARG VERSION
ARG BUILD_NUMBER
ENV APP_VERSION=$VERSION \
    BUILD_NUMBER=$BUILD_NUMBER

WORKDIR /app
COPY --from=publish /app/publish .
ENTRYPOINT ["dotnet", "CoinKeeper.Backend.Api.dll"]
