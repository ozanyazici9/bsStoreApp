using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Entities.DataTransferObjects;
using Entities.Exceptions;
using Entities.Models;
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
    public IActionResult CreateOneBook([FromBody] Book book)
    {
        if (book == null)
            return BadRequest();

        _manager.BookService.CreateOneBook(book);

        return StatusCode(201, book);
    }

    [HttpPut("{id:int}")]
    public IActionResult UpdateOneBook([FromRoute(Name = "id")] int id, [FromBody] BookDtoForUpdate bookDto)
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
        [FromBody] JsonPatchDocument<Book> bookPatch
    )
    {
        var entity = _manager.BookService.GetOneBookById(id, trackChanges: true);

        bookPatch.ApplyTo(entity);
        _manager.BookService.UpdateOneBook(id, new BookDtoForUpdate(entity.Id, entity.Title, entity.Price), trackChanges: true);
        return NoContent();
    }
}
