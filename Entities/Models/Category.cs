using Entities.Contracts;

namespace Entities.Models;

public class Category : IEntity
{
    public int Id { get; set; }
    public string CategoryName { get; set; } = null!;

    // Ref : Collection navigation property
    //public ICollection<Book> Books { get; set; } 
}
