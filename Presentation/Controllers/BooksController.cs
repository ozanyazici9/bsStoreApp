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

    public BooksController(IServiceManager manager)
    {
        _manager = manager;
    }

    [HttpGet]
    public async Task<IActionResult> GetAllBooksAsync()
    {
        var books = await _manager.BookService.GetAllBooksAsync(trackChanges: false);
        return Ok(books);
    }

    [HttpGet("{id:int}")]
    public async Task<IActionResult> GetOneBookAsync([FromRoute(Name = "id")] int id)
    {
        var book = await _manager.BookService.GetOneBookByIdAsync(id, trackChanges: false);

        return Ok(book);
    }

    [ValidationAttributeFilter]
    [HttpPost]
    public async Task<IActionResult> CreateOneBookAsync([FromBody] BookDtoForInsertion bookDto)
    {
        if (bookDto == null)
            return BadRequest();

        if (!ModelState.IsValid)
            return UnprocessableEntity(ModelState);

        var book = await _manager.BookService.CreateOneBookAsync(bookDto);

        return StatusCode(201, book);
    }

    [HttpPut("{id:int}")]
    public async Task<IActionResult> UpdateOneBookAsync(
        [FromRoute(Name = "id")] int id,
        [FromBody] BookDtoForUpdate bookDto
    )
    {
        if (!ModelState.IsValid)
            return UnprocessableEntity(ModelState);

        if (id != bookDto.Id)
            return BadRequest();

        await _manager.BookService.UpdateOneBookAsync(id, bookDto, trackChanges: false);
        return NoContent();
    }

    [HttpDelete("{id:int}")]
    public async Task<IActionResult> DeleteAllBooksAsync([FromRoute(Name = "id")] int id)
    {
        await _manager.BookService.DeleteOneBookAsync(id, trackChanges: true);
        return NoContent();
    }

    [HttpPatch("{id:int}")]
    public async Task<IActionResult> PartiallyUpdateOneBookAsync(
        [FromRoute(Name = "id")] int id,
        [FromBody] JsonPatchDocument<BookDtoForUpdate> bookPatch
    )
    {
        if (bookPatch is null)
            return BadRequest(); // 400

        var result = await _manager.BookService.GetOneBookForPatchAsync(id, trackChanges: false);

        bookPatch.ApplyTo(result.bookDtoForUpdate, ModelState);

        TryValidateModel(result.bookDtoForUpdate);

        if (!ModelState.IsValid)
            return UnprocessableEntity(ModelState);

        await _manager.BookService.SaveChangesForPatchAsync(result.bookDtoForUpdate, result.book);

        return NoContent();
    }
}
