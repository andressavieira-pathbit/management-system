#See https://aka.ms/customizecontainer to learn how to customize your debug container and how Visual Studio uses this Dockerfile to build your images for faster debugging.

FROM mcr.microsoft.com/dotnet/aspnet:8.0 AS base
USER app
WORKDIR /app
EXPOSE 5000
EXPOSE 5001

FROM mcr.microsoft.com/dotnet/sdk:8.0 AS build
ARG BUILD_CONFIGURATION=Release
WORKDIR /src
COPY ["src/Api/Management.System.Api/Management.System.Api.csproj", "src/Api/Management.System.Api/"]
COPY ["src/Application/Management.System.Application/Management.System.Application.csproj", "src/Application/Management.System.Application/"]
COPY ["src/Common/Management.System.Common/Management.System.Common.csproj", "src/Common/Management.System.Common/"]
COPY ["src/Domain/Management.System.Domain/Management.System.Domain.csproj", "src/Domain/Management.System.Domain/"]
COPY ["src/Infrastructure/Management.System.Infrastructure/Management.System.Infrastructure.csproj", "src/Infrastructure/Management.System.Infrastructure/"]
RUN dotnet restore "./src/Api/Management.System.Api/./Management.System.Api.csproj"
COPY . .
WORKDIR "/src/src/Api/Management.System.Api"
RUN dotnet build "./Management.System.Api.csproj" -c $BUILD_CONFIGURATION -o /app/build

FROM build AS publish
ARG BUILD_CONFIGURATION=Release
RUN dotnet publish "./Management.System.Api.csproj" -c $BUILD_CONFIGURATION -o /app/publish /p:UseAppHost=false

FROM base AS final
WORKDIR /app
COPY --from=publish /app/publish .
ENTRYPOINT ["dotnet", "Management.System.Api.dll"]