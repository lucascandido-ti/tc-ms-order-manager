FROM mcr.microsoft.com/dotnet/sdk:7.0 AS build-env
WORKDIR /app
EXPOSE 5000

# Copy everything
COPY . ./
# Restore as distinct layers
WORKDIR /app/Consumers/API
RUN dotnet restore
# Build and publish a release
RUN dotnet publish -c Release -o out

# Build runtime image
FROM mcr.microsoft.com/dotnet/sdk:7.0 as final-env
WORKDIR /app/Consumers/API
COPY --from=build-env /app/Consumers/API/out .
EXPOSE 5000
ENTRYPOINT ["dotnet", "API.dll"]