# 🎟️ Ticketing System

## Description
A simple yet robust ticket management system built with .NET Core, following Clean Architecture principles and Docker support.  
Includes:
- Backend RESTful API (`Ticketing.API`)
- Data infrastructure using Entity Framework Core (`Ticketing.Infrastructure`)
- Domain model and business logic (`Ticketing.Domain`)
- Unit tests for main components
- Orchestration with `docker-compose`

## Architecture
- **Ticketing.API**: Exposes HTTP endpoints, configures services and dependencies.
- **Ticketing.Domain**: Contains entities, aggregates, and domain logic.
- **Ticketing.Infrastructure**: Implements EF Core context, repositories, and migrations.
- **Ticketing.Tests**: Unit tests (xUnit/NUnit/MSTest).

## Requirements
- [.NET 8 SDK](https://dotnet.microsoft.com/)
- Docker & Docker Compose (optional, for containerized environments)
- SQL Server or PostgreSQL database (depending on configuration)

## Getting Started

1. Clone the repository:
   ```bash
   git clone https://github.com/cesarcedillo/Ticketing.git
   cd Ticketing
   ```

2. Add your connection string in `Ticketing.API/appsettings.json`:
   ```json
   "ConnectionStrings": {
     "DefaultConnection": "Server=localhost;Database=TicketingDb;User Id=sa;Password=Your_password123;"
   }
   ```

3. Apply migrations:
   ```bash
   dotnet ef migrations add InitialCreate --project ./Ticketing.Infrastructure --startup-project ./Ticketing.API
   dotnet ef database update --project ./Ticketing.Infrastructure --startup-project ./Ticketing.API
   ```

4. Run the application:
   ```bash
   cd Ticketing.API
   dotnet run
   ```
   The API will be available at `https://localhost:5001` or `http://localhost:5000`.

## Using Docker

1. Start all services:
   ```bash
   docker-compose up -d --build
   ```

2. Containers for the API and the database will be created.

3. Verify with:
   ```bash
   docker ps
   ```

## Available Endpoints
Briefly describe available RESTful endpoints, for example:
- `GET /api/tickets` – List all tickets
- `GET /api/tickets/{id}` – Get ticket by ID
- `POST /api/tickets` – Create a ticket
- `PUT /api/tickets/{id}` – Update a ticket
- `DELETE /api/tickets/{id}` – Delete a ticket


## Running Tests
From the root directory:
```bash
dotnet test
```

## Best Practices
- C# code style: Microsoft .NET Coding Conventions
- Constructor-based dependency injection
- Single Responsibility Principle
- Unit tests for critical logic
- Standard validation and error handling


