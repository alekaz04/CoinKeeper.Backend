# Dockerfile для сборки EF Bundle для применения миграций
# Оптимизирован для ARM64/Raspberry Pi

FROM --platform=$BUILDPLATFORM mcr.microsoft.com/dotnet/sdk:9.0 AS build

WORKDIR /src
COPY . .

# Устанавливаем dotnet-ef как глобальный инструмент
RUN dotnet tool install --global dotnet-ef --version 9.0.4

# Добавляем путь к глобальным инструментам в PATH
ENV PATH="$PATH:/root/.dotnet/tools"

# Восстанавливаем зависимости для ARM64
RUN dotnet restore "src/CoinKeeper.Backend.Api/CoinKeeper.Backend.Api.csproj" \
    -r linux-arm64

# Собираем EF Bundle для ARM64
RUN dotnet ef migrations bundle \
    -s ./src/CoinKeeper.Backend.Api/CoinKeeper.Backend.Api.csproj \
    -p ./src/Common/CoinKeeper.Infrastructure/CoinKeeper.Infrastructure.csproj \
    -c DataContext \
    --self-contained \
    -r linux-arm64 \
    --force \
    -o /app/efbundle

# Финальный образ для запуска миграций
FROM mcr.microsoft.com/dotnet/runtime:9.0 AS final

WORKDIR /app
COPY --from=build /app/efbundle .

# Делаем файл исполняемым
RUN chmod +x ./efbundle

# Запускаем EF Bundle с передачей строки подключения через переменную окружения
ENTRYPOINT ["sh", "-c", "./efbundle --connection \"$ConnectionStrings__DataContext\""]
