# Legacy Order Service

A .NET 8.0 console application that demonstrates a simple order processing system with domain-driven design principles.

## Overview

This application allows users to place orders for products through a console interface. It demonstrates:
- Domain-driven design with rich domain models
- Event-driven architecture with domain events
- Repository pattern for data access
- Caching for improved performance
- SQLite for data persistence

## Tech Stack

- **.NET 8.0** - Target framework
- **Microsoft.Data.Sqlite** - Database access
- **Serilog** - Logging
- **Microsoft.Extensions.Caching.Memory** - In-memory caching

## Project Structure

```
LegacyOrderService/
├── src/                    # Source code
│   ├── Data/              # Repository implementations
│   ├── Events/            # Domain events and event dispatcher
│   ├── Exceptions/        # Custom exceptions
│   ├── Interfaces/        # Service and repository interfaces
│   ├── Models/            # Domain models
│   ├── Services/          # Application services
│   ├── Program.cs         # Application entry point
│   └── orders.db          # SQLite database file
└── tests/                 # Unit tests
    └── LegacyOrderServiceUnitTests/
```

## How to Run

1. Ensure you have .NET 8.0 SDK installed
2. Navigate to the project directory
3. Build the project:
   ```bash
   dotnet build src/LegacyOrderService.csproj
   ```
4. Run the application:
   ```bash
   dotnet run --project src/LegacyOrderService.csproj
   ```
5. Follow the console prompts to enter:
   - Customer name
   - Product name
   - Quantity

## How to Test

Run unit tests using:
```bash
dotnet test tests/LegacyOrderServiceUnitTests/
```

## Key Features

- **Order Placement**: Process orders with customer information, product selection, and quantity
- **Product Caching**: Products are cached to improve lookup performance
- **Domain Events**: OrderPlacedEvent is dispatched after successful order placement
- **Data Validation**: Domain-level validation ensures data integrity
- **Error Handling**: Custom domain exceptions for business rule violations

## Database

The application uses SQLite (`orders.db`) to persist order data. The database file is created automatically in the application's output directory.

## Recent Changes

- Implemented OrderPlacedEvent dispatching for downstream processing
- Refactored OrderRepository to return OrderId after successful save
- Moved total calculation to Order entity for domain logic encapsulation
- Added connection string injection to OrderRepository
