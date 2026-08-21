namespace Entities.DataTransferObjects;

public record CategoryDto
{
    public int CategoryId { get; init; }
    public string CategoryName { get; set; } = null!;
}
