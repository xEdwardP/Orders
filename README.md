# Orders

> A full-stack e-commerce / food-ordering platform built with .NET 8, Blazor WebAssembly and SQL Server.
> Plataforma full-stack de comercio electrónico / pedidos de comida construida con .NET 8, Blazor WebAssembly y SQL Server.

**🇬🇧 [English](#-english) · 🇪🇸 [Español](#-español)**

---

# 🇬🇧 English

## Table of contents

1. [Overview](#overview)
2. [Features](#features)
3. [Tech stack](#tech-stack)
4. [Architecture](#architecture)
5. [Repository structure](#repository-structure)
6. [Prerequisites](#prerequisites)
7. [Configuration](#configuration)
8. [Running the project](#running-the-project)
9. [Database and migrations](#database-and-migrations)
10. [Seed data and default users](#seed-data-and-default-users)
11. [Authentication and authorization](#authentication-and-authorization)
12. [API reference](#api-reference)
13. [Frontend routes](#frontend-routes)
14. [Testing](#testing)
15. [Security notes](#security-notes)
16. [Troubleshooting](#troubleshooting)
17. [Roadmap](#roadmap)

## Overview

**Orders** is a web application that lets customers browse a product catalog, add items to a shopping cart and place orders, while administrators manage the catalog (products, categories), the geographic tables (countries, states, cities), users and the lifecycle of every order.

The solution is split into four .NET 8 projects: a REST API (`Orders.Backend`), a Blazor WebAssembly SPA (`Orders.Frontend`), a class library with entities and DTOs shared by both (`Orders.Shared`), and an MSTest suite (`Orders.Tests`).

## Features

### Customer

- Public product catalog with pagination, text search and category filter.
- Product detail page with an image carousel.
- Shopping cart (*temporal orders*): add, edit quantity/remarks, remove items, live item counter.
- Order confirmation with automatic stock validation and decrement.
- Order history with detail view per order.
- Self-service account: registration with e-mail confirmation, login, profile editing (including photo), password change, password recovery and confirmation-token resend.

### Administrator

- Full CRUD for **Categories**, **Countries**, **States**, **Cities** and **Products**.
- Products support multiple categories and multiple images (uploaded to Azure Blob Storage).
- User listing with pagination.
- Access to every order in the system and the ability to change its status (`New`, `Dispatched`, `Sent`, `Confirmed`, `Cancelled`).

### Cross-cutting

- JWT authentication with role-based authorization (`Admin` / `User`).
- Server-side pagination on every list endpoint.
- Transactional e-mails through SMTP (MailKit).
- Image storage on Azure Blob Storage.
- Swagger UI with `Bearer` token support in Development.
- Automatic database seeding on startup.

## Tech stack

| Layer | Technology |
| --- | --- |
| Runtime | .NET 8 (`net8.0`) |
| API | ASP.NET Core Web API, Swashbuckle 6.5 (Swagger) |
| SPA | Blazor WebAssembly 8.0 |
| UI | MudBlazor 6.19, Bootstrap 5, Blazored.Modal 7.3, SweetAlert2 5.6 |
| ORM | Entity Framework Core 8 (SQL Server provider) |
| Identity | ASP.NET Core Identity + JWT Bearer |
| Storage | Azure.Storage.Blobs 12.19 |
| E-mail | MailKit 4.5 |
| Testing | MSTest 3.1, Moq 4.20, EF Core InMemory, Coverlet |

## Architecture

The backend follows a layered **Controller → Unit of Work → Repository → DbContext** design, with a generic implementation for each layer plus specialized subclasses where extra behavior is needed.

```
                 ┌──────────────────────────────┐
                 │  Orders.Frontend (Blazor WASM)│
                 │  Pages / Shared components    │
                 │  IRepository (typed HttpClient)│
                 │  AuthenticationProviderJWT    │
                 └───────────────┬──────────────┘
                                 │ HTTPS + JWT
                                 ▼
                 ┌──────────────────────────────┐
                 │  Orders.Backend (Web API)     │
                 │  Controllers  (GenericController<T>)
                 │  UnitsOfWork  (GenericUnitOfWork<T>)
                 │  Repositories (GenericRepository<T>)
                 │  Helpers: FileStorage, MailHelper, OrdersHelper
                 └───────┬──────────────┬───────┘
                         │              │
              ┌──────────▼───┐   ┌──────▼─────────────┐
              │ SQL Server   │   │ Azure Blob Storage │
              │ (EF Core)    │   │ (product/user imgs)│
              └──────────────┘   └────────────────────┘

                 ┌──────────────────────────────┐
                 │  Orders.Shared                │
                 │  Entities · DTOs · Enums      │
                 │  ActionResponse<T>            │
                 └──────────────────────────────┘
```

Key building blocks:

- **`GenericController<T>`** — exposes `GET /full`, `GET`, `GET /totalPages`, `GET /{id}`, `POST`, `PUT`, `DELETE /{id}`. Concrete controllers inherit from it and `override` only what they need.
- **`GenericRepository<T>` / `GenericUnitOfWork<T>`** — CRUD plus pagination; every operation returns `ActionResponse<T>` (`WasSuccess`, `Message`, `Result`) so errors travel as data instead of exceptions.
- **`QueryableExtensions.Paginate`** — single `Skip`/`Take` helper used by all repositories.
- **`OrdersHelper.ProcessOrderAsync`** — turns the user's temporal orders into a real `Order`: validates stock, creates the `OrderDetail` lines, decrements `Product.Stock` and clears the cart.
- **`IFileStorage`** — Azure Blob wrapper with a default interface method `EditFileAsync` that deletes the previous blob before uploading the new one.
- **Frontend `IRepository`** — thin wrapper over `HttpClient` returning `HttpResponseWrapper<T>` (`Error`, `Response`, `HttpResponseMessage`).
- **`AuthenticationProviderJWT`** — persists the JWT in `localStorage` under `TOKEN_KEY`, parses its claims and feeds Blazor's `AuthenticationStateProvider`.

## Repository structure

```
Orders/
├── Orders.sln
├── Orders.Backend/                  # REST API
│   ├── Controllers/                 # Accounts, Categories, Cities, Countries,
│   │                                # Orders, Products, States, TemporalOrders
│   ├── Data/
│   │   ├── DataContext.cs           # IdentityDbContext<User> + indexes + no cascade delete
│   │   ├── SeedDb.cs                # Initial data
│   │   └── CountriesStatesCities.sql# Optional full geo dataset
│   ├── Helpers/
│   │   ├── ImgHelpers/              # IFileStorage / FileStorage (Azure Blob)
│   │   ├── MailHelper/              # IMailHelper / MailHelper (MailKit SMTP)
│   │   ├── Orders/                  # IOrdersHelper / OrdersHelper (checkout)
│   │   └── QueryableExtensions.cs   # Paginate<T>
│   ├── Images/                      # Seed images (products, users)
│   ├── Migrations/                  # EF Core migrations
│   ├── Repositories/                # Interfaces + Implementations
│   ├── UnitsOfWork/                 # Interfaces + Implementations
│   ├── Program.cs                   # DI, Identity, JWT, CORS, Swagger, seeding
│   └── appsettings.json             # Connection strings, mail, jwtKey
├── Orders.Frontend/                 # Blazor WebAssembly SPA
│   ├── AuthenticationProviders/     # JWT provider (+ test provider)
│   ├── Helpers/                     # EnumHelper, JS interop (localStorage), selectors
│   ├── Layout/                      # MainLayout, NavMenu
│   ├── Pages/                       # Auth, Cart, Categories, Cities, Countries,
│   │                                # Products, States, Home
│   ├── Repositories/                # IRepository, Repository, HttpResponseWrapper
│   ├── Services/                    # ILoginService
│   ├── Shared/                      # Reusable components (see below)
│   ├── wwwroot/                     # index.html, css, images
│   └── Program.cs                   # DI, HttpClient base address, MudBlazor
├── Orders.Shared/                   # Contracts shared by API and SPA
│   ├── DTOs/                        # Login, Token, User, Product, Pagination, ...
│   ├── Entities/                    # Category, City, Country, Order, OrderDetail,
│   │                                # Product, ProductCategory, ProductImage,
│   │                                # State, TemporalOrder, User
│   ├── Enums/                       # OrderStatus, UserType
│   ├── Interfaces/                  # IEntityWithName
│   └── Responses/                   # ActionResponse<T>
└── Orders.Tests/                    # MSTest unit tests
    ├── Controllers/                 # CategoriesControllerTests
    ├── Repositories/                # CategoriesRepositoryTests (EF InMemory)
    └── UnitsOfWork/                 # CategoriesUnitOfWorkTests
```

### Reusable frontend components (`Orders.Frontend/Shared`)

| Component | Purpose |
| --- | --- |
| `GenericList` | Renders a list or an empty/loading state |
| `Pagination` | Page navigation bound to `PaginationDTO` |
| `GenericFilter` / `FiltersButtonsGeneric` | Text search and page-size selection |
| `FormWithName` | Shared create/edit form for `IEntityWithName` entities |
| `InputImg` | Image upload with base64 preview |
| `MultipleSelector` | Two-list selector (used for product categories) |
| `CarouselView` | MudBlazor carousel for product images |
| `AuthLinks` | Login/logout/profile menu with cart counter |
| `Loading` | Spinner placeholder |

## Data model

```
Country 1─* State 1─* City 1─* User (IdentityUser)
                                 │
                                 ├─* TemporalOrder *─1 Product
                                 └─* Order 1─* OrderDetail *─1 Product

Product *─* Category   (through ProductCategory)
Product 1─* ProductImage
```

Notable `DataContext` rules:

- Unique indexes on `Category.Name`, `Country.Name`, `Product.Name`, `(StateId, City.Name)` and `(CountryId, State.Name)`.
- Cascade delete is disabled globally (`DeleteBehavior.Restrict`), so records with children cannot be deleted.
- Command timeout raised to 600 s (the optional geo script is large).

## Prerequisites

- [.NET 8 SDK](https://dotnet.microsoft.com/download/dotnet/8.0)
- SQL Server — LocalDB (bundled with Visual Studio) is enough for development
- An Azure Storage account (for product/user images)
- An SMTP account (Gmail app password works) for account-confirmation e-mails
- Optional: Visual Studio 2022 17.8+ or JetBrains Rider; `dotnet-ef` for migrations

## Configuration

All backend settings live in `Orders/Orders.Backend/appsettings.json`:

```jsonc
{
  "ConnectionStrings": {
    "LocalConnection": "Server=(localdb)\\MSSQLLocalDB;Database=Orders;Trusted_Connection=True;MultipleActiveResultSets=true;Connection Timeout=600;Command Timeout=600;",
    "AzureStorage": "DefaultEndpointsProtocol=https;AccountName=...;AccountKey=...;EndpointSuffix=core.windows.net"
  },
  "Mail": {
    "From": "you@example.com",
    "Name": "Orders Support",
    "Smtp": "smtp.gmail.com",
    "Port": 587,
    "Password": "your-app-password"
  },
  "Url Frontend": "localhost:7234",
  "jwtKey": "a-long-random-secret-key"
}
```

| Key | Description |
| --- | --- |
| `ConnectionStrings:LocalConnection` | SQL Server connection string (referenced as `name=LocalConnection`) |
| `ConnectionStrings:AzureStorage` | Azure Blob Storage connection string used by `FileStorage` |
| `Mail:*` | SMTP host, port, sender and password used by `MailHelper` |
| `Url Frontend` | Host used to build the confirmation / password-reset links sent by e-mail |
| `jwtKey` | Symmetric key that signs the JWT — **must** be long and random |

> **Use user secrets in development.** From `Orders/Orders.Backend`:
> ```bash
> dotnet user-secrets init
> dotnet user-secrets set "jwtKey" "<your-secret>"
> dotnet user-secrets set "ConnectionStrings:AzureStorage" "<your-connection-string>"
> dotnet user-secrets set "Mail:Password" "<your-app-password>"
> ```

The frontend's API base address is **hard-coded** in `Orders/Orders.Frontend/Program.cs`:

```csharp
builder.Services.AddSingleton(sp => new HttpClient { BaseAddress = new Uri("https://localhost:7128/") });
```

Change that URL when you deploy the API somewhere else.

## Running the project

```bash
git clone https://github.com/xEdwardP/Orders.git
cd Orders/Orders

dotnet restore
dotnet build
```

Both projects must run at the same time.

```bash
# Terminal 1 — API (https://localhost:7128, Swagger at /swagger)
dotnet run --project Orders.Backend --launch-profile https

# Terminal 2 — SPA (https://localhost:7234)
dotnet run --project Orders.Frontend --launch-profile https
```

In Visual Studio, configure multiple startup projects (`Orders.Backend` + `Orders.Frontend`) and press <kbd>F5</kbd>.

| Project | HTTPS | HTTP |
| --- | --- | --- |
| `Orders.Backend` | `https://localhost:7128` | `http://localhost:5184` |
| `Orders.Frontend` | `https://localhost:7234` | `http://localhost:5273` |

CORS is wide open on the API (`AllowAnyMethod`, `AllowAnyHeader`, any origin, `AllowCredentials`), so no extra setup is needed locally.

## Database and migrations

On startup `Program.cs` calls `SeedData(app)`, which runs `SeedDb.SeedAsync()`. That method calls **`Database.EnsureCreatedAsync()`** — the schema is created directly from the model, *not* from the migrations. The first run therefore creates the `Orders` database automatically.

Existing migrations:

| Migration | Content |
| --- | --- |
| `20240429052858_InitialDb` | Countries, States, Cities |
| `20240507214641_AddUsersEntities` | ASP.NET Identity tables + `User` |
| `20240517212454_AddProductsTables` | Products, Categories, ProductCategories, ProductImages |
| `20240529155411_AddTemporalOrder` | Shopping cart |
| `20240603212745_AddOrderAndOrderDetailEntities` | Orders and order lines |

To work with migrations explicitly (from `Orders/Orders.Backend`):

```bash
dotnet tool install --global dotnet-ef       # once
dotnet ef migrations add <Name>
dotnet ef database update
```

> If you prefer migrations over `EnsureCreated`, replace `EnsureCreatedAsync()` with `MigrateAsync()` in `Data/SeedDb.cs`. Mixing the two on the same database is not supported by EF Core.

## Seed data and default users

`SeedDb` populates, in order:

1. **Countries** — Colombia (Antioquia, Bogotá) and United States (Florida, Texas), with five cities each. A full worldwide dataset is available in `Data/CountriesStatesCities.sql` via the commented-out `CheckCountriesFullAsync()`.
2. **Categories** — 14 food categories (`A la plancha`, `Asados`, `Bebidas`, `Carnes`, …).
3. **Roles** — `Admin` and `User`.
4. **Products** — 29 dishes with prices, stock and images read from `Images/products/` and uploaded to Azure Blob Storage.
5. **Users** — two confirmed accounts.

| E-mail | Password | Role |
| --- | --- | --- |
| `epineda@yopmail.com` | `123456` | Admin |
| `hector@yopmail.com` | `123456` | User |

An alternative retail catalog (electronics, footwear, sports) is kept in `CheckProductsAsync1()` / `CheckCategoriesAsync1()` with images in `Images/products1/`; swap the calls in `SeedAsync()` to use it.

> Seeding needs a valid `AzureStorage` connection string: images are uploaded before the products are saved. Exceptions are caught and printed to the console, so a failing seed does not stop the API from starting — but the catalog will be empty.

## Authentication and authorization

- **Identity policy** (`Program.cs`): confirmed e-mail required, unique e-mail, minimum length 6, no digit/uppercase/symbol requirements, lockout after 3 failed attempts for 5 minutes.
- **JWT**: signed with `jwtKey` (HMAC-SHA256), valid for 30 days, issuer/audience validation disabled, zero clock skew. Claims: `Name` (e-mail), `Role`, `Document`, `FirstName`, `LastName`, `Address`, `UserPhoto`, `CityId`.
- **Frontend**: the token is stored in `localStorage` (`TOKEN_KEY`) and attached as `Authorization: bearer <token>`.
- **Roles**: `Admin` sees the management menu (categories, countries, products, users, all orders); `User` only sees their own orders.
- Registration sends a confirmation e-mail; the account cannot log in until the link is clicked (`/api/accounts/ConfirmEmail`).

## API reference

Base URL: `https://localhost:7128`. Every controller except the anonymous endpoints below requires `Authorization: Bearer <token>`.

### Generic endpoints

`api/categories`, `api/cities`, `api/countries`, `api/states`, `api/products` and `api/temporalorders` inherit these routes from `GenericController<T>`:

| Method | Route | Description |
| --- | --- | --- |
| `GET` | `/full` | All records, no pagination |
| `GET` | `/?page=&recordsNumber=&filter=` | Paginated list |
| `GET` | `/totalPages?recordsNumber=&filter=` | Number of pages |
| `GET` | `/{id}` | Single record |
| `POST` | `/` | Create |
| `PUT` | `/` | Update |
| `DELETE` | `/{id}` | Delete |

### Accounts — `/api/accounts`

| Method | Route | Auth | Description |
| --- | --- | --- | --- |
| `POST` | `/Login` | Anonymous | Returns `TokenDTO` (`Token`, `Expiration`) |
| `POST` | `/CreateUser` | Anonymous | Registration + confirmation e-mail |
| `GET` | `/ConfirmEmail?userId=&token=` | Anonymous | Confirms the account |
| `POST` | `/ResendToken` | Anonymous | Resends the confirmation e-mail |
| `POST` | `/RecoverPassword` | Anonymous | Sends the reset link |
| `POST` | `/ResetPassword` | Anonymous | Sets a new password with the token |
| `POST` | `/changePassword` | JWT | Changes the current user's password |
| `GET` | `/` | JWT | Current user's profile |
| `PUT` | `/` | JWT | Updates the profile and returns a fresh token |
| `GET` | `/all` | Anonymous | Paginated user list |
| `GET` | `/totalPages` | Anonymous | Pages of the user list |

### Products — `/api/products`

| Method | Route | Auth | Description |
| --- | --- | --- | --- |
| `GET` | `/?page=&recordsNumber=&filter=&categoryFilter=` | Anonymous | Catalog with filters |
| `GET` | `/totalPages` | Anonymous | Page count |
| `GET` | `/{id}` | Anonymous | Product with categories and images |
| `POST` | `/full` | JWT | Creates a product with categories and images |
| `PUT` | `/full` | JWT | Updates a product with its categories |
| `POST` | `/addImages` | JWT | Adds images (base64) |
| `POST` | `/removeLastImage` | JWT | Removes the last image |
| `DELETE` | `/{id}` | JWT | Deletes the product and its blobs |

### Shopping cart — `/api/temporalorders`

| Method | Route | Description |
| --- | --- | --- |
| `POST` | `/full` | Adds a product to the current user's cart |
| `GET` | `/my` | Cart contents |
| `GET` | `/count` | Number of items (menu badge) |
| `GET` | `/{id}` | Single cart line |
| `PUT` | `/full` | Updates quantity/remarks |
| `DELETE` | `/{id}` | Removes the line |

### Orders — `/api/orders`

| Method | Route | Description |
| --- | --- | --- |
| `POST` | `/` | Confirms the cart and creates the order (validates and decrements stock) |
| `GET` | `/?page=&recordsNumber=` | Own orders; every order when the caller is `Admin` |
| `GET` | `/totalPages` | Page count |
| `GET` | `/{id}` | Order with its lines |
| `PUT` | `/` | Updates status and remarks (`OrderDTO`) |

### Combos (anonymous)

`GET /api/countries/combo` · `GET /api/states/combo/{countryId}` · `GET /api/cities/combo/{stateId}` · `GET /api/categories/combo`

## Frontend routes

| Route | Page | Access |
| --- | --- | --- |
| `/` | Catalog (`Home`) | Public |
| `/products/details/{id}` | Product detail | Public |
| `/Register`, `/Login`, `/logout` | Account | Public |
| `/RecoverPassword`, `/ResendToken` | Account recovery | Public |
| `/api/accounts/ConfirmEmail`, `/api/accounts/ResetPassword` | E-mail link landings | Public |
| `/EditUser`, `/changePassword` | Profile | Authenticated |
| `/Cart/ShowCart`, `/Cart/ModifyTemporalOrder/{id}`, `/Cart/OrderConfirmed` | Cart | Authenticated |
| `/orders`, `/cart/orderDetails/{id}` | Orders | Authenticated |
| `/categories`, `/categories/create`, `/categories/edit/{id}` | Categories | Admin |
| `/countries`, `/countries/create`, `/countries/edit/{id}`, `/countries/details/{id}` | Countries | Admin |
| `/states/create/{countryId}`, `/states/edit/{id}`, `/states/details/{id}` | States | Admin |
| `/cities/create/{stateId}`, `/cities/edit/{id}` | Cities | Admin |
| `/products`, `/products/create`, `/products/edit/{id}` | Products | Admin |
| `/users` | Users | Admin |

## Testing

```bash
cd Orders
dotnet test
```

`Orders.Tests` (MSTest + Moq + EF Core InMemory) currently covers the Categories vertical slice across the three layers:

- `Controllers/CategoriesControllerTests` — mocks both units of work and asserts the `IActionResult` returned by `GetAsync`, `GetPagesAsync` and `GetComboAsync`.
- `Repositories/CategoriesRepositoryTests` — runs against an InMemory database seeded per test with `Guid.NewGuid()` as the database name.
- `UnitsOfWork/CategoriesUnitOfWorkTests` — verifies the unit of work delegates to the right repository.

Use them as the template for new suites: create the mock in `[TestInitialize]`, arrange/act/assert, and clean up the context in `[TestCleanup]`.

## Security notes

⚠️ **`Orders/Orders.Backend/appsettings.json` is committed with real credentials** — Azure Storage account key, Gmail app password and the JWT signing key. Anyone with access to the repository can use them. Recommended remediation:

1. Rotate the Azure key, the Gmail app password and `jwtKey`.
2. Move all three to user secrets (development) or environment variables / Azure Key Vault (production).
3. Add `appsettings.json` to `.gitignore` and commit an `appsettings.example.json` with placeholders instead.

Other things worth hardening before going to production:

- CORS accepts any origin *and* credentials; restrict it to the SPA's domain.
- `GET /api/accounts/all` and `/api/accounts/totalPages` expose the user list without authentication.
- `app.UseAuthentication()` is not called explicitly — it is added by Identity, but making it explicit before `UseAuthorization()` avoids surprises.
- The password policy is deliberately permissive (six characters, no complexity).

## Troubleshooting

| Symptom | Cause / fix |
| --- | --- |
| Empty catalog after the first run | Seeding failed (usually an invalid `AzureStorage` string). Check the API console output. |
| `401 Unauthorized` on every call | Expired or missing token — log in again; the SPA stores it in `localStorage` under `TOKEN_KEY`. |
| SPA cannot reach the API | The API is not running on `https://localhost:7128`, or the base address in `Frontend/Program.cs` does not match. |
| Certificate errors on localhost | Run `dotnet dev-certs https --trust`. |
| No confirmation e-mail | Wrong `Mail:*` settings, or Gmail requires an app password (2FA accounts). |
| `Cannot delete` on a record | Cascade delete is disabled by design; remove the child records first. |

## Roadmap

- Restore the full geographic dataset (`CountriesStatesCities.sql`).
- Extend test coverage to the remaining controllers, repositories and units of work.
- Move secrets out of source control and add a CI workflow (`.github/workflows/` is currently empty).
- Make the API base address configurable per environment in the SPA.

---

# 🇪🇸 Español

## Tabla de contenido

1. [Descripción general](#descripción-general)
2. [Funcionalidades](#funcionalidades)
3. [Tecnologías](#tecnologías)
4. [Arquitectura](#arquitectura-1)
5. [Estructura del repositorio](#estructura-del-repositorio)
6. [Requisitos previos](#requisitos-previos)
7. [Configuración](#configuración)
8. [Ejecutar el proyecto](#ejecutar-el-proyecto)
9. [Base de datos y migraciones](#base-de-datos-y-migraciones)
10. [Datos iniciales y usuarios por defecto](#datos-iniciales-y-usuarios-por-defecto)
11. [Autenticación y autorización](#autenticación-y-autorización)
12. [Referencia de la API](#referencia-de-la-api)
13. [Rutas del frontend](#rutas-del-frontend)
14. [Pruebas](#pruebas)
15. [Notas de seguridad](#notas-de-seguridad)
16. [Solución de problemas](#solución-de-problemas)
17. [Trabajo pendiente](#trabajo-pendiente)

## Descripción general

**Orders** es una aplicación web que permite a los clientes navegar un catálogo de productos, agregarlos a un carrito de compras y realizar pedidos, mientras que los administradores gestionan el catálogo (productos y categorías), las tablas geográficas (países, estados y ciudades), los usuarios y el ciclo de vida de cada pedido.

La solución se divide en cuatro proyectos .NET 8: una API REST (`Orders.Backend`), una SPA en Blazor WebAssembly (`Orders.Frontend`), una biblioteca con las entidades y DTOs compartidos por ambos (`Orders.Shared`) y un conjunto de pruebas con MSTest (`Orders.Tests`).

## Funcionalidades

### Cliente

- Catálogo público de productos con paginación, búsqueda por texto y filtro por categoría.
- Página de detalle de producto con carrusel de imágenes.
- Carrito de compras (*órdenes temporales*): agregar, editar cantidad/comentarios, eliminar líneas y contador de artículos en vivo.
- Confirmación del pedido con validación y descuento automático de inventario.
- Historial de pedidos con vista de detalle por pedido.
- Autogestión de la cuenta: registro con confirmación por correo, inicio de sesión, edición de perfil (incluida la foto), cambio de contraseña, recuperación de contraseña y reenvío del token de confirmación.

### Administrador

- CRUD completo de **Categorías**, **Países**, **Estados/Departamentos**, **Ciudades** y **Productos**.
- Los productos admiten varias categorías y varias imágenes (se suben a Azure Blob Storage).
- Listado de usuarios con paginación.
- Acceso a todos los pedidos del sistema y cambio de su estado (`Nuevo`, `Despachado`, `Enviado`, `Confirmado`, `Cancelado`).

### Transversal

- Autenticación JWT con autorización por roles (`Admin` / `User`).
- Paginación en el servidor en todos los endpoints de listado.
- Correos transaccionales por SMTP (MailKit).
- Almacenamiento de imágenes en Azure Blob Storage.
- Swagger UI con soporte de token `Bearer` en Desarrollo.
- Siembra automática de la base de datos al iniciar.

## Tecnologías

| Capa | Tecnología |
| --- | --- |
| Runtime | .NET 8 (`net8.0`) |
| API | ASP.NET Core Web API, Swashbuckle 6.5 (Swagger) |
| SPA | Blazor WebAssembly 8.0 |
| UI | MudBlazor 6.19, Bootstrap 5, Blazored.Modal 7.3, SweetAlert2 5.6 |
| ORM | Entity Framework Core 8 (proveedor SQL Server) |
| Identidad | ASP.NET Core Identity + JWT Bearer |
| Almacenamiento | Azure.Storage.Blobs 12.19 |
| Correo | MailKit 4.5 |
| Pruebas | MSTest 3.1, Moq 4.20, EF Core InMemory, Coverlet |

## Arquitectura

El backend sigue un diseño en capas **Controlador → Unidad de trabajo → Repositorio → DbContext**, con una implementación genérica por capa y subclases especializadas donde se requiere comportamiento adicional.

```
                 ┌──────────────────────────────┐
                 │  Orders.Frontend (Blazor WASM)│
                 │  Páginas / Componentes        │
                 │  IRepository (HttpClient)     │
                 │  AuthenticationProviderJWT    │
                 └───────────────┬──────────────┘
                                 │ HTTPS + JWT
                                 ▼
                 ┌──────────────────────────────┐
                 │  Orders.Backend (Web API)     │
                 │  Controllers  (GenericController<T>)
                 │  UnitsOfWork  (GenericUnitOfWork<T>)
                 │  Repositories (GenericRepository<T>)
                 │  Helpers: FileStorage, MailHelper, OrdersHelper
                 └───────┬──────────────┬───────┘
                         │              │
              ┌──────────▼───┐   ┌──────▼─────────────┐
              │ SQL Server   │   │ Azure Blob Storage │
              │ (EF Core)    │   │ (imágenes)         │
              └──────────────┘   └────────────────────┘

                 ┌──────────────────────────────┐
                 │  Orders.Shared                │
                 │  Entidades · DTOs · Enums     │
                 │  ActionResponse<T>            │
                 └──────────────────────────────┘
```

Piezas clave:

- **`GenericController<T>`**: expone `GET /full`, `GET`, `GET /totalPages`, `GET /{id}`, `POST`, `PUT` y `DELETE /{id}`. Los controladores concretos heredan de él y solo hacen `override` de lo que necesitan.
- **`GenericRepository<T>` / `GenericUnitOfWork<T>`**: CRUD con paginación; toda operación devuelve `ActionResponse<T>` (`WasSuccess`, `Message`, `Result`), de modo que los errores viajan como datos y no como excepciones.
- **`QueryableExtensions.Paginate`**: único punto de `Skip`/`Take`, usado por todos los repositorios.
- **`OrdersHelper.ProcessOrderAsync`**: convierte las órdenes temporales del usuario en un `Order` real: valida el inventario, crea las líneas `OrderDetail`, descuenta `Product.Stock` y vacía el carrito.
- **`IFileStorage`**: envoltorio de Azure Blob con un método de interfaz por defecto `EditFileAsync` que elimina el blob anterior antes de subir el nuevo.
- **`IRepository` del frontend**: capa delgada sobre `HttpClient` que devuelve `HttpResponseWrapper<T>` (`Error`, `Response`, `HttpResponseMessage`).
- **`AuthenticationProviderJWT`**: guarda el JWT en `localStorage` bajo `TOKEN_KEY`, lee sus claims y alimenta el `AuthenticationStateProvider` de Blazor.

## Estructura del repositorio

```
Orders/
├── Orders.sln
├── Orders.Backend/                  # API REST
│   ├── Controllers/                 # Accounts, Categories, Cities, Countries,
│   │                                # Orders, Products, States, TemporalOrders
│   ├── Data/
│   │   ├── DataContext.cs           # IdentityDbContext<User> + índices + sin borrado en cascada
│   │   ├── SeedDb.cs                # Datos iniciales
│   │   └── CountriesStatesCities.sql# Dataset geográfico completo (opcional)
│   ├── Helpers/
│   │   ├── ImgHelpers/              # IFileStorage / FileStorage (Azure Blob)
│   │   ├── MailHelper/              # IMailHelper / MailHelper (SMTP con MailKit)
│   │   ├── Orders/                  # IOrdersHelper / OrdersHelper (checkout)
│   │   └── QueryableExtensions.cs   # Paginate<T>
│   ├── Images/                      # Imágenes de siembra (productos, usuarios)
│   ├── Migrations/                  # Migraciones de EF Core
│   ├── Repositories/                # Interfaces + Implementaciones
│   ├── UnitsOfWork/                 # Interfaces + Implementaciones
│   ├── Program.cs                   # DI, Identity, JWT, CORS, Swagger, siembra
│   └── appsettings.json             # Cadenas de conexión, correo, jwtKey
├── Orders.Frontend/                 # SPA en Blazor WebAssembly
│   ├── AuthenticationProviders/     # Proveedor JWT (+ proveedor de prueba)
│   ├── Helpers/                     # EnumHelper, interop JS (localStorage), selectores
│   ├── Layout/                      # MainLayout, NavMenu
│   ├── Pages/                       # Auth, Cart, Categories, Cities, Countries,
│   │                                # Products, States, Home
│   ├── Repositories/                # IRepository, Repository, HttpResponseWrapper
│   ├── Services/                    # ILoginService
│   ├── Shared/                      # Componentes reutilizables (ver abajo)
│   ├── wwwroot/                     # index.html, css, imágenes
│   └── Program.cs                   # DI, dirección base del HttpClient, MudBlazor
├── Orders.Shared/                   # Contratos compartidos por API y SPA
│   ├── DTOs/                        # Login, Token, User, Product, Pagination, ...
│   ├── Entities/                    # Category, City, Country, Order, OrderDetail,
│   │                                # Product, ProductCategory, ProductImage,
│   │                                # State, TemporalOrder, User
│   ├── Enums/                       # OrderStatus, UserType
│   ├── Interfaces/                  # IEntityWithName
│   └── Responses/                   # ActionResponse<T>
└── Orders.Tests/                    # Pruebas unitarias con MSTest
    ├── Controllers/                 # CategoriesControllerTests
    ├── Repositories/                # CategoriesRepositoryTests (EF InMemory)
    └── UnitsOfWork/                 # CategoriesUnitOfWorkTests
```

### Componentes reutilizables del frontend (`Orders.Frontend/Shared`)

| Componente | Propósito |
| --- | --- |
| `GenericList` | Renderiza una lista o un estado vacío / de carga |
| `Pagination` | Navegación de páginas enlazada a `PaginationDTO` |
| `GenericFilter` / `FiltersButtonsGeneric` | Búsqueda por texto y selección de registros por página |
| `FormWithName` | Formulario de creación/edición compartido para entidades `IEntityWithName` |
| `InputImg` | Carga de imágenes con vista previa en base64 |
| `MultipleSelector` | Selector de dos listas (usado para las categorías del producto) |
| `CarouselView` | Carrusel de MudBlazor para las imágenes del producto |
| `AuthLinks` | Menú de sesión/perfil con contador del carrito |
| `Loading` | Indicador de carga |

## Modelo de datos

```
Country 1─* State 1─* City 1─* User (IdentityUser)
                                 │
                                 ├─* TemporalOrder *─1 Product
                                 └─* Order 1─* OrderDetail *─1 Product

Product *─* Category   (a través de ProductCategory)
Product 1─* ProductImage
```

Reglas destacadas del `DataContext`:

- Índices únicos en `Category.Name`, `Country.Name`, `Product.Name`, `(StateId, City.Name)` y `(CountryId, State.Name)`.
- El borrado en cascada está deshabilitado globalmente (`DeleteBehavior.Restrict`), por lo que no se pueden eliminar registros con hijos.
- El tiempo de espera de comandos se eleva a 600 s (el script geográfico opcional es grande).

## Requisitos previos

- [SDK de .NET 8](https://dotnet.microsoft.com/download/dotnet/8.0)
- SQL Server — LocalDB (incluido con Visual Studio) es suficiente para desarrollo
- Una cuenta de Azure Storage (para las imágenes de productos y usuarios)
- Una cuenta SMTP (una contraseña de aplicación de Gmail funciona) para los correos de confirmación
- Opcional: Visual Studio 2022 17.8+ o JetBrains Rider; `dotnet-ef` para migraciones

## Configuración

Toda la configuración del backend vive en `Orders/Orders.Backend/appsettings.json`:

```jsonc
{
  "ConnectionStrings": {
    "LocalConnection": "Server=(localdb)\\MSSQLLocalDB;Database=Orders;Trusted_Connection=True;MultipleActiveResultSets=true;Connection Timeout=600;Command Timeout=600;",
    "AzureStorage": "DefaultEndpointsProtocol=https;AccountName=...;AccountKey=...;EndpointSuffix=core.windows.net"
  },
  "Mail": {
    "From": "tucorreo@example.com",
    "Name": "Soporte Orders",
    "Smtp": "smtp.gmail.com",
    "Port": 587,
    "Password": "tu-contraseña-de-aplicación"
  },
  "Url Frontend": "localhost:7234",
  "jwtKey": "una-clave-secreta-larga-y-aleatoria"
}
```

| Clave | Descripción |
| --- | --- |
| `ConnectionStrings:LocalConnection` | Cadena de conexión a SQL Server (se referencia como `name=LocalConnection`) |
| `ConnectionStrings:AzureStorage` | Cadena de conexión de Azure Blob Storage que usa `FileStorage` |
| `Mail:*` | Servidor, puerto, remitente y contraseña usados por `MailHelper` |
| `Url Frontend` | Host con el que se arman los enlaces de confirmación y restablecimiento enviados por correo |
| `jwtKey` | Clave simétrica que firma el JWT — **debe** ser larga y aleatoria |

> **Usa user secrets en desarrollo.** Desde `Orders/Orders.Backend`:
> ```bash
> dotnet user-secrets init
> dotnet user-secrets set "jwtKey" "<tu-secreto>"
> dotnet user-secrets set "ConnectionStrings:AzureStorage" "<tu-cadena-de-conexión>"
> dotnet user-secrets set "Mail:Password" "<tu-contraseña-de-aplicación>"
> ```

La dirección base de la API está **fija en el código**, en `Orders/Orders.Frontend/Program.cs`:

```csharp
builder.Services.AddSingleton(sp => new HttpClient { BaseAddress = new Uri("https://localhost:7128/") });
```

Cambia esa URL cuando despliegues la API en otro lugar.

## Ejecutar el proyecto

```bash
git clone https://github.com/xEdwardP/Orders.git
cd Orders/Orders

dotnet restore
dotnet build
```

Ambos proyectos deben ejecutarse al mismo tiempo.

```bash
# Terminal 1 — API (https://localhost:7128, Swagger en /swagger)
dotnet run --project Orders.Backend --launch-profile https

# Terminal 2 — SPA (https://localhost:7234)
dotnet run --project Orders.Frontend --launch-profile https
```

En Visual Studio, configura varios proyectos de inicio (`Orders.Backend` + `Orders.Frontend`) y presiona <kbd>F5</kbd>.

| Proyecto | HTTPS | HTTP |
| --- | --- | --- |
| `Orders.Backend` | `https://localhost:7128` | `http://localhost:5184` |
| `Orders.Frontend` | `https://localhost:7234` | `http://localhost:5273` |

La API tiene CORS completamente abierto (`AllowAnyMethod`, `AllowAnyHeader`, cualquier origen, `AllowCredentials`), así que no hace falta configuración adicional en local.

## Base de datos y migraciones

Al iniciar, `Program.cs` llama a `SeedData(app)`, que ejecuta `SeedDb.SeedAsync()`. Ese método invoca **`Database.EnsureCreatedAsync()`**: el esquema se crea directamente desde el modelo y *no* desde las migraciones. Por eso, la primera ejecución crea la base de datos `Orders` automáticamente.

Migraciones existentes:

| Migración | Contenido |
| --- | --- |
| `20240429052858_InitialDb` | Países, estados y ciudades |
| `20240507214641_AddUsersEntities` | Tablas de ASP.NET Identity + `User` |
| `20240517212454_AddProductsTables` | Productos, categorías, ProductCategories, ProductImages |
| `20240529155411_AddTemporalOrder` | Carrito de compras |
| `20240603212745_AddOrderAndOrderDetailEntities` | Pedidos y sus líneas |

Para trabajar con migraciones de forma explícita (desde `Orders/Orders.Backend`):

```bash
dotnet tool install --global dotnet-ef       # una sola vez
dotnet ef migrations add <Nombre>
dotnet ef database update
```

> Si prefieres migraciones en lugar de `EnsureCreated`, reemplaza `EnsureCreatedAsync()` por `MigrateAsync()` en `Data/SeedDb.cs`. EF Core no admite mezclar ambos enfoques sobre la misma base de datos.

## Datos iniciales y usuarios por defecto

`SeedDb` puebla, en este orden:

1. **Países**: Colombia (Antioquia, Bogotá) y Estados Unidos (Florida, Texas), con cinco ciudades cada uno. Hay un dataset mundial completo en `Data/CountriesStatesCities.sql`, disponible mediante el método comentado `CheckCountriesFullAsync()`.
2. **Categorías**: 14 categorías de comida (`A la plancha`, `Asados`, `Bebidas`, `Carnes`, …).
3. **Roles**: `Admin` y `User`.
4. **Productos**: 29 platillos con precio, inventario e imágenes leídas de `Images/products/` y subidas a Azure Blob Storage.
5. **Usuarios**: dos cuentas ya confirmadas.

| Correo | Contraseña | Rol |
| --- | --- | --- |
| `epineda@yopmail.com` | `123456` | Admin |
| `hector@yopmail.com` | `123456` | User |

Existe un catálogo alternativo de retail (tecnología, calzado, deportes) en `CheckProductsAsync1()` / `CheckCategoriesAsync1()` con imágenes en `Images/products1/`; basta con intercambiar las llamadas dentro de `SeedAsync()` para usarlo.

> La siembra requiere una cadena `AzureStorage` válida: las imágenes se suben antes de guardar los productos. Las excepciones se capturan y se imprimen en consola, así que un fallo en la siembra no impide que la API arranque, pero el catálogo quedará vacío.

## Autenticación y autorización

- **Política de Identity** (`Program.cs`): correo confirmado obligatorio, correo único, longitud mínima 6, sin requisitos de dígito/mayúscula/símbolo, bloqueo tras 3 intentos fallidos durante 5 minutos.
- **JWT**: firmado con `jwtKey` (HMAC-SHA256), válido por 30 días, sin validación de emisor ni audiencia y con tolerancia de reloj en cero. Claims: `Name` (correo), `Role`, `Document`, `FirstName`, `LastName`, `Address`, `UserPhoto`, `CityId`.
- **Frontend**: el token se guarda en `localStorage` (`TOKEN_KEY`) y se envía como `Authorization: bearer <token>`.
- **Roles**: `Admin` ve el menú de gestión (categorías, países, productos, usuarios y todos los pedidos); `User` solo ve sus propios pedidos.
- El registro envía un correo de confirmación; la cuenta no puede iniciar sesión hasta que se abra el enlace (`/api/accounts/ConfirmEmail`).

## Referencia de la API

URL base: `https://localhost:7128`. Todos los controladores requieren `Authorization: Bearer <token>`, salvo los endpoints anónimos indicados.

### Endpoints genéricos

`api/categories`, `api/cities`, `api/countries`, `api/states`, `api/products` y `api/temporalorders` heredan estas rutas de `GenericController<T>`:

| Método | Ruta | Descripción |
| --- | --- | --- |
| `GET` | `/full` | Todos los registros, sin paginar |
| `GET` | `/?page=&recordsNumber=&filter=` | Listado paginado |
| `GET` | `/totalPages?recordsNumber=&filter=` | Cantidad de páginas |
| `GET` | `/{id}` | Un registro |
| `POST` | `/` | Crear |
| `PUT` | `/` | Actualizar |
| `DELETE` | `/{id}` | Eliminar |

### Cuentas — `/api/accounts`

| Método | Ruta | Auth | Descripción |
| --- | --- | --- | --- |
| `POST` | `/Login` | Anónimo | Devuelve `TokenDTO` (`Token`, `Expiration`) |
| `POST` | `/CreateUser` | Anónimo | Registro + correo de confirmación |
| `GET` | `/ConfirmEmail?userId=&token=` | Anónimo | Confirma la cuenta |
| `POST` | `/ResendToken` | Anónimo | Reenvía el correo de confirmación |
| `POST` | `/RecoverPassword` | Anónimo | Envía el enlace de restablecimiento |
| `POST` | `/ResetPassword` | Anónimo | Establece una nueva contraseña con el token |
| `POST` | `/changePassword` | JWT | Cambia la contraseña del usuario actual |
| `GET` | `/` | JWT | Perfil del usuario actual |
| `PUT` | `/` | JWT | Actualiza el perfil y devuelve un token nuevo |
| `GET` | `/all` | Anónimo | Listado paginado de usuarios |
| `GET` | `/totalPages` | Anónimo | Páginas del listado de usuarios |

### Productos — `/api/products`

| Método | Ruta | Auth | Descripción |
| --- | --- | --- | --- |
| `GET` | `/?page=&recordsNumber=&filter=&categoryFilter=` | Anónimo | Catálogo con filtros |
| `GET` | `/totalPages` | Anónimo | Cantidad de páginas |
| `GET` | `/{id}` | Anónimo | Producto con categorías e imágenes |
| `POST` | `/full` | JWT | Crea un producto con categorías e imágenes |
| `PUT` | `/full` | JWT | Actualiza el producto y sus categorías |
| `POST` | `/addImages` | JWT | Agrega imágenes (base64) |
| `POST` | `/removeLastImage` | JWT | Elimina la última imagen |
| `DELETE` | `/{id}` | JWT | Elimina el producto y sus blobs |

### Carrito — `/api/temporalorders`

| Método | Ruta | Descripción |
| --- | --- | --- |
| `POST` | `/full` | Agrega un producto al carrito del usuario actual |
| `GET` | `/my` | Contenido del carrito |
| `GET` | `/count` | Número de artículos (indicador del menú) |
| `GET` | `/{id}` | Una línea del carrito |
| `PUT` | `/full` | Actualiza cantidad/comentarios |
| `DELETE` | `/{id}` | Elimina la línea |

### Pedidos — `/api/orders`

| Método | Ruta | Descripción |
| --- | --- | --- |
| `POST` | `/` | Confirma el carrito y crea el pedido (valida y descuenta inventario) |
| `GET` | `/?page=&recordsNumber=` | Pedidos propios; todos los pedidos si quien llama es `Admin` |
| `GET` | `/totalPages` | Cantidad de páginas |
| `GET` | `/{id}` | Pedido con sus líneas |
| `PUT` | `/` | Actualiza estado y comentarios (`OrderDTO`) |

### Combos (anónimos)

`GET /api/countries/combo` · `GET /api/states/combo/{countryId}` · `GET /api/cities/combo/{stateId}` · `GET /api/categories/combo`

## Rutas del frontend

| Ruta | Página | Acceso |
| --- | --- | --- |
| `/` | Catálogo (`Home`) | Público |
| `/products/details/{id}` | Detalle del producto | Público |
| `/Register`, `/Login`, `/logout` | Cuenta | Público |
| `/RecoverPassword`, `/ResendToken` | Recuperación de cuenta | Público |
| `/api/accounts/ConfirmEmail`, `/api/accounts/ResetPassword` | Destino de los enlaces del correo | Público |
| `/EditUser`, `/changePassword` | Perfil | Autenticado |
| `/Cart/ShowCart`, `/Cart/ModifyTemporalOrder/{id}`, `/Cart/OrderConfirmed` | Carrito | Autenticado |
| `/orders`, `/cart/orderDetails/{id}` | Pedidos | Autenticado |
| `/categories`, `/categories/create`, `/categories/edit/{id}` | Categorías | Admin |
| `/countries`, `/countries/create`, `/countries/edit/{id}`, `/countries/details/{id}` | Países | Admin |
| `/states/create/{countryId}`, `/states/edit/{id}`, `/states/details/{id}` | Estados | Admin |
| `/cities/create/{stateId}`, `/cities/edit/{id}` | Ciudades | Admin |
| `/products`, `/products/create`, `/products/edit/{id}` | Productos | Admin |
| `/users` | Usuarios | Admin |

## Pruebas

```bash
cd Orders
dotnet test
```

`Orders.Tests` (MSTest + Moq + EF Core InMemory) cubre actualmente la vertical de Categorías en las tres capas:

- `Controllers/CategoriesControllerTests`: simula ambas unidades de trabajo y verifica el `IActionResult` devuelto por `GetAsync`, `GetPagesAsync` y `GetComboAsync`.
- `Repositories/CategoriesRepositoryTests`: se ejecuta contra una base de datos InMemory sembrada por prueba, usando `Guid.NewGuid()` como nombre de base de datos.
- `UnitsOfWork/CategoriesUnitOfWorkTests`: comprueba que la unidad de trabajo delegue en el repositorio correcto.

Úsalas como plantilla para nuevas suites: crea el mock en `[TestInitialize]`, aplica arrange/act/assert y libera el contexto en `[TestCleanup]`.

## Notas de seguridad

⚠️ **`Orders/Orders.Backend/appsettings.json` está versionado con credenciales reales**: la clave de la cuenta de Azure Storage, la contraseña de aplicación de Gmail y la clave de firma del JWT. Cualquiera con acceso al repositorio puede usarlas. Remediación recomendada:

1. Rotar la clave de Azure, la contraseña de aplicación de Gmail y `jwtKey`.
2. Mover las tres a user secrets (desarrollo) o a variables de entorno / Azure Key Vault (producción).
3. Agregar `appsettings.json` al `.gitignore` y versionar en su lugar un `appsettings.example.json` con marcadores de posición.

Otros puntos a endurecer antes de ir a producción:

- CORS acepta cualquier origen *y* credenciales; conviene restringirlo al dominio de la SPA.
- `GET /api/accounts/all` y `/api/accounts/totalPages` exponen el listado de usuarios sin autenticación.
- No se llama explícitamente a `app.UseAuthentication()` — lo agrega Identity, pero declararlo antes de `UseAuthorization()` evita sorpresas.
- La política de contraseñas es deliberadamente permisiva (seis caracteres, sin complejidad).

## Solución de problemas

| Síntoma | Causa / solución |
| --- | --- |
| Catálogo vacío tras la primera ejecución | Falló la siembra (normalmente por una cadena `AzureStorage` inválida). Revisa la consola de la API. |
| `401 Unauthorized` en todas las llamadas | Token ausente o vencido: vuelve a iniciar sesión; la SPA lo guarda en `localStorage` bajo `TOKEN_KEY`. |
| La SPA no alcanza la API | La API no está corriendo en `https://localhost:7128`, o la dirección base de `Frontend/Program.cs` no coincide. |
| Errores de certificado en localhost | Ejecuta `dotnet dev-certs https --trust`. |
| No llega el correo de confirmación | Configuración `Mail:*` incorrecta, o Gmail exige contraseña de aplicación (cuentas con 2FA). |
| `No se puede eliminar` un registro | El borrado en cascada está deshabilitado por diseño; elimina primero los registros hijos. |

## Trabajo pendiente

- Restaurar el dataset geográfico completo (`CountriesStatesCities.sql`).
- Ampliar la cobertura de pruebas al resto de controladores, repositorios y unidades de trabajo.
- Sacar los secretos del control de versiones y agregar un workflow de CI (`.github/workflows/` está vacío actualmente).
- Hacer configurable por entorno la dirección base de la API en la SPA.

