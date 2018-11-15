FROM microsoft/dotnet:2.1-runtime AS base
WORKDIR /app

FROM microsoft/dotnet:2.1-sdk AS build
WORKDIR /src
COPY Docker.OrderDomain.Grpc.Server/Docker.OrderDomain.Grpc.Server.csproj Docker.OrderDomain.Grpc.Server/
COPY Docker.OrderDomain.Grpc/Docker.OrderDomain.Grpc.csproj Docker.OrderDomain.Grpc/
RUN dotnet restore Docker.OrderDomain.Grpc.Server/Docker.OrderDomain.Grpc.Server.csproj
COPY . .
WORKDIR /src/Docker.OrderDomain.Grpc.Server
RUN dotnet build Docker.OrderDomain.Grpc.Server.csproj -c Release -o /app

FROM build AS publish
RUN dotnet publish Docker.OrderDomain.Grpc.Server.csproj -c Release -o /app

FROM base AS final
WORKDIR /app
COPY --from=publish /app .
EXPOSE 50051/tcp
ENTRYPOINT ["dotnet", "Docker.OrderDomain.Grpc.Server.dll"]
