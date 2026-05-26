# CleanArchAPI

ASP.NET Core 8 Web API byggt med Clean Architecture, CQRS, MediatR, Entity Framework Core och JWT-autentisering.

## Arkitektur

Projektet följer Clean Architecture med fyra lager där beroenden alltid pekar inåt mot Domain.

### Domain
Kärnan i applikationen. Innehåller entiteter och interface utan beroenden till något annat lager.
- `Product`, `Category`, `AppUser` — domänentiteter med 1-till-många relation (Category → Products)
- `IProductRepository`, `ICategoryRepository`, `IUserRepository` — interface som definierar dataåtkomst

### Application
Koordinerar applikationslogiken via CQRS-mönstret med MediatR.
- **Commands** — `CreateProduct`, `UpdateProduct`, `DeleteProduct`, `CreateCategory`, `UpdateCategory`, `DeleteCategory`, `Register`, `Login`
- **Queries** — `GetAllProducts` (med filter och paginering), `GetProductById`, `GetAllCategories`, `GetCategoryById`, `GetProductsByCategory`
- **Pipeline Behaviours** — `ValidationBehaviour` (FluentValidation) körs före varje handler, `LoggingBehaviour` loggar alla requests
- **AutoMapper** — `MappingProfile` mappar entiteter till DTOs så att databastabeller aldrig exponeras direkt

### Infrastructure
Implementerar interface definierade i Domain.
- `AppDbContext` — EF Core DbContext med SQL Server
- `ProductRepository`, `CategoryRepository`, `UserRepository` — databasåtkomst via EF Core
- `JwtService` — genererar JWT Bearer-tokens med roll-claims

### API
Tar emot HTTP-anrop och delegerar till Application-lagret via MediatR.
- `ProductsController`, `CategoriesController`, `AuthController`
- `ExceptionHandlingMiddleware` — global felhantering med JSON-svar
- Swagger/OpenAPI med JWT-stöd

## Kom igång

### Krav
- .NET 8 SDK
- SQL Server eller SQL Server LocalDB

### Starta projektet

```bash
# Klona repot
git clone https://github.com/<ditt-användarnamn>/CleanArchAPI.git
cd CleanArchAPI

# Starta API:et (migrering körs automatiskt vid uppstart)
cd CleanArchAPI.API
dotnet run
```

Swagger finns på `https://localhost:{port}/swagger`

### Connection string
Ändra vid behov i `CleanArchAPI.API/appsettings.json`:
```json
"DefaultConnection": "Server=(localdb)\\mssqllocaldb;Database=CleanArchDB;Trusted_Connection=True"
```

## Endpoints

### Auth
| Method | Route | Beskrivning |
|---|---|---|
| POST | /api/auth/register | Registrera användare, returnerar JWT-token |
| POST | /api/auth/login | Logga in, returnerar JWT-token |

### Products
| Method | Route | Roll | Beskrivning |
|---|---|---|---|
| GET | /api/products | User/Admin | Hämta produkter med filter och paginering |
| GET | /api/products/{id} | User/Admin | Hämta produkt via id |
| POST | /api/products | Admin | Skapa ny produkt |
| PUT | /api/products/{id} | Admin | Uppdatera produkt |
| DELETE | /api/products/{id} | Admin | Ta bort produkt |

### Categories
| Method | Route | Roll | Beskrivning |
|---|---|---|---|
| GET | /api/categories | User/Admin | Hämta alla kategorier |
| GET | /api/categories/{id} | User/Admin | Hämta kategori via id |
| GET | /api/categories/{id}/products | User/Admin | Hämta alla produkter i en kategori |
| POST | /api/categories | Admin | Skapa ny kategori |
| PUT | /api/categories/{id} | Admin | Uppdatera kategori |
| DELETE | /api/categories/{id} | Admin | Ta bort kategori |

## Autentisering

1. Registrera en Admin-användare via `POST /api/auth/register`:
```json
{
  "username": "admin",
  "password": "admin123",
  "role": "Admin"
}
```
2. Kopiera token från svaret
3. Klicka **Authorize** i Swagger och ange: `Bearer <token>`

## Sökfilter och paginering

`GET /api/products` stödjer följande query-parametrar:

| Parameter | Typ | Beskrivning |
|---|---|---|
| name | string | Filtrera på produktnamn |
| categoryId | int | Filtrera på kategori |
| minPrice | decimal | Minsta pris |
| maxPrice | decimal | Högsta pris |
| page | int | Sidnummer (standard: 1) |
| pageSize | int | Antal per sida (standard: 10) |

Exempel: `GET /api/products?name=laptop&minPrice=5000&page=1&pageSize=5`

## VG-krav

| Krav | Implementation |
|---|---|
| AutoMapper | `MappingProfile.cs` i Application-lagret, används i alla handlers |
| Pipeline Behaviour | `ValidationBehaviour` + `LoggingBehaviour` registrerade i rätt ordning |
| JWT | `JwtService.cs` genererar tokens med roll-claims |
| RBAC | `[Authorize(Roles = "Admin")]` på write-endpoints, Admin och User-roller |
| Repository Pattern | Interface i Domain, implementation i Infrastructure |
| CQRS + MediatR | Separerade Commands och Queries i Application-lagret |
