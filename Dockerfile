FROM mcr.microsoft.com/dotnet/aspnet:9.0 AS base
WORKDIR /app
EXPOSE 8080

FROM mcr.microsoft.com/dotnet/sdk:9.0 AS build
WORKDIR /src
COPY ["src/PeoplePortal.Api/PeoplePortal.Api.csproj", "src/PeoplePortal.Api/"]
COPY ["src/PeoplePortal.Application/PeoplePortal.Application.csproj", "src/PeoplePortal.Application/"]
COPY ["src/PeoplePortal.Domain/PeoplePortal.Domain.csproj", "src/PeoplePortal.Domain/"]
COPY ["src/PeoplePortal.Infrastructure/PeoplePortal.Infrastructure.csproj", "src/PeoplePortal.Infrastructure/"]
RUN dotnet restore "src/PeoplePortal.Api/PeoplePortal.Api.csproj"
COPY . .
RUN dotnet build "src/PeoplePortal.Api/PeoplePortal.Api.csproj" -c Release -o /app/build

FROM build AS publish
RUN dotnet publish "src/PeoplePortal.Api/PeoplePortal.Api.csproj" -c Release -o /app/publish /p:UseAppHost=false

FROM build AS migrations
RUN dotnet tool install --global dotnet-ef --version 9.0.8
ENTRYPOINT ["sh", "-c", "sleep ${MIGRATION_START_DELAY:-20} && /root/.dotnet/tools/dotnet-ef database update --project src/PeoplePortal.Infrastructure/PeoplePortal.Infrastructure.csproj --startup-project src/PeoplePortal.Api/PeoplePortal.Api.csproj"]

FROM base AS final
WORKDIR /app
COPY --from=publish /app/publish .
ENTRYPOINT ["dotnet", "PeoplePortal.Api.dll"]
