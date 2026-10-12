# ---------- Build ----------
FROM mcr.microsoft.com/dotnet/sdk:10.0 AS build
WORKDIR /src

# Restaura dependências primeiro para aproveitar o cache de camadas.
COPY src/Pedidos.Api/Pedidos.Api.csproj src/Pedidos.Api/
RUN dotnet restore src/Pedidos.Api/Pedidos.Api.csproj

# Copia o restante do código e publica em modo Release.
COPY src/ src/
RUN dotnet publish src/Pedidos.Api/Pedidos.Api.csproj \
    -c Release \
    -o /app/publish \
    --no-restore

# ---------- Runtime ----------
FROM mcr.microsoft.com/dotnet/aspnet:10.0 AS final
WORKDIR /app

COPY --from=build /app/publish .

# Usuário não-root já presente nas imagens .NET 8+.
USER app

ENV ASPNETCORE_URLS=http://+:8080
EXPOSE 8080

ENTRYPOINT ["dotnet", "Pedidos.Api.dll"]
