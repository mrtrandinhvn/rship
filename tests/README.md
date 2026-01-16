# Tests

This directory contains unit tests for the Legacy Order Service application.

## Structure

```
tests/
└── LegacyOrderServiceUnitTests/
    └── Services/
        └── OrderServiceTests.cs
```

## Test Framework

The tests use:
- **xUnit** - Test framework
- **.NET 8.0** - Target framework

## Test Organization

### `/Services`
Tests for application services:
- **OrderServiceTests.cs** - Tests for OrderService business logic

## Testing Strategy

The tests follow these principles:

1. **Unit Tests** - Test individual components in isolation
2. **Mocking** - Dependencies are mocked to isolate the unit under test
3. **Arrange-Act-Assert** - Tests follow the AAA pattern for clarity
4. **Test Naming** - Method names describe the scenario and expected outcome

## Test Coverage Areas

Current test coverage includes:
- Order placement workflow
- Input validation
- Domain model validation
- Repository interactions
- Event dispatching

## Adding New Tests

When adding new tests:
1. Create test classes in the appropriate subdirectory matching the source structure
2. Follow the naming convention: `{ClassUnderTest}Tests.cs`
3. Use descriptive test method names: `MethodName_Scenario_ExpectedResult`
4. Mock external dependencies (repositories, event dispatchers, etc.)
5. Test both success and failure cases

## Test Dependencies

Test projects typically reference:
- **xUnit** - Test framework
- **Moq** or similar - Mocking framework
- **FluentAssertions** (optional) - Fluent assertion library
- Reference to the main project (`LegacyOrderService`)
