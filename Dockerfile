FROM mcr.microsoft.com/dotnet/aspnet:8.0 AS base
WORKDIR /app
EXPOSE 80
EXPOSE 443

FROM mcr.microsoft.com/dotnet/sdk:8.0 AS build
WORKDIR /src

COPY ["src/InventoryManagement.Core/InventoryManagement.Core.csproj", "InventoryManagement.Core/"]
COPY ["src/InventoryManagement.Domain/InventoryManagement.Domain.csproj", "InventoryManagement.Domain/"]
COPY ["src/InventoryManagement.Infrastructure/InventoryManagement.Infrastructure.csproj", "InventoryManagement.Infrastructure/"]
COPY ["src/InventoryManagement.Persistence.Postgres/InventoryManagement.Persistence.Postgres.csproj", "InventoryManagement.Persistence.Postgres/"]
COPY ["src/InventoryManagement.Persistence.SqlServer/InventoryManagement.Persistence.SqlServer.csproj", "InventoryManagement.Persistence.SqlServer/"]
COPY ["src/InventoryManagement.WebApi/InventoryManagement.WebApi.csproj", "InventoryManagement.WebApi/"]
COPY ["src/Shared/InventoryManagement.Shared.Abstractions/InventoryManagement.Shared.Abstractions.csproj", "Shared/InventoryManagement.Shared.Abstractions/"]
COPY ["src/Shared/InventoryManagement.Shared.Infrastructure/InventoryManagement.Shared.Infrastructure.csproj", "Shared/InventoryManagement.Shared.Infrastructure/"]

RUN dotnet restore "InventoryManagement.WebApi/InventoryManagement.WebApi.csproj"
COPY . .
WORKDIR /src
RUN dotnet build "src/InventoryManagement.WebApi/InventoryManagement.WebApi.csproj" -c Release -o /app/build

FROM build AS publish

RUN dotnet publish "src/InventoryManagement.WebApi/InventoryManagement.WebApi.csproj" -c Release -o /app/publish
RUN dotnet dev-certs https -ep /app/publish/radyalabs.pfx -p pa55w0rd!


FROM base AS final
WORKDIR /app
COPY --from=publish /app/publish .
ENV ASPNETCORE_Kestrel__Certificates__Default__Password=pa55w0rd!
ENV ASPNETCORE_Kestrel__Certificates__Default__Path=radyalabs.pfx

ENTRYPOINT ["dotnet", "InventoryManagement.WebApi.dll"]