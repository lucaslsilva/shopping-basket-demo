using FluentValidation;
using Microsoft.OpenApi.Any;
using Microsoft.OpenApi.Models;
using ShoppingBasket.Api.Endpoints;
using ShoppingBasket.Api.Middleware;
using ShoppingBasket.Application.Contracts;
using ShoppingBasket.Application.Services;
using ShoppingBasket.Application.Validators;
using ShoppingBasket.Domain.Repositories;
using ShoppingBasket.Infrastructure.Repositories;

var builder = WebApplication.CreateBuilder(args);

// Validators
builder.Services.AddValidatorsFromAssemblyContaining<AddItemRequestValidator>();

// Services
builder.Services.AddSingleton<IBasketRepository, BasketRepository>();
builder.Services.AddScoped<IDiscountCodeService, DiscountCodeService>();
builder.Services.AddScoped<IShippingService, ShippingService>();
builder.Services.AddScoped<IBasketService, BasketService>();

// Swagger/OpenAPI
builder.Services.AddEndpointsApiExplorer();
builder.Services.AddSwaggerGen(options =>
{
    options.SwaggerDoc("v1", new OpenApiInfo
    {
        Title = "Shopping Basket API",
        Version = "v1",
        Description = "REST API for managing a shopping basket, including items, discounts, and shipping."
    });
});

var app = builder.Build();

// Middleware
app.UseMiddleware<ExceptionHandlingMiddleware>();
app.UseMiddleware<RequestResponseLoggingMiddleware>();

// Map endpoints
app.MapBasketEndpoints();

// Swagger UI only in Development
if (app.Environment.IsDevelopment())
{
    app.UseSwagger();
    app.UseSwaggerUI(c =>
    {
        c.SwaggerEndpoint("/swagger/v1/swagger.json", "Shopping Basket API v1");
        c.RoutePrefix = string.Empty; // Swagger at root
    });
}

app.Run();
