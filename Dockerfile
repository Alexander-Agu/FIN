FROM mcr.microsoft.com/dotnet/sdk:9.0 AS build

WORKDIR /src


# Restore Stage
COPY ["FIN/FIN.csproj", "FIN/"]
RUN dotnet restore "FIN/FIN.csproj"


# Build Stage
COPY ["FIN/", "FIN/"]
WORKDIR /src/FIN
RUN dotnet build "FIN.csproj" -c Release -o app/build


# Publish Stage
FROM build AS publish
RUN dotnet publish "FIN.csproj" -c Release -o /app/publish


# Run Stage
FROM mcr.microsoft.com/dotnet/aspnet:9.0
ENV ASPNETCORE_HTTP_PORTS=5000
WORKDIR /app
COPY --from=publish /app/publish .
EXPOSE 5000
ENTRYPOINT ["dotnet", "FIN.dll"]