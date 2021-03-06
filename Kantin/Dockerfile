#See https://aka.ms/containerfastmode to understand how Visual Studio uses this Dockerfile to build your images for faster debugging.

FROM mcr.microsoft.com/dotnet/core/aspnet:3.1-buster-slim AS base
WORKDIR /app

FROM mcr.microsoft.com/dotnet/core/sdk:3.1-buster AS build
WORKDIR /src
COPY ["Kantin/Kantin.csproj", "Kantin/"]
COPY ["Kantin.Data/Kantin.Data.csproj", "Kantin.Data/"]
COPY ["Kantin.Service/Kantin.Service.csproj", "Kantin.Service/"]
RUN dotnet restore "Kantin/Kantin.csproj"
COPY . .
WORKDIR "/src/Kantin"
RUN dotnet build "Kantin.csproj" -c Release -o /app/build

FROM build AS publish
RUN dotnet publish "Kantin.csproj" -c Release -o /app/publish

FROM base AS final
WORKDIR /app
COPY --from=publish /app/publish .
ENTRYPOINT ["dotnet", "Kantin.dll"]