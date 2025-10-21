namespace Catalog.API.Products.CreateProduct;

public record CreateProductCommand(
    string Name,
    List<string> Category,
    string Description,
    string ImageFile,
    decimal Price) : ICommand<CreateProductResult>;

public record CreateProductResult(Guid Id);

public class CreateProductHandlerValidator : AbstractValidator<CreateProductCommand>
{
    public CreateProductHandlerValidator()
    {
        RuleFor(x => x.Name)
            .NotEmpty().WithMessage("Product name is required.")
            .MaximumLength(100).WithMessage("Product name must not exceed 100 characters.");

        RuleFor(x => x.ImageFile)
            .NotEmpty().WithMessage("Product ImageFile is required.")
            .MaximumLength(1000).WithMessage("Product ImageFile must not exceed 1000 characters.");

        RuleFor(x => x.Price)
            .GreaterThan(0).WithMessage("Product price must be greater than zero.");

        RuleFor(x => x.Category)
            .NotEmpty().WithMessage("At least one category is required.");
    }
}

internal class CreateProductHandler(IDocumentSession session, IValidator<CreateProductCommand> validator) 
    : ICommandHandler<CreateProductCommand, CreateProductResult>
{
    public async Task<CreateProductResult> Handle(CreateProductCommand request, CancellationToken cancellationToken)
    {
        var result = await validator.ValidateAsync(request, cancellationToken);
        
        var errors = result.Errors
            .GroupBy(e => e.PropertyName)
            .ToDictionary(
                g => g.Key,
                g => g.Select(e => e.ErrorMessage).ToArray()
            );

        if (errors.Count != 0)
        {
            var errorMessages = string.Join("; ", errors.Select(kvp => $"{kvp.Key}: {string.Join(", ", kvp.Value)}"));
            throw new ValidationException(errorMessages);
        }
        
        var product = new Product()
        {
            Name = request.Name,
            Category = request.Category,
            Description = request.Description,
            ImageFile = request.ImageFile,
            Price = request.Price
        };
        
        session.Store(product);
        await session.SaveChangesAsync(cancellationToken);
        
        return new CreateProductResult(product.Id);
    }
}