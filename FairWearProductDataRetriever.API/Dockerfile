FROM mcr.microsoft.com/dotnet/aspnet:7.0 AS base
WORKDIR /app
EXPOSE 80
EXPOSE 443

FROM mcr.microsoft.com/dotnet/sdk:7.0 AS build
WORKDIR /src
COPY ["FairWearProductDataRetriever.API/FairWearProductDataRetriever.API.csproj", "FairWearProductDataRetriever.API/"]
RUN dotnet restore "FairWearProductDataRetriever.API/FairWearProductDataRetriever.API.csproj"
COPY . .
WORKDIR "/src/FairWearProductDataRetriever.API"
RUN dotnet build "FairWearProductDataRetriever.API.csproj" -c Release -o /app/build

FROM build AS publish
RUN dotnet publish "FairWearProductDataRetriever.API.csproj" -c Release -o /app/publish /p:UseAppHost=false

FROM base AS final
WORKDIR /app
COPY --from=publish /app/publish .
ENTRYPOINT ["dotnet", "FairWearProductDataRetriever.API.dll"]
