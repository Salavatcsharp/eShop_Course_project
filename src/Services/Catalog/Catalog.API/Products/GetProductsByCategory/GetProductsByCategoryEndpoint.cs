namespace Catalog.API.Products.GetProductsByCategory;

public record GetProductsByCategoryRequest(string Category);

public record GetProductsByCategoryResponse(IEnumerable<Product> Products);

public class GetProductsByCategoryEndpoint : ICarterModule
{
    public void AddRoutes(IEndpointRouteBuilder app)
    {
        app.MapGet("/products/category/{category}",
                async (string category, ISender sender) =>
                {
                    var query = new GetProductsByCategoryQuery(category);
                    var result = await sender.Send(query);
                    var response = result.Adapt<GetProductsByCategoryResponse>();
                    return Results.Ok(response);
                })
            .WithName("GetProductsByCategory")
            .Produces<GetProductsByCategoryResponse>()
            .ProducesProblem(StatusCodes.Status400BadRequest)
            .WithSummary("Get products by category")
            .WithDescription("Get products by category");
    }
}