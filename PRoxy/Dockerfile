FROM mcr.microsoft.com/dotnet/aspnet:6.0 AS base
WORKDIR /app
EXPOSE 80
EXPOSE 443

FROM mcr.microsoft.com/dotnet/sdk:6.0 AS build
WORKDIR /src
COPY ["PRoxy/PRoxy.csproj", "PRoxy/"]
RUN dotnet restore "PRoxy/PRoxy.csproj"
COPY . .
WORKDIR "/src/PRoxy"
RUN dotnet build "PRoxy.csproj" -c Release -o /app/build

FROM build AS publish
RUN dotnet publish "PRoxy.csproj" -c Release -o /app/publish /p:UseAppHost=false

FROM base AS final
WORKDIR /app
COPY --from=publish /app/publish .
ENTRYPOINT ["dotnet", "PRoxy.dll"]