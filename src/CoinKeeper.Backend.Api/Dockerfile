FROM --platform=$BUILDPLATFORM mcr.microsoft.com/dotnet/sdk:9.0 AS build

WORKDIR /src
COPY . .

# Восстанавливаем зависимости внутри контейнера
RUN dotnet restore "src/CoinKeeper.Backend.Api/CoinKeeper.Backend.Api.csproj" \
    -r linux-arm64

# Публикуем приложение
RUN dotnet publish "src/CoinKeeper.Backend.Api/CoinKeeper.Backend.Api.csproj" \
    -c Release \
    -o /app/publish \
    -r linux-arm64 \
    --self-contained false \
    --no-restore

# Финальный образ для ARM64
FROM mcr.microsoft.com/dotnet/aspnet:9.0 AS final
WORKDIR /app
COPY --from=build /app/publish .
EXPOSE 80
EXPOSE 443
ENTRYPOINT ["dotnet", "CoinKeeper.Backend.Api.dll"]
