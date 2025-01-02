# Use .NET SDK image
FROM mcr.microsoft.com/dotnet/sdk:7.0 AS build

# Set the working directory
WORKDIR /app

# Copy project files
COPY [MongoExample.csproj, ./]

# Restore dependencies for the specified project
RUN dotnet restore ./MongoExample.csproj

# Copy the rest of the application files
COPY . .

# Build the project
RUN dotnet build ./MongoExample.csproj -c Release -o /app/build

# Publish the application
RUN dotnet publish ./MongoExample.csproj -c Release -o /app/publish

# Use a smaller runtime image for production
FROM mcr.microsoft.com/dotnet/aspnet:7.0 AS runtime
WORKDIR /app
COPY --from=build /app/publish .

# Run the application
ENTRYPOINT ["dotnet", "MongoExample.dll"]
