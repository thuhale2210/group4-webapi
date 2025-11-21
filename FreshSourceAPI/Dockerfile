# === 1. Base image for running the app (runtime only) ===
FROM mcr.microsoft.com/dotnet/aspnet:8.0 AS base
WORKDIR /app
EXPOSE 8080

# === 2. Build image (SDK) ===
FROM mcr.microsoft.com/dotnet/sdk:8.0 AS build
WORKDIR /src

# IMPORTANT: replace FreshSourceAPI.csproj with your actual .csproj file name
COPY ["FreshSourceAPI.csproj", "./"]
RUN dotnet restore "FreshSourceAPI.csproj"

# Copy the rest of the code and build
COPY . .
RUN dotnet build "FreshSourceAPI.csproj" -c Release -o /app/build

# === 3. Publish the app ===
FROM build AS publish
RUN dotnet publish "FreshSourceAPI.csproj" -c Release -o /app/publish /p:UseAppHost=false

# === 4. Final image ===
FROM base AS final
WORKDIR /app
COPY --from=publish /app/publish .

# Kestrel will listen on port 8080 inside the container
ENV ASPNETCORE_URLS=http://+:8080

# IMPORTANT: replace FreshSourceAPI.dll with your actual DLL name
ENTRYPOINT ["dotnet", "FreshSourceAPI.dll"]
