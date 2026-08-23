using Entities.Contracts;

namespace Entities.Models;

public class Book : IEntity
{
    public int Id { get; set; }
    public string Title { get; set; } = null!;
    public decimal Price { get; set; }
}
