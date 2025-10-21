namespace Catalog.API.Products.UpdateProduct;

public record UpdateProductCommand(Guid Id, string Name, string Description, decimal Price, List<string> Category, string ImageFile)
    : ICommand<UpdateProductResult>;

public record UpdateProductResult(bool IsSuccess);

public class UpdateProductHandler(IDocumentSession session, ILogger<UpdateProductHandler> logger) : ICommandHandler<UpdateProductCommand, UpdateProductResult>
{
    public async Task<UpdateProductResult> Handle(UpdateProductCommand request, CancellationToken cancellationToken)
    {
        logger.LogInformation("UpdateProductHandler: Updating product {@Product}", request.Id);
        
        var product = await session.LoadAsync<Product>(request.Id, cancellationToken);

        if (product == null)
            throw new ProductNotFoundException();
        
        product.Name = request.Name;
        product.Description = request.Description;
        product.Price = request.Price;
        product.Category = request.Category;
        product.ImageFile = request.ImageFile;
        
        session.Update(product);
        await session.SaveChangesAsync(cancellationToken);
        
        return new  UpdateProductResult(true);
    }
}