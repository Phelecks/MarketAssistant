#See https://aka.ms/containerfastmode to understand how Visual Studio uses this Dockerfile to build your images for faster debugging.

FROM mcr.microsoft.com/dotnet/aspnet:9.0 AS base
WORKDIR /app
EXPOSE 80
EXPOSE 81
EXPOSE 82

FROM mcr.microsoft.com/dotnet/sdk:9.0 AS build
WORKDIR /src
COPY ["Microservices/Informing/Informing.Grpc/Informing.Grpc.csproj", "Microservices/Informing/Informing.Grpc/"]
RUN dotnet restore "Microservices/Informing/Informing.Grpc/Informing.Grpc.csproj"
COPY . .
WORKDIR "/src/Microservices/Informing/Informing.Grpc"
RUN dotnet build "Informing.Grpc.csproj" -c Release -o /app/build

FROM build AS publish
RUN dotnet publish "Informing.Grpc.csproj" -c Release -o /app/publish /p:UseAppHost=false

FROM base AS final
WORKDIR /app
COPY --from=publish /app/publish .
ENTRYPOINT ["dotnet", "Informing.Grpc.dll"]