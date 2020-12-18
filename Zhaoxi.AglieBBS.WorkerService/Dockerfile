#See https://aka.ms/containerfastmode to understand how Visual Studio uses this Dockerfile to build your images for faster debugging.

FROM mcr.microsoft.com/dotnet/runtime:5.0-buster-slim AS base
WORKDIR /app

FROM mcr.microsoft.com/dotnet/sdk:5.0-buster-slim AS build
WORKDIR /src
COPY ["Zhaoxi.AglieBBS.WorkerService/Zhaoxi.AglieBBS.WorkerService.csproj", "Zhaoxi.AglieBBS.WorkerService/"]
RUN dotnet restore "Zhaoxi.AglieBBS.WorkerService/Zhaoxi.AglieBBS.WorkerService.csproj"
COPY . .
WORKDIR "/src/Zhaoxi.AglieBBS.WorkerService"
RUN dotnet build "Zhaoxi.AglieBBS.WorkerService.csproj" -c Release -o /app/build

FROM build AS publish
RUN dotnet publish "Zhaoxi.AglieBBS.WorkerService.csproj" -c Release -o /app/publish

FROM base AS final
WORKDIR /app
COPY --from=publish /app/publish .
ENTRYPOINT ["dotnet", "Zhaoxi.AglieBBS.WorkerService.dll"]