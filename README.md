# Shopping Basket API
This is a REST API implementing an online shopping basket system using .NET 8 and C# 12.
The focus is on clean architecture, domain-driven design principles, and REST API best practices. 
The basket and its items are stored in-memory for simplicity.

## Features Implemented
### Basket Management
- Add an item to the basket
- Add multiple items to the basket
- Remove an item from the basket
- Add multiple of the same item to the basket (increases quantity)
- Retrieve the basket 
- Clear the basket
- Ensure all items in the basket have the same currency

### Pricing
- Get the total cost of the basket with VAT (20%)
- Get the total cost of the basket without VAT

### Discounts
- Add discounted items (per-item discount)
- Apply discount codes to the basket (excluding already discounted items)

### Shipping
- Apply shipping cost based on country (UK, US, DE, or default cost)

### Validation
- Request validation using FluentValidation
- Business rules are also validated, for example positive price and quantity greater than or equal to 1.

### Error handling
- Global exception handling middleware
- Returns 400 responses for validation errors

### Logging
- Integrated request/response logging middleware
- Application level logging for critical operations

### Documentation
- Swagger/OpenAPI with grouped endpoints
- Explicit response types

## Tests
- There are some unit tests for the Domain and Application layers.

## How to run the API
1. Restore dependencies:
```
dotnet restore
```

2. Build the solution
```
dotnet build
```

3. Run the API:
```
dotnet run --project src/Basket.Api/ShoppingBasket.Api.csproj
```

4. Open the browser, and navigate to `http://localhost:5132/index.html` to access the Swagger page.
You can find the port in the console:
```
Now listening on: http://localhost:5132
```

## How to run the unit tests
```
dotnet test
```

## Next steps and improvements
- Add authentication/authoriaztion
- Replace in-memory repository with a database
- Add persistence with migrations
- Remove hard-coded discount codes and replace them with codes from the database
- Remove the hard-coded list of countries and their shipping costs and replace them with data from the database
- Support multiple currencies with proper conversion rules
- Improve test coverage with integration tests for API layer
- Add metrics/telemetry
- Add API rate limits
- Provide Dockerfile for containerized deployment
- Consider using CQRS