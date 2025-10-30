# User Management API (.NET 8)

A layered ASP.NET Core 8 Web API that demonstrates JWT based authentication, role-protected endpoints, and CRUD operations for managing user accounts backed by PostgreSQL and Entity Framework Core.

The solution is organised into Core (domain contracts), Repository (data access), Service (business logic), and API projects. Swagger/OpenAPI is enabled for interactive exploration, and the API issues both access and refresh tokens.

## Features

- ✅ ASP.NET Core 8 minimal hosting model
- ✅ Entity Framework Core 8 with PostgreSQL provider
- ✅ JWT bearer authentication with refresh token support
- ✅ Role-based authorization for admin, super-admin, and basic users
- ✅ Layered architecture separating API, service, and repository responsibilities

## Prerequisites

- [.NET SDK 8.0](https://dotnet.microsoft.com/download)
- PostgreSQL 13+ (local installation or Docker)

Optional:

- Docker (for the provided `docker-compose.yml` Postgres service)

## Getting Started

1. **Clone the repository**
   ```bash
   git clone <repo-url>
   cd .net6-rest-api-crud-user-management
   ```

2. **Start PostgreSQL**
   - Update the connection string named `PostgreSQLdb` in `User.API/appsettings.json` to match your environment.
   - If you have Docker installed you can run the bundled database:
     ```bash
     docker compose up -d
     ```

3. **Configure the database**
   - Create the `userInfosDb` database manually or via tooling.
   - Add Entity Framework Core migrations and apply them as required (for example `dotnet ef migrations add InitialCreate` followed by `dotnet ef database update`).

4. **Restore and build the solution**
   ```bash
   dotnet restore
   dotnet build
   ```

5. **Run the API**
   ```bash
   dotnet run --project User.API
   ```
   The API listens on the port configured in `launchSettings.json` (defaults to `https://localhost:7083` and `http://localhost:5180`). Swagger UI is available at `/swagger`.

## Authentication Flow

1. Register a user with the desired role using `POST /api/User`.
2. Authenticate via `POST /api/Login` with the user's email and password to receive an access/refresh token pair.
3. Include the `Authorization: Bearer <accessToken>` header for subsequent requests.
4. Access role-protected endpoints on `/api/Product` (e.g., `/admin`, `/superAdmin`, `/basicUser`).

## Project Structure

```
User.Core         // Domain models and shared contracts
User.Repository   // EF Core DbContext and repositories
User.Service      // Application services and business logic
User.API          // ASP.NET Core 8 Web API with controllers and JWT handling
```

## Testing the API Quickly

Use a tool such as [HTTPie](https://httpie.io), curl, or [Postman](https://www.postman.com/) to exercise the endpoints.

Example using HTTPie:

```bash
# Register a user
http POST :5180/api/User fullName="Jane Admin" age:=30 email=jane@example.com password="P@ssword1" role=Admin

# Authenticate and capture the token
http POST :5180/api/Login email=jane@example.com password="P@ssword1"

# Call a protected endpoint
http GET :5180/api/Product/admin "Authorization: Bearer <accessToken>"
```

## Notes

- Update JWT settings (`Jwt:Issuer`, `Jwt:Audience`, `Jwt:Key`) in `appsettings.json` before running in production scenarios.
- Refresh token persistence is handled in the `UserInfo` entity; adjust token lifetimes in `TokenCreationHandler` as needed.
- This sample intentionally keeps password handling simple—ensure you hash and salt passwords for real-world usage.
