# Source Code Structure

This directory contains the main source code for the Legacy Order Service application.

## Architecture Overview

The application follows domain-driven design principles with a layered architecture:

```
┌─────────────────┐
│   Program.cs    │  Entry Point & Manual DI
├─────────────────┤
│   Services/     │  Application Services
├─────────────────┤
│   Models/       │  Domain Models (Entities)
├─────────────────┤
│   Events/       │  Domain Events
├─────────────────┤
│   Data/         │  Repositories & Data Access
├─────────────────┤
│   orders.db     │  SQLite Database
└─────────────────┘
```

## Directory Structure

### `/Data`
Repository implementations for data access:
- **OrderRepository.cs** - Persists orders to SQLite database
- **ProductRepository.cs** - Retrieves product information
- **CachingProductRepository.cs** - Decorator that adds caching to product repository

### `/Events`
Domain event infrastructure:
- **OrderPlacedEvent.cs** - Event raised when an order is successfully placed
- **DomainEventDispatcher.cs** - Publishes domain events to subscribers

### `/Exceptions`
Custom exception types for domain-specific errors:
- Domain validation exceptions
- Business rule violations

### `/Interfaces`
Contracts for services and repositories:
- IOrderService
- IOrderRepository
- IProductRepository
- IDomainEventDispatcher

### `/Models`
Domain entities and value objects:
- **Order.cs** - Order aggregate root with validation logic
- **Product.cs** - Product entity
- **OrderReadModel.cs** - Read model for order display
- **OrderMapping.cs** - Mapping between Order and OrderReadModel

### `/Services`
Application services that orchestrate business logic:
- **OrderService.cs** - Coordinates order placement workflow

## Key Design Patterns

### Repository Pattern
Data access is abstracted behind repository interfaces, allowing the domain layer to remain independent of persistence concerns.

### Decorator Pattern
`CachingProductRepository` wraps `ProductRepository` to add caching behavior without modifying the original implementation.

### Domain Events
Orders raise `OrderPlacedEvent` when successfully persisted, allowing other parts of the system to react to order placement.

### Factory Method
`Order.Create()` static factory method ensures all Order instances are properly validated.

## Dependency Management

The application uses **manual dependency injection** in Program.cs. Dependencies are created and wired up explicitly without a DI container:
- Logger (Serilog)
- Distributed Cache (Memory)
- Repositories
- Event Dispatcher
- Order Service

## Database Schema

The SQLite database (`orders.db`) stores order information with the following structure:
- Order ID (auto-increment)
- Customer Name
- Product Name
- Quantity
- Price
- Total

## Validation Strategy

Validation occurs at two levels:
1. **Input Validation** - Console input is validated in Program.cs
2. **Domain Validation** - Order.Create() enforces business rules and invariants
