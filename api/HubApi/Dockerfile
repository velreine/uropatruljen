FROM mcr.microsoft.com/dotnet/aspnet:6.0 AS base
WORKDIR /app
EXPOSE 80
EXPOSE 443

FROM mcr.microsoft.com/dotnet/sdk:6.0 AS build
WORKDIR /src
COPY ["HubApi/HubApi.csproj", "HubApi/"]
RUN dotnet restore "HubApi/HubApi.csproj"
COPY . .
WORKDIR "/src/HubApi"
RUN dotnet build "HubApi.csproj" -c Release -o /app/build

FROM build AS publish
RUN dotnet publish "HubApi.csproj" -c Release -o /app/publish

FROM base AS final
WORKDIR /app
COPY --from=publish /app/publish .
ENTRYPOINT ["dotnet", "HubApi.dll"]
