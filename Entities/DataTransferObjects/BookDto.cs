namespace Entities.DataTransferObjects;

public record BookDto
{
    public int Id { get; init; }
    public string Title { get; init; } = null!;
    public decimal Price { get; init; }
};

