
# Etapa 1: build
FROM mcr.microsoft.com/dotnet/sdk:8.0 AS build
WORKDIR /src

COPY Pedido/src/Pedido.Api/Pedido.Api.csproj Pedido/src/Pedido.Api/
COPY Pedido/src/Pedido.Application/Pedido.Application.csproj Pedido/src/Pedido.Application/
COPY Pedido/src/Pedido.Domain/Pedido.Domain.csproj Pedido/src/Pedido.Domain/
COPY Pedido/src/Pedido.Infrastructure/Pedido.Infrastructure.csproj Pedido/src/Pedido.Infrastructure/

RUN dotnet restore Pedido/src/Pedido.Api/Pedido.Api.csproj

COPY . .
WORKDIR /src/Pedido/src/Pedido.Api
RUN dotnet publish -c Release -o /app/publish

# Etapa 2: runtime
FROM mcr.microsoft.com/dotnet/aspnet:8.0
WORKDIR /app
COPY --from=build /app/publish .

EXPOSE 8080
ENTRYPOINT ["dotnet", "Pedido.Api.dll"]
