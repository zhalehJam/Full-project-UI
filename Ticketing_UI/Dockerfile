FROM mcr.microsoft.com/dotnet/sdk:6.0 AS build
WORKDIR /src
COPY ["Ticketing_UI/Ticketing_UI.csproj", "Ticketing_UI/"]
COPY ["Framework.Models/Framework.Models.csproj", "Framework.Models/"]
COPY ["Framework/Framework.Core/Framework.Core.csproj", "Framework/Framework.Core/"]
COPY ["Ticketing.Client.Contracts/Framework.Client.Contracts.csproj", "Ticketing.Client.Contracts/"]
COPY ["Ticketing.Repository/Ticketing.Repository.csproj", "Ticketing.Repository/"]
COPY ["Framework/Framework.UserDataManagement/Framework.UserDataManagement.csproj", "Framework/Framework.UserDataManagement/"]
COPY ["Framework/Framework.Pagination/Framework.Pagination.csproj", "Framework/Framework.Pagination/"]
RUN dotnet restore "Ticketing_UI/Ticketing_UI.csproj"
COPY . .
WORKDIR "/src/Ticketing_UI"
RUN dotnet build "Ticketing_UI.csproj" -c Release -o /app/build

FROM build AS publish
RUN dotnet publish "Ticketing_UI.csproj" -c Release -o /app/publish

FROM nginx:alpine AS final
WORKDIR /usr/share/nginx/html
COPY --from=publish /app/publish/wwwroot .
COPY Ticketing_UI/nginx.conf /etc/nginx/nginx.conf