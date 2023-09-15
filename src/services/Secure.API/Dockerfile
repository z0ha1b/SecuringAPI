FROM mcr.microsoft.com/dotnet/aspnet:7.0 AS base
WORKDIR /app
EXPOSE 80
EXPOSE 443

FROM mcr.microsoft.com/dotnet/sdk:7.0 AS build
WORKDIR /src
COPY ["src/services/Secure.API/Secure.API.csproj", "src/services/Secure.API/"]
RUN dotnet restore "src/services/Secure.API/Secure.API.csproj"
COPY . .
WORKDIR "/src/src/services/Secure.API"
RUN dotnet build "Secure.API.csproj" -c Release -o /app/build

FROM build AS publish
RUN dotnet publish "Secure.API.csproj" -c Release -o /app/publish /p:UseAppHost=false

FROM base AS final
WORKDIR /app
COPY --from=publish /app/publish .
ENTRYPOINT ["dotnet", "Secure.API.dll"]
