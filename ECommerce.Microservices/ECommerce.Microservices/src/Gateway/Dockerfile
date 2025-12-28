FROM mcr.microsoft.com/dotnet/aspnet:10.0 AS base
WORKDIR /app
EXPOSE 80

FROM mcr.microsoft.com/dotnet/sdk:10.0 AS build
WORKDIR /src

COPY ["src/Gateway/ECommerce.Gateway.csproj", "src/Gateway/"]
RUN dotnet restore "src/Gateway/ECommerce.Gateway.csproj"

COPY . .
WORKDIR "/src/src/Gateway"
RUN dotnet build "ECommerce.Gateway.csproj" -c Release -o /app/build

FROM build AS publish
RUN dotnet publish "ECommerce.Gateway.csproj" -c Release -o /app/publish /p:UseAppHost=false

FROM base AS final
WORKDIR /app
COPY --from=publish /app/publish .
ENTRYPOINT ["dotnet", "ECommerce.Gateway.dll"]
