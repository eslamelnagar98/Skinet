FROM mcr.microsoft.com/dotnet/sdk:7.0 as build-env
WORKDIR /app
EXPOSE 80
EXPOSE 443

COPY ["API/API.csproj", "API/"]
COPY ["Core/Core.csproj", "Core/"]
COPY ["Infrastructure/Infrastructure.csproj", "Infrastructure/"]
RUN dotnet restore "./API/API.csproj"

COPY . ./
RUN dotnet publish -c Release - out

FROM mcr.microsoft.com/dotnet/sdk:7.0 

WORKDIR /app
COPY --from=build-env /app/out .
ENTRYPOINT ["dotnet","API/API.dll"]
