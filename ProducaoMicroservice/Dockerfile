FROM mcr.microsoft.com/dotnet/sdk:8.0 AS base
WORKDIR /app
EXPOSE 8080

# Definir a imagem base para build
FROM mcr.microsoft.com/dotnet/sdk:8.0 AS build
WORKDIR /src

# Copiar apenas o .csproj do projeto principal
COPY ["ProducaoMicroservice/ProducaoMicroservice.csproj", "./"]

# Restaurar dependências do projeto principal
RUN dotnet restore "ProducaoMicroservice.csproj"

# Copiar apenas os arquivos do projeto principal (excluindo a pasta de testes)
COPY ProducaoMicroservice/. . 

WORKDIR "/src"
RUN dotnet build "ProducaoMicroservice.csproj" -c Release -o /app/build

FROM build AS publish
RUN dotnet publish "ProducaoMicroservice.csproj" -c Release -o /app/publish

FROM mcr.microsoft.com/dotnet/sdk:8.0 AS final
WORKDIR /app

# Copiar o conteúdo do publish para o container final
COPY --from=publish /app/publish .

# Definir variável de ambiente para o LocalStack
ENV AWS_SERVICE_URL=http://localstack:4566

ENTRYPOINT ["dotnet", "ProducaoMicroservice.dll"]
