FROM mcr.microsoft.com/dotnet/sdk:8.0 AS build
ENV NETCoreSdkVersion=8.0.416
WORKDIR /src

COPY ["Directory.Build.props", "."]
COPY ["global.json", "."]
COPY ["src/AbpSaasPlatform.Domain.Shared/AbpSaasPlatform.Domain.Shared.csproj", "src/AbpSaasPlatform.Domain.Shared/"]
COPY ["src/AbpSaasPlatform.Domain/AbpSaasPlatform.Domain.csproj", "src/AbpSaasPlatform.Domain/"]
COPY ["src/AbpSaasPlatform.Application.Contracts/AbpSaasPlatform.Application.Contracts.csproj", "src/AbpSaasPlatform.Application.Contracts/"]
COPY ["src/AbpSaasPlatform.Application/AbpSaasPlatform.Application.csproj", "src/AbpSaasPlatform.Application/"]
COPY ["src/AbpSaasPlatform.EntityFrameworkCore/AbpSaasPlatform.EntityFrameworkCore.csproj", "src/AbpSaasPlatform.EntityFrameworkCore/"]
COPY ["src/AbpSaasPlatform.HttpApi.Host/AbpSaasPlatform.HttpApi.Host.csproj", "src/AbpSaasPlatform.HttpApi.Host/"]

RUN dotnet restore "src/AbpSaasPlatform.HttpApi.Host/AbpSaasPlatform.HttpApi.Host.csproj"

COPY . .
WORKDIR "/src/src/AbpSaasPlatform.HttpApi.Host"
RUN dotnet build "AbpSaasPlatform.HttpApi.Host.csproj" -c Release -o /app/build

FROM build AS publish
RUN dotnet publish "AbpSaasPlatform.HttpApi.Host.csproj" -c Release -o /app/publish

FROM mcr.microsoft.com/dotnet/aspnet:8.0 AS runtime
RUN apt-get update \
	&& apt-get install -y curl \
	&& rm -rf /var/lib/apt/lists/*
WORKDIR /app
COPY --from=publish /app/publish .
EXPOSE 5000
ENTRYPOINT ["dotnet", "AbpSaasPlatform.HttpApi.Host.dll"]
