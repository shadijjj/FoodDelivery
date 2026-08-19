FROM mcr.microsoft.com/dotnet/aspnet:10.0 AS base
WORKDIR /app
EXPOSE 80

FROM mcr.microsoft.com/dotnet/sdk:10.0 AS build
WORKDIR /src
COPY ["src/Services/Delivery/Delivery.API/Delivery.API.csproj", "src/Services/Delivery/Delivery.API/"]
COPY ["src/Services/Delivery/Delivery.Application/Delivery.Application.csproj", "src/Services/Delivery/Delivery.Application/"]
COPY ["src/Services/Delivery/Delivery.Domain/Delivery.Domain.csproj", "src/Services/Delivery/Delivery.Domain/"]
COPY ["src/Services/Delivery/Delivery.Infrastructure/Delivery.Infrastructure.csproj", "src/Services/Delivery/Delivery.Infrastructure/"]
COPY ["src/Shared/FoodDelivery.Shared.Contracts/FoodDelivery.Shared.Contracts.csproj", "src/Shared/FoodDelivery.Shared.Contracts/"]
RUN dotnet restore "src/Services/Delivery/Delivery.API/Delivery.API.csproj"
COPY . .
WORKDIR "/src/src/Services/Delivery/Delivery.API"
RUN dotnet build "Delivery.API.csproj" -c Release -o /app/build

FROM build AS publish
RUN dotnet publish "Delivery.API.csproj" -c Release -o /app/publish /p:UseAppHost=false

FROM base AS final
WORKDIR /app
COPY --from=publish /app/publish .
ENTRYPOINT ["dotnet", "Delivery.API.dll"]