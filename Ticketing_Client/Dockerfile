#See https://aka.ms/customizecontainer to learn how to customize your debug container and how Visual Studio uses this Dockerfile to build your images for faster debugging.

FROM .../mcr.microsoft.com/dotnet/aspnet:7.0 AS base
WORKDIR /app
EXPOSE 80
EXPOSE 443

FROM .../mcr.microsoft.com/dotnet/sdk:7.0 AS build
WORKDIR /src
COPY ["NuGet.Config", "/root/.nuget/NuGet/"]
COPY ["Framework/Framework.Core/Framework.Core.csproj", "Framework/Framework.Core/"]
COPY ["Framework/Framework.UserDataManagement/Framework.UserDataManagement.csproj", "Framework/Framework.UserDataManagement/"]
COPY ["Framework/Framework.Pagination/Framework.Pagination.csproj", "Framework/Framework.Pagination/"]
COPY ["Ticketing.Models/Ticketing.Models.csproj", "Ticketing.Models/"]
COPY ["Ticketing.Client.Contracts/Ticketing.Client.Contracts.csproj", "Ticketing.Client.Contracts/"]
COPY ["Ticketing.Repository/Ticketing.Repository.csproj", "Ticketing.Repository/"]
COPY ["Ticketing_Client/Ticketing_Client.csproj", "Ticketing_Client/"]
COPY . .
RUN dotnet restore "Ticketing_Client/Ticketing_Client.csproj"
WORKDIR "/src/Ticketing_Client"
RUN dotnet build "Ticketing_Client.csproj" -c Release -o /app/build

FROM build AS publish
RUN dotnet publish "Ticketing_Client.csproj" -c Release -o /app/publish /p:UseAppHost=false

FROM base AS final
WORKDIR /app
COPY --from=publish /app/publish .
ENTRYPOINT ["dotnet", "Ticketing_Client.dll"]