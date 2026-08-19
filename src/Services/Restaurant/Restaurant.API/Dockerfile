FROM mcr.microsoft.com/dotnet/aspnet:10.0 AS base
WORKDIR /app
EXPOSE 80

FROM mcr.microsoft.com/dotnet/sdk:10.0 AS build
WORKDIR /src
COPY ["src/Services/Restaurant/Restaurant.API/Restaurant.API.csproj", "src/Services/Restaurant/Restaurant.API/"]
COPY ["src/Services/Restaurant/Restaurant.Application/Restaurant.Application.csproj", "src/Services/Restaurant/Restaurant.Application/"]
COPY ["src/Services/Restaurant/Restaurant.Domain/Restaurant.Domain.csproj", "src/Services/Restaurant/Restaurant.Domain/"]
COPY ["src/Services/Restaurant/Restaurant.Infrastructure/Restaurant.Infrastructure.csproj", "src/Services/Restaurant/Restaurant.Infrastructure/"]
RUN dotnet restore "src/Services/Restaurant/Restaurant.API/Restaurant.API.csproj"
COPY . .
WORKDIR "/src/src/Services/Restaurant/Restaurant.API"
RUN dotnet build "Restaurant.API.csproj" -c Release -o /app/build

FROM build AS publish
RUN dotnet publish "Restaurant.API.csproj" -c Release -o /app/publish /p:UseAppHost=false

FROM base AS final
WORKDIR /app
COPY --from=publish /app/publish .
ENTRYPOINT ["dotnet", "Restaurant.API.dll"]