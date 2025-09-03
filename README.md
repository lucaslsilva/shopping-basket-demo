# shopping-basket-demo
This is a REST based Web API for an example online shopping basket using .NET 8 and C# 12.
It stores the basket and the items in an in-memory repository for simplicity.
It validates the input using FluentValidation.
There are some unit tests for the Domain and Application layers.

## Implemented Features

1. Add an item to the basket
POST /basket/items

2. Add multiple items to the basket
POST /basket/items/bulk

3. Remove an item from the basket
DELETE /basket/items/{productId}

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
