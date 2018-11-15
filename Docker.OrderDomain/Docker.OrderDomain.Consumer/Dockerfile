FROM microsoft/dotnet:2.1-runtime AS base
WORKDIR /app

FROM microsoft/dotnet:2.1-sdk AS build
WORKDIR /src
COPY Docker.OrderDomain.Consumer/Docker.OrderDomain.Consumer.csproj Docker.OrderDomain.Consumer/
COPY Docker.OrderDomain.Grpc/Docker.OrderDomain.Grpc.csproj Docker.OrderDomain.Grpc/
RUN dotnet restore Docker.OrderDomain.Consumer/Docker.OrderDomain.Consumer.csproj
COPY . .
WORKDIR /src/Docker.OrderDomain.Consumer
RUN dotnet build Docker.OrderDomain.Consumer.csproj -c Release -o /app

FROM build AS publish
RUN dotnet publish Docker.OrderDomain.Consumer.csproj -c Release -o /app

FROM base AS final
WORKDIR /app
COPY --from=publish /app .
ENTRYPOINT ["dotnet", "Docker.OrderDomain.Consumer.dll"]
