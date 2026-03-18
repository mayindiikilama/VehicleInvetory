Vehicle Inventory Microservice
Clean Architecture + Domain-Driven Design (ASP.NET Core Web API)

---

 BUSINESS CONTEXT

**Inventory Bounded Context** of a car rental platform:

 Manages vehicles at pickup and return locations  
 Tracks vehicle availability and operational state  
 Enforces vehicle lifecycle and status rules  
 Exposes inventory data to other bounded contexts  

**OUT OF SCOPE** (handled by other services):
- Reservations
- Payments
- Customers
- Loyalty programs

---

 CLEAN ARCHITECTURE LAYERS

 Layer Structure (Dependency Inversion)

```
WebAPI Layer (HTTP)
    ↓ References
Application Layer (Use Cases)
    ↓ References
Domain Layer (Business Rules)
    ↑ No dependencies
Infrastructure Layer (Persistence)
```

### Layer Responsibilities

**1. Domain Layer** 
- SIVehicle Aggregate Root
- VehicleStatus Enum (Available=0, Reserved=1, Rented=2, Serviced=3)
- DomainException for rule violations
- NO frameworks, NO EF Core, NO controllers

**2. Application Layer** 
- ISIVehicleService interface (use cases)
- ISIVehicleRepository interface (abstraction)
- DTOs (CreateVehicleRequestDto, VehicleResponseDto, UpdateVehicleStatusRequestDto)
- Business workflow coordination
- NO EF Core, NO ASP.NET dependencies

**3. Infrastructure Layer** 
- SIInventoryDbContext (EF Core mapping)
- SIVehicleRepository (concrete implementation)
- Database migrations
- SQL Server LocalDB configuration

**4. WebAPI Layer**
- VehiclesController (REST endpoints)
- Swagger/OpenAPI documentation
- Dependency Injection configuration
- HTTP status codes and error handling

---



