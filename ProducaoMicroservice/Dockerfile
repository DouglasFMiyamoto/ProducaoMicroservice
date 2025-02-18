FROM mcr.microsoft.com/dotnet/aspnet:8.0 AS base
WORKDIR /app
EXPOSE 8080

# Definir a imagem base para build
FROM mcr.microsoft.com/dotnet/sdk:8.0 AS build
WORKDIR /src

COPY ["ProducaoMicroservice.csproj", "."]
RUN dotnet restore "./ProducaoMicroservice.csproj"

COPY . .

WORKDIR "/src/."
RUN dotnet build "ProducaoMicroservice.csproj" -c $BUILD_CONFIGURATION -o /app/build

FROM build AS publish
RUN dotnet publish "ProducaoMicroservice.csproj" -c Release -o /app/publish

FROM mcr.microsoft.com/dotnet/sdk:8.0 AS final
WORKDIR /app
COPY --from=publish /app/publish .

# Definir variável de ambiente para o LocalStack
ENV AWS_SERVICE_URL=http://localstack:4566

ENTRYPOINT ["dotnet", "ProducaoMicroservice.dll"]