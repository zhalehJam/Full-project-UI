FROM mcr.microsoft.com/dotnet/sdk:7.0 AS build

WORKDIR /
COPY ["Framework/Framework.Core/Framework.Core.csproj", "Framework/Framework.Core/"]
COPY ["Framework/Framework.UserDataManagement/Framework.UserDataManagement.csproj", "Framework/Framework.UserDataManagement/"]
COPY ["Framework/Framework.Pagination/Framework.Pagination.csproj", "Framework/Framework.Pagination/"]
COPY ["Ticketing_UI/Ticketing_UI.csproj", "Ticketing_UI/"]
COPY ["Ticketing.Models/Ticketing.Models.csproj", "Ticketing.Models/"]
COPY ["Ticketing.Client.Contracts/Ticketing.Client.Contracts.csproj", "Ticketing.Client.Contracts/"]
COPY ["Ticketing.Repository/Ticketing.Repository.csproj", "Ticketing.Repository/"]
RUN dotnet restore "Ticketing_UI/Ticketing_UI.csproj"
COPY . .

WORKDIR "/Ticketing_UI"
RUN dotnet build "Ticketing_UI.csproj" -c Release -o /app/build

FROM build AS publish
RUN dotnet publish "Ticketing_UI.csproj" -c Release -o /app/publish

FROM nginx:alpine AS final
WORKDIR /usr/share/nginx/html
COPY --from=publish /app/publish/wwwroot .
COPY nginx.conf /etc/nginx/nginx.conf


