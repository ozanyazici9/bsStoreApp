namespace Entities.Exceptions;

public sealed class BookNotFound : NotFoundException
{
    public BookNotFound(int id)
        : base($"Book with id:{id} could not found.") { }
}
