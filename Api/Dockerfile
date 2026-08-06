# 1. ÇALIŞTIRMA ORTAMI
FROM mcr.microsoft.com/dotnet/aspnet:10.0 AS base
WORKDIR /app
EXPOSE 8080

# 2. İNŞA ORTAMI
FROM mcr.microsoft.com/dotnet/sdk:10.0 AS build
WORKDIR /src
COPY . .
WORKDIR "/src/Api"
RUN dotnet build "Api.csproj" -c Release -o /app/build

# 3. YAYINLAMA
FROM build AS publish
RUN dotnet publish "Api.csproj" -c Release -o /app/publish

# 4. SON AŞAMA
FROM base AS final
WORKDIR /app
COPY --from=publish /app/publish .
ENTRYPOINT ["dotnet", "Api.dll"]