# BluwoxServiceManagement

 Bluwox Service Management API

Clean Architecture REST API for managing services and categories with JWT authentication.

 Tech Stack
- .NET 8, ASP.NET Core Web API
- Entity Framework Core 8, SQL Server
- FluentValidation, JWT Bearer Authentication
- Swagger/OpenAPI

 Quick Start


 Clone and navigate
git clone <repo-url>
cd BluwoxServiceManagement

 Update connection string in appsettings.json
 Run migrations
cd src/BluwoxServiceManagement.API
dotnet ef database update 

 Run API
dotnet run

 Open Swagger
https://localhost:7285/swagger


 API Flow

 1. Get Authentication Token
http
POST /api/v1/auth/token
{
  "username": "testuser"
}

Response: { "token": "eyJhbGc..." }

 2. Authorize in Swagger
Click *Authorize*→ Enter: Bearer {token}`

### 3. Create Category

POST /api/v1/categories
Authorization: Bearer {token}

{
  "name": "Pipe Installation/Repair",
  "description": "Professional pipe services"
}



### 4. Create Service
http
POST /api/v1/services
Authorization: Bearer {token}

{
  "serviceName": "Plumbing",
  "baseFare": 8000.00,
  "isActive": true,
  "categoryIds": ["abc-123"]
}


### 5. Get All Services

GET /api/v1/services?pageNumber=1&pageSize=10
Authorization: Bearer {token}


### 6. Update Service
http
PUT /api/v1/services/{id}
Authorization: Bearer {token}

{
  "serviceName": "Updated Plumbing",
  "baseFare": 10000.00,
  "isActive": true,
  "categoryIds": ["abc-123"]
}


 7. Delete Service (Soft Delete)

DELETE /api/v1/services/{id}
Authorization: Bearer {token}

Layers:

API: Controllers, JWT auth, global exception handling
Application: Business logic, DTOs, validators
Domain: Entities, interfaces
Infrastructure: EF Core, repositories, database
