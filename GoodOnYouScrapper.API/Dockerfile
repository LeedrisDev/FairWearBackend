FROM mcr.microsoft.com/dotnet/aspnet:7.0-alpine3.17 AS base
WORKDIR /app
EXPOSE 80
EXPOSE 443

FROM mcr.microsoft.com/dotnet/sdk:7.0 AS build
WORKDIR /src
COPY ["GoodOnYouScrapper.API/GoodOnYouScrapper.API.csproj", "GoodOnYouScrapper.API/"]
RUN dotnet restore "GoodOnYouScrapper.API/GoodOnYouScrapper.API.csproj"
COPY . .
WORKDIR "/src/GoodOnYouScrapper.API"
RUN dotnet build "GoodOnYouScrapper.API.csproj" -c Release -o /app/build

FROM build AS publish
RUN dotnet publish "GoodOnYouScrapper.API.csproj" -c Release -o /app/publish

FROM base AS final
WORKDIR /app
COPY --from=publish /app/publish .
ENTRYPOINT ["dotnet", "GoodOnYouScrapper.API.dll"]
