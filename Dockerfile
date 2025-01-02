# Stage 1: Build the application
FROM mcr.microsoft.com/dotnet/sdk:7.0 AS build
WORKDIR /app

# Copy project files and restore dependencies
COPY . ./
RUN dotnet restore

# Publish the application
RUN dotnet publish -c Release -o /app/out

# Stage 2: Serve the application
FROM mcr.microsoft.com/dotnet/aspnet:7.0
WORKDIR /app
COPY --from=build /app/out .

# Expose port 80
EXPOSE 80

# Command to run the application
ENTRYPOINT ["dotnet", "MongoExample.dll"]
