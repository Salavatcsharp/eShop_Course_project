namespace Catalog.API.Products.UpdateProduct;

public record UpdateProductRequest(
    Guid Id,
    string Name,
    string Description,
    decimal Price,
    List<string> Category,
    string ImageFile);
public record UpdateProductResponse(bool Succeeded);

public class UpdateProductEndpoint : ICarterModule
{
    public void AddRoutes(IEndpointRouteBuilder app)
    {
        app.MapPut("/products", async (UpdateProductRequest request, ISender sender) =>
        {
            var command = new UpdateProductCommand(
                request.Id,
                request.Name,
                request.Description,
                request.Price,
                request.Category,
                request.ImageFile);

            var result = await sender.Send(command);
            
            var response = result.Adapt<UpdateProductResponse>();

            return Results.Ok(response);
        })
        .WithTags("Products")
        .WithName("UpdateProduct")
        .WithSummary("Updates an existing product.")
        .WithDescription("Updates the details of an existing product in the catalog.")
        .Produces<UpdateProductResponse>()
        .Produces<UpdateProductResponse>(StatusCodes.Status400BadRequest)
        .Produces<UpdateProductResponse>(StatusCodes.Status404NotFound);
    }
}