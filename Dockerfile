FROM mcr.microsoft.com/dotnet/sdk:8.0 AS build
WORKDIR /app

COPY ChawlaClinic.sln .
COPY ChawlaClinic.API/*.csproj ./ChawlaClinic.API/
COPY ChawlaClinic.DAL/*.csproj ./ChawlaClinic.DAL/
COPY ChawlaClinic.BL/*.csproj ./ChawlaClinic.BL/
COPY ChawlaClinic.Common/*.csproj ./ChawlaClinic.Common/

RUN dotnet restore ChawlaClinic.sln

COPY . .

RUN dotnet publish -c Release -o out

FROM mcr.microsoft.com/dotnet/aspnet:8.0 AS runtime
WORKDIR /app
COPY --from=build /app/out ./

EXPOSE 8080

ENTRYPOINT ["dotnet", "ChawlaClinic.API.dll"]