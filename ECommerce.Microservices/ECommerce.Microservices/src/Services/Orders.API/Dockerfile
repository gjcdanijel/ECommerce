FROM mcr.microsoft.com/dotnet/aspnet:10.0 AS base
WORKDIR /app
EXPOSE 80

FROM mcr.microsoft.com/dotnet/sdk:10.0 AS build
WORKDIR /src

COPY ["src/Shared/ECommerce.Shared.csproj", "src/Shared/"]
COPY ["src/Services/Orders.API/Orders.API.csproj", "src/Services/Orders.API/"]
RUN dotnet restore "src/Services/Orders.API/Orders.API.csproj"

COPY . .
WORKDIR "/src/src/Services/Orders.API"
RUN dotnet build "Orders.API.csproj" -c Release -o /app/build

FROM build AS publish
RUN dotnet publish "Orders.API.csproj" -c Release -o /app/publish /p:UseAppHost=false

FROM base AS final
WORKDIR /app
COPY --from=publish /app/publish .
ENTRYPOINT ["dotnet", "Orders.API.dll"]
