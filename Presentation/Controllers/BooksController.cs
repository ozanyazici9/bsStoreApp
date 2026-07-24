using AutoMapper;
using Entities.DataTransferObjects;
using Microsoft.AspNetCore.JsonPatch;
using Microsoft.AspNetCore.Mvc;
using Services.Contracts;

namespace Presentation.Controllers;

[ApiController]
[Route("api/[controller]")]
public class BooksController : ControllerBase
{
    private readonly IServiceManager _manager;
    private readonly IMapper _mapper;

    public BooksController(IServiceManager manager, IMapper mapper)
    {
        _manager = manager;
        _mapper = mapper;
    }

    [HttpGet]
    public IActionResult GetAllBooks()
    {
        var books = _manager.BookService.GetAllBooks(trackChanges: false);
        return Ok(books);
    }

    [HttpGet("{id:int}")]
    public IActionResult GetOneBook([FromRoute(Name = "id")] int id)
    {
        var book = _manager.BookService.GetOneBookById(id, trackChanges: false);

        return Ok(book);
    }

    [HttpPost]
    public IActionResult CreateOneBook([FromBody] BookDtoForInsertion bookDto)
    {
        if (bookDto == null)
            return BadRequest();

        if (!ModelState.IsValid)
            return UnprocessableEntity(ModelState);

        var book = _manager.BookService.CreateOneBook(bookDto);

        return StatusCode(201, book);
    }

    [HttpPut("{id:int}")]
    public IActionResult UpdateOneBook(
        [FromRoute(Name = "id")] int id,
        [FromBody] BookDtoForUpdate bookDto
    )
    {
        if (id != bookDto.Id)
            return BadRequest();

        _manager.BookService.UpdateOneBook(id, bookDto, trackChanges: true);
        return NoContent();
    }

    [HttpDelete("{id:int}")]
    public IActionResult DeleteAllBooks([FromRoute(Name = "id")] int id)
    {
        _manager.BookService.DeleteOneBook(id, trackChanges: true);
        return NoContent();
    }

    [HttpPatch("{id:int}")]
    public IActionResult PartiallyUpdateOneBook(
        [FromRoute(Name = "id")] int id,
        [FromBody] JsonPatchDocument<BookDtoForUpdate> bookPatch
    )
    {
        var entity = _manager.BookService.GetOneBookById(id, trackChanges: true);

        var bookDto = _mapper.Map<BookDtoForUpdate>(entity);

        bookPatch.ApplyTo(bookDto);
        _manager.BookService.UpdateOneBook(id, bookDto, trackChanges: true);
        return NoContent();
    }
}
