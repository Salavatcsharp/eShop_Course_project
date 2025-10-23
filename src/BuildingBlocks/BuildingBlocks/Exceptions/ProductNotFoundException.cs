namespace BuildingBlocks.Exceptions;

public class ProductNotFoundException(Guid id) : NotFoundException("Product", id);