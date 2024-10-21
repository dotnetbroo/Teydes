FROM mcr.microsoft.com/dotnet/sdk:7.0 AS build
WORKDIR /src
COPY . .
RUN dotnet restore "src/Tedyes.TelegramBot/Tedyes.TelegramBot.csproj"
WORKDIR "/src/src/Tedyes.TelegramBot"
RUN dotnet build "Tedyes.TelegramBot.csproj" -c Release -o /app/build

FROM build AS publish
RUN dotnet publish "Tedyes.TelegramBot.csproj" -c Release -o /app/publish

FROM mcr.microsoft.com/dotnet/aspnet:7.0 AS runtime
WORKDIR /app
COPY --from=publish /app/publish .
ENTRYPOINT ["dotnet", "Tedyes.TelegramBot.dll", "--urls=http://0.0.0.0:4002"]
