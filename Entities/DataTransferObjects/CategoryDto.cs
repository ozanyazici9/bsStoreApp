namespace Entities.DataTransferObjects;

public record CategoryDto
{
    public int Id { get; init; }
    public string CategoryName { get; init; } = null!;
}
