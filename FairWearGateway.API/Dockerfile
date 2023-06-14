FROM mcr.microsoft.com/dotnet/aspnet:7.0-alpine3.17 AS base
WORKDIR /app
EXPOSE 80
EXPOSE 443

FROM mcr.microsoft.com/dotnet/sdk:7.0 AS build
WORKDIR /src
COPY ["FairWearGateway.API/FairWearGateway.API.csproj", "FairWearGateway.API/"]
RUN dotnet restore "FairWearGateway.API/FairWearGateway.API.csproj"
COPY . .
WORKDIR "/src/FairWearGateway.API"
RUN dotnet build "FairWearGateway.API.csproj" -c Release -o /app/build

FROM build AS publish
RUN dotnet publish "FairWearGateway.API.csproj" -c Release -o /app/publish

FROM base AS final
WORKDIR /app
COPY --from=publish /app/publish .
ENTRYPOINT ["dotnet", "FairWearGateway.API.dll"]
