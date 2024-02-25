FROM mcr.microsoft.com/dotnet/aspnet:8.0 AS base
WORKDIR /app

FROM mcr.microsoft.com/dotnet/sdk:8.0 AS build
WORKDIR /src
COPY . .
RUN dotnet restore "Adapters/Driving/Apis/ProjectK.Api/ProjectK.Api.csproj"

COPY . .

WORKDIR "/src/Adapters/Driving/Apis/ProjectK.Api"
RUN dotnet build "ProjectK.Api.csproj" -c Release -o /app/build

FROM build AS publish
RUN dotnet publish "ProjectK.Api.csproj" -c Release -o /app/publish /p:UseAppHost=false

FROM base AS final
WORKDIR /app
COPY --from=publish /app/publish .
CMD ASPNETCORE_URLS=http://*:$PORT dotnet ProjectK.Api.dll