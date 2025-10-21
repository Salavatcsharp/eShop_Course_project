namespace Catalog.API.Products.DeleteProduct;

public record DeleteProductCommand(Guid Id) : ICommand<DeleteProductResult>;

public record DeleteProductResult(bool IsSuccess);

public class DeleteProductHandler(IDocumentSession sessision) : ICommandHandler<DeleteProductCommand, DeleteProductResult>
{
    public Task<DeleteProductResult> Handle(DeleteProductCommand request, CancellationToken cancellationToken)
    {
        throw new NotImplementedException();
    }
}