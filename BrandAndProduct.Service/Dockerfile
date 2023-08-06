FROM mcr.microsoft.com/dotnet/aspnet:7.0-alpine3.18 AS base
WORKDIR /app
EXPOSE 80
EXPOSE 443

FROM mcr.microsoft.com/dotnet/sdk:7.0 AS build
WORKDIR /src
COPY ["BrandAndProduct.Service/BrandAndProduct.Service.csproj", "BrandAndProduct.Service/"]
RUN dotnet restore "BrandAndProduct.Service/BrandAndProduct.Service.csproj"
COPY . .
WORKDIR "/src/BrandAndProduct.Service"
RUN dotnet build "BrandAndProduct.Service.csproj" -c Release -o /app/build

FROM build AS publish
RUN dotnet publish "BrandAndProduct.Service.csproj" -c Release -o /app/publish /p:UseAppHost=false

FROM base AS final
WORKDIR /app
COPY --from=publish /app/publish .
ENTRYPOINT ["dotnet", "BrandAndProduct.Service.dll"]
