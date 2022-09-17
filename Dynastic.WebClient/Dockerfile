FROM mcr.microsoft.com/dotnet/aspnet:6.0 AS base
WORKDIR /app
EXPOSE 80
EXPOSE 443

FROM mcr.microsoft.com/dotnet/sdk:6.0 AS build
WORKDIR /src
COPY ["Dynastic.WebClient/Dynastic.WebClient.csproj", "Dynastic.WebClient/"]
RUN dotnet restore "Dynastic.WebClient/Dynastic.WebClient.csproj"
COPY . .
WORKDIR "/src/Dynastic.WebClient"
RUN dotnet build "Dynastic.WebClient.csproj" -c Release -o /app/build

FROM build AS publish
RUN dotnet publish "Dynastic.WebClient.csproj" -c Release -o /app/publish

FROM base AS final
WORKDIR /app
COPY --from=publish /app/publish .
ENTRYPOINT ["dotnet", "Dynastic.WebClient.dll"]
