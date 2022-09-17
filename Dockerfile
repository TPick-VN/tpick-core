FROM mcr.microsoft.com/dotnet/aspnet:6.0 AS base
WORKDIR /app
EXPOSE 80
EXPOSE 443

FROM mcr.microsoft.com/dotnet/sdk:6.0 AS build
WORKDIR /src
COPY ["TPick/TPick.Api/TPick.Api.csproj", "TPick/TPick.Api/"]
COPY ["CsMicro/", "CsMicro/"]
RUN dotnet restore "TPick/TPick.Api/TPick.Api.csproj"
COPY . .
WORKDIR "/src/TPick/TPick.Api"
RUN dotnet build "TPick.Api.csproj" -c Release -o /app/build

FROM build AS publish
RUN dotnet publish "TPick.Api.csproj" -c Release -o /app/publish

FROM base AS final
WORKDIR /app
COPY --from=publish /app/publish .
ENTRYPOINT ["dotnet", "TPick.Api.dll"]
