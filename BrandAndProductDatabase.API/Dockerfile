FROM mcr.microsoft.com/dotnet/aspnet:7.0 AS base
WORKDIR /app
EXPOSE 80
EXPOSE 443

FROM mcr.microsoft.com/dotnet/sdk:7.0 AS build
WORKDIR /src
COPY ["BrandAndProductDatabase.API/BrandAndProductDatabase.API.csproj", "BrandAndProductDatabase.API/"]
RUN dotnet restore "BrandAndProductDatabase.API/BrandAndProductDatabase.API.csproj"
COPY . .
WORKDIR "/src/BrandAndProductDatabase.API"
RUN dotnet build "BrandAndProductDatabase.API.csproj" -c Release -o /app/build

FROM build AS publish
RUN dotnet publish "BrandAndProductDatabase.API.csproj" -c Release -o /app/publish /p:UseAppHost=false

FROM base AS final
WORKDIR /app
COPY --from=publish /app/publish .
ENTRYPOINT ["dotnet", "BrandAndProductDatabase.API.dll"]
