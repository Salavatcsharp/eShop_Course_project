namespace Catalog.API.Products.DeleteProduct;

public record DeleteProductResponse(bool Succeeded);

public class DeleteProductEndpoint : ICarterModule
{
    public void AddRoutes(IEndpointRouteBuilder app)
    {
        app.MapDelete("/products/{id:guid}",
                async (Guid id, ISender sender) =>
                {
                    var command = new DeleteProductCommand(id);
                    var result = await sender.Send(command);
                    var response = result.Adapt<DeleteProductResponse>();
                    return Results.Ok(response);
                })
            .WithName("DeleteProduct")
            .Produces(StatusCodes.Status204NoContent)
            .ProducesProblem(StatusCodes.Status400BadRequest)
            .WithSummary("Delete product")
            .WithDescription("Delete product by Id");
    }
}