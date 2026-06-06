# Use SDK image to build the app
FROM mcr.microsoft.com/dotnet/sdk:10.0 AS build
WORKDIR /app

# Copy solution and csproj files
COPY danialNewsNetX.sln ./
COPY src/Domain/*.csproj ./src/Domain/
COPY src/Application/*.csproj ./src/Application/
COPY src/Infrastructure/*.csproj ./src/Infrastructure/
COPY src/WebUI/*.csproj ./src/WebUI/

# Restore dependencies
RUN dotnet restore

# Copy the rest of the code
COPY . .

# Build and publish
RUN dotnet publish src/WebUI/danialNewsNetX.WebUI.csproj -c Release -o out

# Use runtime image
FROM mcr.microsoft.com/dotnet/aspnet:10.0 AS runtime
WORKDIR /app
COPY --from=build /app/out .

ENTRYPOINT ["dotnet", "danialNewsNetX.WebUI.dll"]
