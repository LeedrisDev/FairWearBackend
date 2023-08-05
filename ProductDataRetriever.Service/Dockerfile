FROM mcr.microsoft.com/dotnet/aspnet:7.0-alpine3.17 AS base
WORKDIR /app
EXPOSE 80
EXPOSE 443

FROM mcr.microsoft.com/dotnet/sdk:7.0 AS build
WORKDIR /src
COPY ["FairWearProductDataRetriever.Service/FairWearProductDataRetriever.Service.csproj", "FairWearProductDataRetriever.Service/"]
RUN dotnet restore "FairWearProductDataRetriever.Service/FairWearProductDataRetriever.Service.csproj"
COPY . .
WORKDIR "/src/FairWearProductDataRetriever.Service"
RUN dotnet build "FairWearProductDataRetriever.Service.csproj" -c Release -o /app/build

FROM build AS publish
RUN dotnet publish "FairWearProductDataRetriever.Service.csproj" -c Release -o /app/publish /p:UseAppHost=false

FROM base AS final
WORKDIR /app
COPY --from=publish /app/publish .
ENTRYPOINT ["dotnet", "FairWearProductDataRetriever.Service.dll"]
