FROM mcr.microsoft.com/dotnet/core/aspnet:3.1-buster-slim AS base
WORKDIR /app

FROM mcr.microsoft.com/dotnet/core/sdk:3.1-buster AS build
WORKDIR /src
COPY ["CashManager.csproj", "./"]
RUN dotnet restore "./CashManager.csproj"
COPY . .
WORKDIR "/src/."
RUN dotnet build "CashManager.csproj" -c Release -o /app/build

FROM build AS publish
RUN dotnet publish "CashManager.csproj" -c Release -o /app/publish

FROM base AS final
WORKDIR /app
COPY --from=publish /app/publish .
ENV ASPNETCORE_URLS http://*:5000
ENTRYPOINT ["dotnet", "CashManager.dll"]