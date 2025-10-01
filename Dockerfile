FROM mcr.microsoft.com/dotnet/aspnet:8.0 AS base
WORKDIR /app
EXPOSE 8080
EXPOSE 8081

FROM mcr.microsoft.com/dotnet/sdk:8.0 AS build
ARG BUILD_CONFIGURATION=Release
WORKDIR /src

COPY ["okenovTest.csproj", "./"]
RUN dotnet restore "./okenovTest.csproj"

COPY . .

RUN dotnet build "./okenovTest.csproj" -c $BUILD_CONFIGURATION -o /app/build
RUN dotnet publish "./okenovTest.csproj" -c $BUILD_CONFIGURATION -o /app/publish /p:UseAppHost=false

FROM base AS final
WORKDIR /app
COPY --from=build /app/publish .
ENTRYPOINT ["dotnet", "okenovTest.dll"]
