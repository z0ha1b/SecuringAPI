FROM mcr.microsoft.com/dotnet/runtime:7.0 AS base
WORKDIR /app

FROM mcr.microsoft.com/dotnet/sdk:7.0 AS build
WORKDIR /src
COPY ["src/services/Secure.API.Client/Secure.API.Client.csproj", "src/services/Secure.API.Client/"]
RUN dotnet restore "src/services/Secure.API.Client/Secure.API.Client.csproj"
COPY . .
WORKDIR "/src/src/services/Secure.API.Client"
RUN dotnet build "Secure.API.Client.csproj" -c Release -o /app/build

FROM build AS publish
RUN dotnet publish "Secure.API.Client.csproj" -c Release -o /app/publish /p:UseAppHost=false

FROM base AS final
WORKDIR /app
COPY --from=publish /app/publish .
ENTRYPOINT ["dotnet", "Secure.API.Client.dll"]
