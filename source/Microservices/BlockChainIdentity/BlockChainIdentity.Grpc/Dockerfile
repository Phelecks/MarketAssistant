#See https://aka.ms/customizecontainer to learn how to customize your debug container and how Visual Studio uses this Dockerfile to build your images for faster debugging.

FROM mcr.microsoft.com/dotnet/aspnet:9.0 AS base
WORKDIR /app
EXPOSE 80
EXPOSE 81
EXPOSE 82

FROM mcr.microsoft.com/dotnet/sdk:9.0 AS build
WORKDIR /src
COPY ["Microservices/BlockChainIdentity/BlockChainIdentity.Grpc/BlockChainIdentity.Grpc.csproj", "Microservices/BlockChainIdentity/BlockChainIdentity.Grpc/"]
RUN dotnet restore "Microservices/BlockChainIdentity/BlockChainIdentity.Grpc/BlockChainIdentity.Grpc.csproj"
COPY . .
WORKDIR "/src/Microservices/BlockChainIdentity/BlockChainIdentity.Grpc"
RUN dotnet build "BlockChainIdentity.Grpc.csproj" -c Release -o /app/build

FROM build AS publish
RUN dotnet publish "BlockChainIdentity.Grpc.csproj" -c Release -o /app/publish /p:UseAppHost=false

FROM base AS final
WORKDIR /app
COPY --from=publish /app/publish .
ENTRYPOINT ["dotnet", "BlockChainIdentity.Grpc.dll"]