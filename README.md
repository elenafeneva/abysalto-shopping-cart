# Abysalto Shopping Cart

Full-stack shopping cart application built with .NET Web API and Angular. This repository contains: 
- .NET 8 backend (ASP.NET Core)
- Angular frontend implementing a shopping cart example. 

The backend project lives under `ShoppingCart.API`, database code / EF migrations live in `ShoppingCart.Infrastructure`, and domain models live in `ShoppingCart.Domain`.


## Project layout
- `ShoppingCart.API` - ASP.NET Core Web API (controllers, features, services)
- `ShoppingCart.Infrastructure` - EF Core DbContext and Migrations
- `ShoppingCart.Domain` - domain entities and DTOs
- `ShoppingCart.App` - Angular frontend

When running EF CLI commands you must specify `--project` and `--startup-project` as shown in the migrations section. For example to run the migrations for this project ensure that you are in `ShoppingCart.API` project and then run `dotnet ef database update --project ../ShoppingCart.Infrastructure --startup-project ../ShoppingCart.API`

---

## Prerequisites
- .NET 8 SDK installed 
- PostgreSQL 
- `dotnet-ef` tool (for migrations)
- `Microsoft.EntityFrameworkCore.Design` referenced in `ShoppingCart.Infrastructure` project
- Node.js and npm (for the Angular frontend)

---

## Configuration
Important configuration files:
- ShoppingCart.API
	- `ShoppingCart.API/appsettings.json`
	- `ShoppingCart.API/appsettings.Development.json`
- ShoppingCart.App
	- `src/environments/environment`

Important keys used by the app (examples):
- `ConnectionString` 
- `DummyProductsUrl` 
- JWT Token: `Jwt:Secret`, `Jwt:Issuer`, `Jwt:Audience`, `Jwt:ExpiresMinutes` (JWT settings used for authentication)

---

## Run backend (development)

Check `appsettings.Development.json` and JWT / ConnectionString (`Host=localhost; Database=ShoppingCartDb; Username=postgres; Password=yourpassword`) values are set correctly before starting the app.

---

## Run frontend (Angular)
- Go to the frontend folder:
  - `cd ShoppingCart.App`
- Install dependencies: `npm install`
- Start dev server: `ng s -o`
- Configure `src/environments/environment.ts` to set `apiBase` to the backend URL (example: `https://localhost:44349/api`) and `tokenKey` used by localStorage.

---

## API endpoints (summary)
### Auth
- `POST /api/auth/register` - register user
- `POST /api/auth/login` - login user 
- `GET /api/auth/currentUser` - returns current user

### Products
- `GET /api/products?limit={n}&skip={n}&sortField={field}&sortOrder=asc|desc` - paged products (requires Authorization)
- `GET /api/products/{id}` - product by id (requires Authorization)
- `POST /api/products/{productId}` - create favorite product (requires Authorization)

### Cart
- `GET /api/cart` - retrieves products in user's shopping cart
- `POST /api/cart` - creates a new item to the cart or updates quantity if it already exists.
- `DELETE /api/cart/{productId}` removes a specific item from the shopping cart.

Response shapes
- Backend serializes public properties (not fields). If you see missing keys in JSON, change public fields to properties in DTOs/response objects.

---

## Useful commands for Entity Framework
- Create migration: `dotnet ef migrations add <Name> --project ShoppingCart.Infrastructure --startup-project ShoppingCart.API`
- Apply migrations: `dotnet ef database update --project ShoppingCart.Infrastructure --startup-project ShoppingCart.API`
- Remove last migration (if not applied): `dotnet ef migrations remove --project ShoppingCart.Infrastructure --startup-project ShoppingCart.API`

---
