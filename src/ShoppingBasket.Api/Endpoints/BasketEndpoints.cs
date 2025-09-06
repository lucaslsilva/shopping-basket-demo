using ShoppingBasket.Api.Filters;
using ShoppingBasket.Application.Contracts;
using ShoppingBasket.Application.Services;

namespace ShoppingBasket.Api.Endpoints
{
    public static class BasketEndpoints
    {
        public static void MapBasketEndpoints(this IEndpointRouteBuilder app)
        {
            // Get the basket
            app.MapGet("/basket", async (IBasketService service) => Results.Ok(await service.GetBasketAsync()));

            // Clear the basket
            app.MapDelete("/basket", async (IBasketService basketService, CancellationToken ct) =>
            {
                var basket = await basketService.ClearBasketAsync(ct);
                return Results.Ok(basket);
            });

            // Add an item to the basket
            app.MapPost("/basket/items", async (AddItemRequest req, IBasketService service) =>
            {
                var updatedBasket = await service.AddItemToBasketAsync(req);
                return Results.Ok(updatedBasket);
            }).AddEndpointFilter<ValidationFilter<AddItemRequest>>();

            // Add multiple items to the basket
            app.MapPost("/basket/items/bulk", async (AddMultipleItemsRequest req, IBasketService service) =>
            {
                var updatedBasket = await service.AddMultipleItemsToBasketAsync(req);
                return Results.Ok(updatedBasket);
            }).AddEndpointFilter<ValidationFilter<AddMultipleItemsRequest>>();

            // Remove an item from the basket
            app.MapDelete("/basket/items/{productId:guid}", async (Guid productId, IBasketService service) =>
            {
                var updatedBasket = await service.RemoveItemFromBasketAsync(productId);
                return Results.Ok(updatedBasket);
            });

            // Get total without VAT
            app.MapGet("/basket/total/without-vat", async (IBasketService service, CancellationToken ct) =>
            {
                var total = await service.GetTotalWithoutVatAsync(ct);
                return Results.Ok(total);
            });

            // Get total with VAT
            app.MapGet("/basket/total/with-vat", async (IBasketService service, CancellationToken ct) =>
            {
                var total = await service.GetTotalWithVatAsync(ct);
                return Results.Ok(total);
            });

            // Apply a discount code
            app.MapPost("/basket/discount-code", async (
                ApplyDiscountCodeRequest request,
                IBasketService service,
                CancellationToken ct) =>
            {
                var basket = await service.ApplyDiscountCodeAsync(request.Code, ct);
                return Results.Ok(basket);
            }).AddEndpointFilter<ValidationFilter<ApplyDiscountCodeRequest>>();

            // Set shipping based on country code
            app.MapPost("/basket/shipping", async (
                SetShippingRequest request,
                IBasketService service,
                CancellationToken ct) =>
            {
                var basket = await service.SetShippingAsync(request.CountryCode, ct);
                return Results.Ok(basket);
            }).AddEndpointFilter<ValidationFilter<SetShippingRequest>>();
        }
    }
}