### Prerequisites
- .NET 8 SDK (https://dotnet.microsoft.com/download)
- SQL Server LocalDB (included with Visual Studio 2022)
- Visual Studio 2022 Community Edition (free)

---

 API ENDPOINTS (Contract-First)

### Versioning Strategy
- Base URL: /api/v1/vehicles
- Version 2+ planned for future features (backward compatible)

### Endpoints

| HTTP | Endpoint | Request | Response | Status |
|------|----------|---------|----------|--------|
| GET | /api/v1/vehicles | - | VehicleResponseDto[] | 200 |
| GET | /api/v1/vehicles/{id} | - | VehicleResponseDto | 200, 404 |
| POST | /api/v1/vehicles | CreateVehicleRequestDto | VehicleResponseDto | 201, 400 |
| PUT | /api/v1/vehicles/{id}/status | UpdateVehicleStatusRequestDto | VehicleResponseDto | 200, 400, 404 |
| DELETE | /api/v1/vehicles/{id} | - | - | 204, 404 |

### Request/Response Examples

**POST /api/v1/vehicles (Create Vehicle)**
Request Body:
```json
{
  "vehicleCode": "TOY-001",
  "locationId": "11111111-1111-1111-1111-111111111111",
  "vehicleType": "Compact"
}
```

Response (201 Created):
```json
{
  "id": "550e8400-e29b-41d4-a716-446655440000",
  "vehicleCode": "TOY-001",
  "locationId": "11111111-1111-1111-1111-111111111111",
  "vehicleType": "Compact",
  "status": 0
}
```

**PUT /api/v1/vehicles/{id}/status (Update Status)**
Request Body:
```json
{
  "newStatus": 1
}
```

Status Codes:
- 0 = Available
- 1 = Reserved
- 2 = Rented
- 3 = Serviced

---

## 🔒 DOMAIN MODEL & BUSINESS RULES

### Vehicle Entity Properties
- Id (Guid) - Unique identifier
- VehicleCode (string, max 100) - Business code (e.g., TOY-001)
- LocationId (Guid) - Pickup/return location reference
- VehicleType (string, max 100) - Vehicle classification
- Status (enum) - Lifecycle state

### Business Rules Enforced in Domain Layer

**Rule 1: Cannot rent already rented vehicle**
```
Vehicle.Status == Rented → MarkRented() throws DomainException
```

**Rule 2: Cannot rent reserved vehicle**
```
Vehicle.Status == Reserved → MarkRented() throws DomainException
→ Customer must pickup first (transition to Available)
```

**Rule 3: Cannot rent vehicle under service**
```
Vehicle.Status == Serviced → MarkRented() throws DomainException
```

**Rule 4: Reserved vehicle needs explicit release**
```
Vehicle.Status == Reserved → MarkAvailable() throws DomainException
→ Manager must use explicit release process
```

**Rule 5: Only available vehicles can be reserved**
```
Vehicle.Status != Available → MarkReserved() throws DomainException
```

**Rule 6: Rented vehicles cannot go to service**
```
Vehicle.Status == Rented → MarkServiced() throws DomainException
→ Must return to Available first
```

### Status Transition State Machine

```
Available
    ├─ → Reserved (MarkReserved)
    └─ → Serviced (MarkServiced)

Reserved
    └─ → Rented (MarkRented - but must release first)

Rented
    └─ → Available (MarkAvailable)

Serviced
    └─ → Available (MarkAvailable)
```

---

##  VALIDATION & TESTING

### Input Validation (DTOs)
- VehicleCode: Required, Max 100 characters
- LocationId: Required (non-empty Guid)
- VehicleType: Required, Max 100 characters
- NewStatus: Required (valid enum 0-3)

### Domain Rule Testing via API

**Test: Invalid Transition (400 Bad Request)**
1. Create vehicle (Status = 0: Available)
2. PUT /status with newStatus=1 (Reserved) → Success 200
3. PUT /status with newStatus=2 (Rented) → FAILS 400
   - Response: "Reserved vehicle must be picked up before renting"

**Test: Vehicle Not Found (404)**
1. GET /api/v1/vehicles/00000000-0000-0000-0000-000000000000
   - Response: 404 Not Found

**Test: Invalid JSON (400)**
1. POST with missing VehicleCode
   - Response: 400 Bad Request (auto-validation)

---

## 📁 PROJECT STRUCTURE

```
SIVehicleInventory/
│
├── SIVehicleInventory.Domain/
│   ├── SIEntities/
│   │   └── SIVehicle.cs          (Aggregate Root, All Rules)
│   ├── SIEnums/
│   │   └── VehicleStatus.cs      (0-3 Status enum)
│   └── SIExceptions/
│       └── DomainException.cs     (Business rule violations)
│
├── SIVehicleInventory.Application/
│   ├── SIInterfaces/
│   │   ├── ISIVehicleRepository.cs    (Abstraction)
│   │   └── ISIVehicleService.cs       (Use cases)
│   ├── SIDtos/
│   │   ├── CreateVehicleRequestDto.cs
│   │   ├── VehicleResponseDto.cs
│   │   └── UpdateVehicleStatusRequestDto.cs
│   └── SIServices/
│       └── SIVehicleService.cs        (Workflows)
│
├── SIVehicleInventory.Infrastructure/
│   ├── Data/
│   │   ├── SIInventoryDbContext.cs    (EF Core)
│   │   └── SIInventoryDbContextFactory.cs (Design-time)
│   ├── Repositories/
│   │   └── SIVehicleRepository.cs     (SQL impl)
│   └── Migrations/
│       └── (EF Core generated)
│
├── SIVehicleInventory.WebAPI/
│   ├── Controllers/
│   │   └── VehiclesController.cs      (REST endpoints)
│   ├── Properties/
│   │   ├── launchSettings.json
│   │   └── appsettings.json
│   └── Program.cs                      (DI & middleware)
│
└── README.md                           (This file)
```

---

---

 ERROR HANDLING

### DomainException (Business Rule Violation)
```
HTTP 400 Bad Request
{
  "error": "Cannot rent reserved vehicle"
}
```

### KeyNotFoundException (Vehicle Not Found)
```
HTTP 404 Not Found
```

### ValidationProblem (Invalid Input)
```
HTTP 400 Bad Request
{
  "type": "https://tools.ietf.org/html/rfc9110#section-15.5.1",
  "title": "One or more validation errors occurred.",
  "errors": {
    "vehicleCode": ["The VehicleCode field is required."]
  }
}
```

---


- 4 separate projects with correct references
- No framework dependencies in Domain/Application
- Proper dependency inversion


- Vehicle aggregate with private setters
- All rules enforced in domain methods
- DomainException thrown on violations


- ISIVehicleService orchestrates workflows
- DTOs separate layers
- Repository abstraction implemented


- DbContext with Fluent configuration
- Migrations applied successfully
- Repository pattern implemented


- RESTful endpoints with correct verbs
- Delegates to Application layer
- Proper HTTP status codes


- DTO validation with data annotations
- Domain rules enforced via Swagger
- All CRUD operations tested


- OpenAPI spec auto-generated
- All endpoints visible and testable
- Try It Out functionality works


- Meaningful commits per feature
- Clean history showing progression


- Clear instructions
- Architecture explained
- Rules documented

---

## 🔐 KNOWN LIMITATIONS & FUTURE WORK

### Current Limitations
- Single bounded context (Inventory only)
- No pagination (small result sets)
- LocalDB only (development)
- No caching implemented
- No async event publishing


---

## 📝 DEPLOYMENT NOTES

### Local Development
```
dotnet run --project SIVehicleInventory.WebAPI
```

### Production (Azure SQL)
1. Update appsettings.Production.json with Azure SQL connection
2. Run migrations against production database
3. Deploy WebAPI to Azure App Service

---

## 👤 AUTHOR

**Samson Ikilama**  
Waterloo, Ontario, Canada  
March 2026
