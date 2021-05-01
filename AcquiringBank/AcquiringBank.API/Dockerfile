#See https://aka.ms/containerfastmode to understand how Visual Studio uses this Dockerfile to build your images for faster debugging.

FROM mcr.microsoft.com/dotnet/aspnet:5.0 AS base
WORKDIR /app
EXPOSE 80
EXPOSE 443

FROM mcr.microsoft.com/dotnet/sdk:5.0 AS build
WORKDIR /src
COPY ["AcquiringBank/AcquiringBank.API/AcquiringBank.API.csproj", "AcquiringBank/AcquiringBank.API/"]
RUN dotnet restore "AcquiringBank/AcquiringBank.API/AcquiringBank.API.csproj"
COPY . .
WORKDIR "/src/AcquiringBank/AcquiringBank.API"
RUN dotnet build "AcquiringBank.API.csproj" -c Release -o /app/build

FROM build AS publish
RUN dotnet publish "AcquiringBank.API.csproj" -c Release -o /app/publish

FROM base AS final
WORKDIR /app
COPY --from=publish /app/publish .
ENTRYPOINT ["dotnet", "AcquiringBank.API.dll"]