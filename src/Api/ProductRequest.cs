namespace Api;

public record ProductRequest
{
    public string? Name { get; init; }
    public int Quantity { get; init; }
};
