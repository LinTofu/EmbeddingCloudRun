# ============================
# Build Stage
# ============================
FROM mcr.microsoft.com/dotnet/sdk:10.0-preview AS build

WORKDIR /src

# 複製 csproj
COPY EmbeddingCloudRun.csproj .

# 還原 NuGet
RUN dotnet restore

# 複製所有程式碼
COPY . .

# Publish
RUN dotnet publish \
    -c Release \
    -o /app/publish \
    --no-restore

# ============================
# Runtime Stage
# ============================
FROM mcr.microsoft.com/dotnet/aspnet:10.0-preview

WORKDIR /app

COPY --from=build /app/publish .

# Cloud Run 使用 8080
ENV ASPNETCORE_URLS=http://+:8080

EXPOSE 8080

ENTRYPOINT ["dotnet", "EmbeddingCloudRun.dll"]