using ShoppingBasket.Domain.Repositories;
using ShoppingBasket.Infrastructure.Repositories;

var builder = WebApplication.CreateBuilder(args);

// Add services to the container.
builder.Services.AddSingleton<IBasketRepository, BasketRepository>();

var app = builder.Build();

app.MapGet("/", () => "Hello World!");

app.Run();
