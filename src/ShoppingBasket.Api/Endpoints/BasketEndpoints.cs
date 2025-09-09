using ShoppingBasket.Api.Filters;
using ShoppingBasket.Application.Contracts;
using ShoppingBasket.Application.Services;
using ShoppingBasket.Domain.Entities;
using ShoppingBasket.Domain.ValueObjects;

namespace ShoppingBasket.Api.Endpoints
{
    public static class BasketEndpoints
    {
        public static void MapBasketEndpoints(this IEndpointRouteBuilder app)
        {
            /// <summary>Gets the current basket with all items, discounts, and shipping.</summary>
            app.MapGet("/basket", async (IBasketService service, CancellationToken ct) =>
                Results.Ok(await service.GetBasketAsync(ct)))
                .WithTags("Basket")
                .WithName("GetBasket")
                .Produces<Basket>(StatusCodes.Status200OK)
                .Produces(StatusCodes.Status404NotFound);

            /// <summary>Clears all items from the basket.</summary>
            app.MapDelete("/basket", async (IBasketService basketService, CancellationToken ct) =>
                Results.Ok(await basketService.ClearBasketAsync(ct)))
                .WithTags("Basket")
                .WithName("ClearBasket")
                .Produces<Basket>(StatusCodes.Status200OK);

            /// <summary>Adds a new item to the shopping basket.</summary>
            app.MapPost("/basket/items", async (AddItemRequest req, IBasketService service, CancellationToken ct) =>
                Results.Ok(await service.AddItemToBasketAsync(req, ct)))
                .AddEndpointFilter<ValidationFilter<AddItemRequest>>()
                .WithTags("Items")
                .WithName("AddItemToBasket")
                .Produces<Basket>(StatusCodes.Status200OK)
                .Produces(StatusCodes.Status400BadRequest);

            /// <summary>Adds multiple items to the basket.</summary>
            app.MapPost("/basket/items/bulk", async (AddMultipleItemsRequest req, IBasketService service, CancellationToken ct) =>
                Results.Ok(await service.AddMultipleItemsToBasketAsync(req, ct)))
                .AddEndpointFilter<ValidationFilter<AddMultipleItemsRequest>>()
                .WithTags("Items")
                .WithName("AddMultipleItemsToBasket")
                .Produces<Basket>(StatusCodes.Status200OK)
                .Produces(StatusCodes.Status400BadRequest);

            /// <summary>Removes an item from the basket by product ID.</summary>
            app.MapDelete("/basket/items/{productId:guid}", async (Guid productId, IBasketService service, CancellationToken ct) =>
                Results.Ok(await service.RemoveItemFromBasketAsync(productId, ct)))
                .WithTags("Items")
                .WithName("RemoveItemFromBasket")
                .Produces<Basket>(StatusCodes.Status200OK)
                .Produces(StatusCodes.Status404NotFound);

            /// <summary>Gets the total basket value without VAT.</summary>
            app.MapGet("/basket/total/without-vat", async (IBasketService service, CancellationToken ct) =>
                Results.Ok(await service.GetTotalWithoutVatAsync(ct)))
                .WithTags("Basket")
                .WithName("GetTotalWithoutVat")
                .Produces<Money>(StatusCodes.Status200OK);

            /// <summary>Gets the total basket value including VAT.</summary>
            app.MapGet("/basket/total/with-vat", async (IBasketService service, CancellationToken ct) =>
                Results.Ok(await service.GetTotalWithVatAsync(ct)))
                .WithTags("Basket")
                .WithName("GetTotalWithVat")
                .Produces<Money>(StatusCodes.Status200OK);

            /// <summary>Applies a discount code to the basket.</summary>
            app.MapPost("/basket/discount-code", async (ApplyDiscountCodeRequest request, IBasketService service, CancellationToken ct) =>
                Results.Ok(await service.ApplyDiscountCodeAsync(request.Code, ct)))
                .AddEndpointFilter<ValidationFilter<ApplyDiscountCodeRequest>>()
                .WithTags("Discount")
                .WithName("ApplyDiscountCode")
                .Produces<Basket>(StatusCodes.Status200OK)
                .Produces(StatusCodes.Status400BadRequest);

            /// <summary>Sets shipping method based on country code.</summary>
            app.MapPost("/basket/shipping", async (SetShippingRequest request, IBasketService service, CancellationToken ct) =>
                Results.Ok(await service.SetShippingAsync(request.CountryCode, ct)))
                .AddEndpointFilter<ValidationFilter<SetShippingRequest>>()
                .WithTags("Shipping")
                .WithName("SetShipping")
                .Produces<Basket>(StatusCodes.Status200OK)
                .Produces(StatusCodes.Status400BadRequest);
        }
    }
}