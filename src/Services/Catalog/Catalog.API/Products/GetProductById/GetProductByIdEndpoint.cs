namespace Catalog.API.Products.GetProductById;

public record GetProductByIdResponse(Product Product);

public class GetProductByIdEndpoint : ICarterModule
{
    public void AddRoutes(IEndpointRouteBuilder app)
    {
        app.MapGet("/products/{id:guid}",
                async (Guid id, ISender sender) =>
                {
                    var query = new GetProductByIdQuery(id);
                    var result = await sender.Send(query);
                    var response = result.Adapt<GetProductByIdResponse>();
                    return Results.Ok(response);
                })
            .WithName("GetProductById")
            .Produces<GetProductByIdResponse>()
            .ProducesProblem(StatusCodes.Status400BadRequest)
            .WithSummary("Get product by Id")
            .WithDescription("Get product by Id");
    }
}