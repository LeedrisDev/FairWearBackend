FROM mcr.microsoft.com/dotnet/aspnet:7.0-alpine3.18 AS base
WORKDIR /app
EXPOSE 80
EXPOSE 443

FROM mcr.microsoft.com/dotnet/sdk:7.0 AS build
WORKDIR /src
COPY ["Users.Service/Users.Service.csproj", "Users.Service/"]
RUN dotnet restore "Users.Service/Users.Service.csproj"
COPY . .
WORKDIR "/src/Users.Service"
RUN dotnet build "Users.Service.csproj" -c Release -o /app/build

FROM build AS publish
RUN dotnet publish "Users.Service.csproj" -c Release -o /app/publish /p:UseAppHost=false

FROM base AS final
WORKDIR /app
COPY --from=publish /app/publish .
ENTRYPOINT ["dotnet", "Users.Service.dll"]