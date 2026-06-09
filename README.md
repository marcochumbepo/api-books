# ms-books API

API REST para gestion de libros desarrollada con .NET 8 y Clean Architecture.

## Stack Tecnologico

| Categoria       | Tecnologia                       |
| --------------- | -------------------------------- |
| Backend         | .NET 8 / ASP.NET Core Web API    |
| ORM             | Entity Framework Core 8          |
| Base de Datos   | SQL Server 2022                  |
| Validacion      | FluentValidation                 |
| Testing         | xUnit, Moq, FluentAssertions     |
| Documentacion   | Swagger / OpenAPI                |
| Contenedores    | Docker, Docker Compose           |
| CI/CD           | GitHub Actions                   |
| Cloud           | Azure Container Apps             |

## Arquitectura

El proyecto sigue **Clean Architecture** con 4 capas:

```
src/
├── Api/              # Controllers, Middleware, Program.cs
├── Application/      # Casos de uso, DTOs, Validaciones
├── Domain/           # Entidades, Interfaces de repositorio
└── Infrastructure/   # EF Core, Repositorios, DbContext

tests/
└── UnitTests/        # Pruebas unitarias con xUnit + Moq
```

**Principios aplicados:**
- **Dependency Inversion**: las capas internas definen interfaces, las externas las implementan.
- **Single Responsibility**: cada clase tiene una unica razon para cambiar.
- **Repository Pattern**: abstrae el acceso a datos detras de `IUserRepository`.

## Endpoints

| Metodo | Ruta              | Descripcion               | Respuestas |
| ------ | ----------------- | ------------------------- | ---------- |
| POST   | `/api/books`      | Crear libro               | 201, 400   |
| GET    | `/api/books`      | Listar todos los libros   | 200        |
| GET    | `/api/books/{id}` | Obtener por ID            | 200, 404   |
| PATCH  | `/api/books/{id}` | Actualizar libro          | 204, 404   |
| GET    | `/health`         | Health check              | 200        |

## Configuracion

Toda la configuracion de ambiente se maneja con **variables de entorno**.
El archivo `.env` es la unica fuente para desarrollo local y Docker.

```bash
cp .env.example .env
```

## Ejecucion Local

### Requisitos previos

- [Docker Desktop](https://www.docker.com/products/docker-desktop/)

### Con Docker Compose

```bash
cp .env.example .env
docker compose up -d

# La API estara disponible en http://localhost:8080
# Swagger: http://localhost:8080/swagger
# Health:  http://localhost:8080/health

docker compose down
```

## Testing

```bash
dotnet test ms-books.sln --configuration Release

# Con Docker
docker run --rm -v "${PWD}:/src" -w /src mcr.microsoft.com/dotnet/sdk:8.0 dotnet test
```

## CI/CD Pipeline

Definido en `.github/workflows/ci-cd.yml`:

1. **Restore**: descarga dependencias NuGet
2. **Build**: compila la solucion
3. **Test**: ejecuta pruebas unitarias
4. **Docker Build & Push**: construye y publica imagen en ACR
5. **Deploy**: actualiza Azure Container Apps con la nueva imagen
