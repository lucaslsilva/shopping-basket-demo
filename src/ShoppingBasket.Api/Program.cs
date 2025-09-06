using FluentValidation;
using Microsoft.AspNetCore.Diagnostics;
using ShoppingBasket.Api.Endpoints;
using ShoppingBasket.Api.Filters;
using ShoppingBasket.Api.Middleware;
using ShoppingBasket.Application.Contracts;
using ShoppingBasket.Application.Services;
using ShoppingBasket.Application.Validators;
using ShoppingBasket.Domain.Repositories;
using ShoppingBasket.Infrastructure.Repositories;

var builder = WebApplication.CreateBuilder(args);

// Automatically scan the assembly where validators live
builder.Services.AddValidatorsFromAssemblyContaining<AddItemRequestValidator>();

// Add services to the container.
builder.Services.AddSingleton<IBasketRepository, BasketRepository>();
builder.Services.AddScoped<IDiscountCodeService, DiscountCodeService>();
builder.Services.AddScoped<IShippingService, ShippingService>();
builder.Services.AddScoped<IBasketService, BasketService>();

builder.Services.AddEndpointsApiExplorer();
builder.Services.AddSwaggerGen();

var app = builder.Build();

app.UseExceptionHandler(exceptionApp =>
{
    exceptionApp.Run(async context =>
    {
        var exception = context.Features.Get<IExceptionHandlerFeature>()?.Error;

        context.Response.ContentType = "application/json";

        switch (exception)
        {
            case InvalidOperationException ex:
                context.Response.StatusCode = StatusCodes.Status400BadRequest;
                await context.Response.WriteAsJsonAsync(new
                {
                    Title = "Invalid operation",
                    Detail = ex.Message,
                    Status = 400
                });
                break;

            default:
                context.Response.StatusCode = StatusCodes.Status500InternalServerError;
                await context.Response.WriteAsJsonAsync(new
                {
                    Title = "An unexpected error occurred",
                    Detail = exception?.Message,
                    Status = 500
                });
                break;
        }
    });
});

app.UseMiddleware<RequestResponseLoggingMiddleware>();

if (app.Environment.IsDevelopment())
{
    app.UseSwagger();
    app.UseSwaggerUI(c =>
    {
        c.SwaggerEndpoint("/swagger/v1/swagger.json", "Online Shopping Basket API v1");
        c.RoutePrefix = string.Empty;
    });
}

app.MapBasketEndpoints();

app.Run();
