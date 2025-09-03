using FluentValidation;
using FluentValidation.AspNetCore;
using Microsoft.AspNetCore.Mvc;
using ShoppingBasket.Api.Filters;
using ShoppingBasket.Application.Contracts;
using ShoppingBasket.Application.Services;
using ShoppingBasket.Application.Validators;
using ShoppingBasket.Domain.Repositories;
using ShoppingBasket.Infrastructure.Repositories;

var builder = WebApplication.CreateBuilder(args);

// Register FluentValidation
builder.Services.AddValidatorsFromAssemblyContaining<AddItemRequestValidator>();

// Add services to the container.
builder.Services.AddSingleton<IBasketRepository, BasketRepository>();
builder.Services.AddScoped<IBasketService, BasketService>();

builder.Services.AddEndpointsApiExplorer();
builder.Services.AddSwaggerGen();

var app = builder.Build();

if (app.Environment.IsDevelopment())
{
    app.UseSwagger();
    app.UseSwaggerUI(c =>
    {
        c.SwaggerEndpoint("/swagger/v1/swagger.json", "Online Shopping Basket API v1");
        c.RoutePrefix = string.Empty;
    });
}

// Get the basket
app.MapGet("/baskets", async (IBasketService service) => Results.Ok(await service.GetBasketAsync()));

app.MapPost("/basket/items", async (AddItemRequest req, IBasketService service) =>
{
    var basket = await service.AddItemToBasketAsync(req);
    return Results.Ok(basket);
}).AddEndpointFilter<ValidationFilter<AddItemRequest>>();

app.MapPost("/basket/items/bulk", async (AddMultipleItemsRequest req, IBasketService service) =>
{
    var updatedBasket = await service.AddMultipleItemsToBasketAsync(req);
    return Results.Ok(updatedBasket);
})
.AddEndpointFilter<ValidationFilter<AddMultipleItemsRequest>>();

app.Run();
