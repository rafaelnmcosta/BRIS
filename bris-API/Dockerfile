# Estágio 1: Build
FROM mcr.microsoft.com/dotnet/sdk:8.0 AS build
WORKDIR /src
COPY ["bris-API.csproj", "./"]
RUN dotnet restore "./bris-API.csproj"

# Instalar o dotnet-ef CLI globalmente
RUN dotnet tool install --global dotnet-ef

# Adicionar o caminho do dotnet-ef ao PATH
ENV PATH="$PATH:/root/.dotnet/tools"

COPY . .
WORKDIR "/src"
RUN dotnet build "bris-API.csproj" -c Release -o /app/build
RUN dotnet publish "bris-API.csproj" -c Release -o /app/publish

# Estágio 2: Runtime
FROM mcr.microsoft.com/dotnet/aspnet:8.0 AS runtime
WORKDIR /app
COPY --from=build /app/publish .
ENTRYPOINT dotnet bris-API.dll --urls "http://0.0.0.0:80"
