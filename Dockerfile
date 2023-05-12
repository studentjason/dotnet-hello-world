# Start with the .NET Core 3.1 SDK image
FROM mcr.microsoft.com/dotnet/sdk:3.1 AS build

# Set the working directory to /app
WORKDIR /app

# Copy the .csproj file to the working directory
COPY *.csproj ./

# Restore any dependencies
RUN dotnet restore

# Copy the rest of the application code to the working directory
COPY . ./

# Build the application
RUN dotnet publish -c Release -o out

# Start with a new image
FROM mcr.microsoft.com/dotnet/aspnet:3.1 AS runtime

# Set the working directory to /app
WORKDIR /app

# Copy the published application from the build image
COPY --from=build /app/out ./

# Expose port 80 for the application
EXPOSE 80

# Start the application
ENTRYPOINT ["dotnet", "workspace.dll"]