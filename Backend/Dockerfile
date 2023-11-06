FROM mcr.microsoft.com/dotnet/sdk:8.0 AS build
WORKDIR /app

COPY Backend/*.csproj ./Backend/
COPY Backend.Application/*.csproj ./Backend.Application/
COPY Backend.Database/*.csproj ./Backend.Database/
COPY Backend.Domain/*.csproj ./Backend.Domain/
COPY Backend.Infrastructure/*.csproj ./Backend.Infrastructure/
COPY Backend.Tests/*.csproj ./Backend.Tests/

RUN dotnet restore Backend/Backend.csproj

COPY . ./
WORKDIR /app/Backend
RUN dotnet publish -c Release -o out

FROM mcr.microsoft.com/dotnet/aspnet:6.0 AS runtime
WORKDIR /app
COPY --from=build /app/Backend/out ./

EXPOSE 80

ENTRYPOINT ["dotnet", "Backend.dll"]

