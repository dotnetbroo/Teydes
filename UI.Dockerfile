FROM mcr.microsoft.com/dotnet/sdk:7.0 AS build
WORKDIR /src
COPY . .

RUN dotnet restore "Teydes.Web/Teydes.Web.csproj"
WORKDIR "/src/Teydes.Web"
RUN dotnet build "Teydes.Web.csproj" -c Release -o /app/build

FROM build AS publish
RUN dotnet publish "Teydes.Web.csproj" -c Release -o /app/publish

FROM mcr.microsoft.com/dotnet/aspnet:7.0 AS runtime
WORKDIR /app
COPY --from=publish /app/publish .
ENTRYPOINT ["dotnet", "Teydes.Web.dll", "--urls=http://0.0.0.0:4000"]
