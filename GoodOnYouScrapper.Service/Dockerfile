FROM mcr.microsoft.com/dotnet/aspnet:7.0-alpine3.18 AS base
WORKDIR /app
EXPOSE 80
EXPOSE 443

FROM mcr.microsoft.com/dotnet/sdk:7.0 AS build
WORKDIR /src
COPY ["GoodOnYouScrapper.Service/GoodOnYouScrapper.Service.csproj", "GoodOnYouScrapper.Service/"]
RUN dotnet restore "GoodOnYouScrapper.Service/GoodOnYouScrapper.Service.csproj"
COPY . .
WORKDIR "/src/GoodOnYouScrapper.Service"
RUN dotnet build "GoodOnYouScrapper.Service.csproj" -c Release -o /app/build

FROM build AS publish
RUN dotnet publish "GoodOnYouScrapper.Service.csproj" -c Release -o /app/publish /p:UseAppHost=false

FROM base AS final
WORKDIR /app
COPY --from=publish /app/publish .
ENTRYPOINT ["dotnet", "GoodOnYouScrapper.Service.dll"]
