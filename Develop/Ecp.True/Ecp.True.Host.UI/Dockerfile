#See https://aka.ms/containerfastmode to understand how Visual Studio uses this Dockerfile to build your images for faster debugging.

FROM mcr.microsoft.com/dotnet/core/aspnet:3.1-buster-slim AS base
WORKDIR /app
EXPOSE 80

# Install last version of bash (5.1)
RUN echo "deb http://deb.debian.org/debian bullseye main" | tee -a /etc/apt/sources.list
RUN apt update
RUN apt-get install bash -y
RUN bash --version 

FROM mcr.microsoft.com/dotnet/core/sdk:3.1-buster AS build
WORKDIR /src
COPY ["Ecp.True/Ecp.True.Host.UI/Ecp.True.Host.UI.csproj", "Ecp.True/Ecp.True.Host.UI/"]
COPY ["Ecp.True/Ecp.True.Host.Core/Ecp.True.Host.Core.csproj", "Ecp.True/Ecp.True.Host.Core/"]
COPY ["Ecp.True/Ecp.True.DataAccess/Ecp.True.DataAccess.csproj", "Ecp.True/Ecp.True.DataAccess/"]
COPY ["Ecp.True/Ecp.True.Core/Ecp.True.Core.csproj", "Ecp.True/Ecp.True.Core/"]
COPY ["Ecp.True/Ecp.True.Entities/Ecp.True.Entities.csproj", "Ecp.True/Ecp.True.Entities/"]
COPY ["Ecp.True/Ecp.True.Entities.Core/Ecp.True.Entities.Core.csproj", "Ecp.True/Ecp.True.Entities.Core/"]
COPY ["Ecp.True/Ecp.True.Entities.Configuration/Ecp.True.Entities.Configuration.csproj", "Ecp.True/Ecp.True.Entities.Configuration/"]
COPY ["Ecp.True/Ecp.True.Ioc/Ecp.True.Ioc.csproj", "Ecp.True/Ecp.True.Ioc/"]
COPY ["Ecp.True/Ecp.True.Logging/Ecp.True.Logging.csproj", "Ecp.True/Ecp.True.Logging/"]
COPY ["Ecp.True/Ecp.True.Configuration/Ecp.True.Configuration.csproj", "Ecp.True/Ecp.True.Configuration/"]
COPY ["Ecp.True/Ecp.True.KeyStore/Ecp.True.KeyStore.csproj", "Ecp.True/Ecp.True.KeyStore/"]
COPY ["Ecp.True/Ecp.True.Caching/Ecp.True.Caching.csproj", "Ecp.True/Ecp.True.Caching/"]
COPY ["Ecp.True/Ecp.True.Proxies.Azure/Ecp.True.Proxies.Azure.csproj", "Ecp.True/Ecp.True.Proxies.Azure/"]
COPY ["Ecp.True/Ecp.True.ExceptionHandling/Ecp.True.ExceptionHandling.csproj", "Ecp.True/Ecp.True.ExceptionHandling/"]
COPY ["Ecp.True/Ecp.True.DataAccess.Sql/Ecp.True.DataAccess.Sql.csproj", "Ecp.True/Ecp.True.DataAccess.Sql/"]
COPY ["Ecp.True/Ecp.True.Repositories/Ecp.True.Repositories.csproj", "Ecp.True/Ecp.True.Repositories/"]
COPY ["Ecp.True/Ecp.True.Processors.Api/Ecp.True.Processors.Api.csproj", "Ecp.True/Ecp.True.Processors.Api/"]
COPY ["Ecp.True/Ecp.True.Proxies.OwnershipRules/Ecp.True.Proxies.OwnershipRules.csproj", "Ecp.True/Ecp.True.Proxies.OwnershipRules/"]
COPY ["Ecp.True/Ecp.True.Processors.Transform/Ecp.True.Processors.Transform.csproj", "Ecp.True/Ecp.True.Processors.Transform/"]
COPY ["Ecp.True/Ecp.True.Processors.Registration/Ecp.True.Processors.Registration.csproj", "Ecp.True/Ecp.True.Processors.Registration/"]
COPY ["Ecp.True/Ecp.True.Processors.Core/Ecp.True.Processors.Core.csproj", "Ecp.True/Ecp.True.Processors.Core/"]
COPY ["Ecp.True/Ecp.True.DataAccess.NoSql/Ecp.True.DataAccess.NoSql.csproj", "Ecp.True/Ecp.True.DataAccess.NoSql/"]
RUN dotnet restore "Ecp.True/Ecp.True.Host.UI/Ecp.True.Host.UI.csproj"
COPY . .
WORKDIR "/src/Ecp.True/Ecp.True.Host.UI"
RUN dotnet build "Ecp.True.Host.UI.csproj" -c Release -o /app/build

FROM build AS publish
RUN dotnet publish "Ecp.True.Host.UI.csproj" -c Release -o /app/publish

FROM base AS final
WORKDIR /app
COPY --from=publish /app/publish .
ENTRYPOINT ["dotnet", "Ecp.True.Host.UI.dll"]