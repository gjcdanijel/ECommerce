FROM mcr.microsoft.com/dotnet/aspnet:10.0 AS base
WORKDIR /app
EXPOSE 80

FROM mcr.microsoft.com/dotnet/sdk:10.0 AS build
WORKDIR /src

COPY ["src/Shared/ECommerce.Shared.csproj", "src/Shared/"]
COPY ["src/Services/Payments.API/Payments.API.csproj", "src/Services/Payments.API/"]
RUN dotnet restore "src/Services/Payments.API/Payments.API.csproj"

COPY . .
WORKDIR "/src/src/Services/Payments.API"
RUN dotnet build "Payments.API.csproj" -c Release -o /app/build

FROM build AS publish
RUN dotnet publish "Payments.API.csproj" -c Release -o /app/publish /p:UseAppHost=false

FROM base AS final
WORKDIR /app
COPY --from=publish /app/publish .
ENTRYPOINT ["dotnet", "Payments.API.dll"]
