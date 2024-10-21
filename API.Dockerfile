FROM mcr.microsoft.com/dotnet/sdk:7.0 AS build
WORKDIR /src
COPY . .
RUN dotnet restore "src/Teydes.Api/Teydes.Api.csproj"
WORKDIR "/src/src/Teydes.Api"
RUN dotnet build "Teydes.Api.csproj" -c Release -o /app/build

FROM build AS publish
RUN dotnet publish "Teydes.Api.csproj" -c Release -o /app/publish

FROM mcr.microsoft.com/dotnet/aspnet:7.0 AS runtime
WORKDIR /app
COPY --from=publish /app/publish .
ENTRYPOINT ["dotnet", "Teydes.Api.dll", "--urls=http://0.0.0.0:4001"]
