FROM mcr.microsoft.com/dotnet/sdk:8.0 AS build
WORKDIR /src

COPY ["src/Domain/MsBooks.Domain.csproj", "src/Domain/"]
COPY ["src/Application/MsBooks.Application.csproj", "src/Application/"]
COPY ["src/Infrastructure/MsBooks.Infrastructure.csproj", "src/Infrastructure/"]
COPY ["src/Api/MsBooks.csproj", "src/Api/"]
COPY ["tests/UnitTests/MsBooks.UnitTests.csproj", "tests/UnitTests/"]
COPY ["ms-books.sln", "./"]

RUN dotnet restore "ms-books.sln"

COPY . .
RUN dotnet publish "src/Api/MsBooks.csproj" -c Release -o /app/publish --no-restore

FROM mcr.microsoft.com/dotnet/aspnet:8.0 AS runtime
WORKDIR /app

RUN adduser --disabled-password --gecos "" appuser && chown -R appuser /app
USER appuser

COPY --from=build /app/publish .

ENV ASPNETCORE_URLS="http://+:8080"
EXPOSE 8080

ENTRYPOINT ["dotnet", "MsBooks.dll"]
