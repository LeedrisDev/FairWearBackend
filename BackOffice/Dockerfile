﻿FROM mcr.microsoft.com/dotnet/aspnet:7.0-alpine3.18 AS base
WORKDIR /app
EXPOSE 80
EXPOSE 443

FROM mcr.microsoft.com/dotnet/sdk:7.0 AS build
WORKDIR /src
COPY ["BackOffice/BackOffice.csproj", "BackOffice/"]
RUN dotnet restore "BackOffice/BackOffice.csproj"
COPY . .
WORKDIR "/src/BackOffice"
RUN dotnet build "BackOffice.csproj" -c Release -o /app/build

FROM build AS publish
RUN dotnet publish "BackOffice.csproj" -c Release -o /app/publish /p:UseAppHost=false

FROM base AS final
WORKDIR /app
COPY --from=publish /app/publish .
ENTRYPOINT ["dotnet", "BackOffice.dll"]
