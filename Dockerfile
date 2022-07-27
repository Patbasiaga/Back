#See https://aka.ms/containerfastmode to understand how Visual Studio uses this Dockerfile to build your images for faster debugging.

FROM mcr.microsoft.com/dotnet/aspnet:6.0 AS base
WORKDIR /app
EXPOSE 80
EXPOSE 443

FROM mcr.microsoft.com/dotnet/sdk:6.0 AS build
WORKDIR /src
COPY ["SignalRWithMQTT_Back.csproj", "."]
RUN dotnet restore "SignalRWithMQTT_Back.csproj"
COPY . .
WORKDIR "/src/"
RUN dotnet build "SignalRWithMQTT_Back.csproj" -c Release -o /app/build

FROM build AS publish
RUN dotnet publish "SignalRWithMQTT_Back.csproj" -c Release -o /app/publish

FROM base AS final
WORKDIR /app
COPY --from=publish /app/publish .
ENTRYPOINT ["dotnet", "SignalRWithMQTT_Back.dll"]
