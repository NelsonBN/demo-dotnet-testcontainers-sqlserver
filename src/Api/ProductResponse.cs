﻿namespace Api;

public record ProductResponse
{
    public int Id { get; init; }
    public string? Name { get; init; }
    public int Quantity { get; init; }
};
