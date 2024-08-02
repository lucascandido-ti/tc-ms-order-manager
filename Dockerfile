# Etapa de build
FROM mcr.microsoft.com/dotnet/sdk:7.0 AS build-env
WORKDIR /app

# Copiar tudo
COPY . ./
# Restaurar dependências
WORKDIR /app/Consumers/API
RUN dotnet restore

# Build e publicar uma versão de release
RUN dotnet publish -c Release -o out

# Imagem de runtime
FROM mcr.microsoft.com/dotnet/aspnet:7.0 AS runtime
WORKDIR /app/Consumers/API
COPY --from=build-env /app/Consumers/API/out .
EXPOSE 5000
ENTRYPOINT ["dotnet", "API.dll"]