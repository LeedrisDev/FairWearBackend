FROM mcr.microsoft.com/dotnet/aspnet:7.0-alpine3.18 AS base
WORKDIR /app
EXPOSE 80
EXPOSE 443

FROM mcr.microsoft.com/dotnet/sdk:7.0 AS build
WORKDIR /src
COPY ["ProductDataRetriever.Service/ProductDataRetriever.Service.csproj", "ProductDataRetriever.Service/"]
RUN dotnet restore "ProductDataRetriever.Service/ProductDataRetriever.Service.csproj"
COPY . .
WORKDIR "/src/ProductDataRetriever.Service"
RUN dotnet build "ProductDataRetriever.Service.csproj" -c Release -o /app/build

FROM build AS publish
RUN dotnet publish "ProductDataRetriever.Service.csproj" -c Release -o /app/publish /p:UseAppHost=false

FROM base AS final
WORKDIR /app
COPY --from=publish /app/publish .
ENTRYPOINT ["dotnet", "ProductDataRetriever.Service.dll"]
