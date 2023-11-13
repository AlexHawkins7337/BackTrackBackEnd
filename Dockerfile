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

RUN dotnet tool install --global dotnet-ef
RUN dotnet add package Microsoft.EntityFrameworkCore.Design
ENV PATH="$PATH:/root/.dotnet/tools"

ENV ASPNETCORE_URLS=http://+:80

FROM build AS publish
RUN dotnet publish "BackTrackBackEnd.csproj" -c Release -o /app/publish /p:UseAppHost=false

FROM publish AS final
WORKDIR /app
COPY --from=publish /app/publish .
COPY entrypoint.sh ./
RUN chmod +x ./entrypoint.sh
ENTRYPOINT ["./entrypoint.sh"]
