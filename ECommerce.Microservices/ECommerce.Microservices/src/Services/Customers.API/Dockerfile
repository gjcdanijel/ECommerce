FROM mcr.microsoft.com/dotnet/aspnet:10.0 AS base
WORKDIR /app
EXPOSE 80

FROM mcr.microsoft.com/dotnet/sdk:10.0 AS build
WORKDIR /src

COPY ["src/Shared/ECommerce.Shared.csproj", "src/Shared/"]
COPY ["src/Services/Customers.API/Customers.API.csproj", "src/Services/Customers.API/"]
RUN dotnet restore "src/Services/Customers.API/Customers.API.csproj"

COPY . .
WORKDIR "/src/src/Services/Customers.API"
RUN dotnet build "Customers.API.csproj" -c Release -o /app/build

FROM build AS publish
RUN dotnet publish "Customers.API.csproj" -c Release -o /app/publish /p:UseAppHost=false

FROM base AS final
WORKDIR /app
COPY --from=publish /app/publish .
ENTRYPOINT ["dotnet", "Customers.API.dll"]
