using AutoMapper;
using Entities.DataTransferObjects;
using Entities.Exceptions;
using Entities.Models;
using Entities.RequestFeatures;
using Repositories.Contracts;
using Services.Contracts;

namespace Services;

public class BookManager : IBookServices
{
    private readonly IRepositoryManager _manager;
    private readonly IMapper _mapper;

    public BookManager(IRepositoryManager manager, IMapper mapper)
    {
        _manager = manager;
        _mapper = mapper;
    }

    public async Task<BookDto> CreateOneBookAsync(BookDtoForInsertion bookDto)
    {
        var entity = _mapper.Map<Book>(bookDto);
        _manager.Book.CreateOneBook(entity);
        await _manager.SaveAsync();

        return _mapper.Map<BookDto>(entity);
    }

    public async Task DeleteOneBookAsync(int id, bool trackChanges)
    {
        var entity = await GetOneBookAndCheckExists(id, trackChanges);

        _manager.Book.DeleteOneBook(entity);
        await _manager.SaveAsync();
    }

    public async Task<IEnumerable<BookDto>> GetAllBooksAsync(BookParameters bookParameters, bool trackChanges)
    {
        var books = await _manager.Book.GetAllBooksAsync(bookParameters ,trackChanges);
        return _mapper.Map<IEnumerable<BookDto>>(books);
    }

    public async Task<BookDto> GetOneBookByIdAsync(int id, bool trackChanges)
    {
        var entity = await GetOneBookAndCheckExists(id, trackChanges);

        return _mapper.Map<BookDto>(entity);
    }

    public async Task<(BookDtoForUpdate bookDtoForUpdate, Book book)> GetOneBookForPatchAsync(
        int id,
        bool trackChanges
    )
    {
        var book = await GetOneBookAndCheckExists(id, trackChanges);

        var bookDtoForUpdate = _mapper.Map<BookDtoForUpdate>(book);
        return (bookDtoForUpdate, book);
    }

    public async Task SaveChangesForPatchAsync(BookDtoForUpdate bookDtoForUpdate, Book book)
    {
        _mapper.Map(bookDtoForUpdate, book);
        _manager.Book.UpdateOneBook(book);
        await _manager.SaveAsync();
    }

    public async Task UpdateOneBookAsync(int id, BookDtoForUpdate bookDto, bool trackChanges)
    {
        var entity = await GetOneBookAndCheckExists(id, trackChanges);

        // Mapping
        // entity.Title = bookDto.Title;
        // entity.Price = bookDto.Price;
        entity = _mapper.Map<Book>(bookDto);

        _manager.Book.UpdateOneBook(entity);
        await _manager.SaveAsync();
    }

    private async Task<Book> GetOneBookAndCheckExists(int id , bool trackChanges)
    {
        var entity = await _manager.Book.GetOneBookByIdAsync(id, trackChanges);

        if (entity is null)
            throw new BookNotFoundException(id);

        return entity;
    }
}