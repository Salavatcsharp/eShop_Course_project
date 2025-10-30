namespace Basket.API.Basket.DeleteBasket;

public record DeleteBasketRequest(string UserName);

public record DeleteBasketResponse(bool IsSuccess);

public class DeleteBasketEndpoint : ICarterModule
{
    public void AddRoutes(IEndpointRouteBuilder app)
    {
        app.MapPost("basket/delete/{userName}",
                async (string userName, ISender sender) =>
                {
                    var command = new DeleteBasketCommand(userName);
                    var result = await sender.Send(command);
                    var response = result.Adapt<DeleteBasketResponse>();
                    return Results.Ok(response);
                })
            .WithName("DeleteBasket")
            .Produces<DeleteBasketResponse>()
            .ProducesProblem(StatusCodes.Status400BadRequest)
            .ProducesProblem(StatusCodes.Status404NotFound)
            .WithSummary("Delete basket")
            .WithDescription("Delete basket");
    }
}