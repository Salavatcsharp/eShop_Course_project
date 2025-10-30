namespace Basket.API.Basket.StoreBasket;

public record StoreBasketRequest(ShoppingCart Cart);

public record StoreBasketResponse(string UserName);

public class StoreBasketEndpoint : ICarterModule
{
    public void AddRoutes(IEndpointRouteBuilder app)
    {
        app.MapPost("basket",
                async (StoreBasketRequest request, ISender sender) =>
                {
                    var command = new StoreBasketCommand(request.Cart);
                    var result = await sender.Send(command);
                    var response = result.Adapt<StoreBasketResponse>();
                    return Results.Created($"/basket/{response.UserName}", response);
                })
            .WithName("StoreBasket")
            .Produces<StoreBasketResponse>()
            .ProducesProblem(StatusCodes.Status400BadRequest)
            .WithSummary("Store basket")
            .WithDescription("Store basket");
    }
}