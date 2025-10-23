namespace BuildingBlocks.Exceptions;

public class NotFoundException : Exception
{
    public NotFoundException(string message) : base(message)
    {
    }
    
    public NotFoundException(string message, object key) : base($"Entity not found. {message} Key: {key}")
    {
    }
}