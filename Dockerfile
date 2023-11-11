FROM mcr.microsoft.com/dotnet/aspnet:7.0 AS base
WORKDIR /app
EXPOSE 80
EXPOSE 443

FROM mcr.microsoft.com/dotnet/sdk:7.0 AS build
WORKDIR /src
COPY ["BackTrackBackEnd/BackTrackBackEnd.csproj", "BackTrackBackEnd/"]
RUN dotnet restore "BackTrackBackEnd/BackTrackBackEnd.csproj"
COPY . .
WORKDIR "/src/BackTrackBackEnd"
RUN dotnet build "BackTrackBackEnd.csproj" -c Release -o /app/build

FROM build AS publish
RUN dotnet publish "BackTrackBackEnd.csproj" -c Release -o /app/publish /p:UseAppHost=false

FROM base AS final
WORKDIR /app
COPY --from=publish /app/publish .
ENTRYPOINT ["dotnet", "BackTrackBackEnd.dll"]
