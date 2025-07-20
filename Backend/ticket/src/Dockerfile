FROM mcr.microsoft.com/dotnet/aspnet:8.0 AS base
WORKDIR /app

ENV ASPNETCORE_HTTP_PORTS 80

FROM mcr.microsoft.com/dotnet/sdk:8.0 AS build
WORKDIR /src
COPY ["Ticketing.API/Ticketing.API.csproj", "Ticketing.API/"]
COPY ["Ticketing.Domain/Ticketing.Domain.csproj", "Ticketing.Domain/"]
COPY ["Ticketing.Application/Ticketing.Application.csproj", "Ticketing.Application/"]
COPY ["Ticketing.Infrastructure/Ticketing.Infrastructure.csproj", "Ticketing.Infrastructure/"]

ENV DOTNET_SYSTEM_NET_HTTP_USESOCKETSHTTPHANDLER=0

RUN dotnet restore "Ticketing.Domain/Ticketing.Domain.csproj" -s "https://api.nuget.org/v3/index.json" 
RUN dotnet restore "Ticketing.API/Ticketing.API.csproj" -s "https://api.nuget.org/v3/index.json"
RUN dotnet restore "Ticketing.Application/Ticketing.Application.csproj" -s "https://api.nuget.org/v3/index.json" 
RUN dotnet restore "Ticketing.Infrastructure/Ticketing.Infrastructure.csproj" -s "https://api.nuget.org/v3/index.json" 

COPY . .

WORKDIR "/src/Ticketing.API"
RUN dotnet build "Ticketing.API.csproj" -c Release -o /app/build

FROM build AS publish
RUN dotnet publish "Ticketing.API.csproj" -c Release -o /app/publish

FROM base AS final
WORKDIR /app
COPY --from=publish /app/publish .
ENTRYPOINT ["dotnet", "Ticketing.API.dll"]