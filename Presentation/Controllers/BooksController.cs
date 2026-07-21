using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
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
        throw new Exception("test");
        var book = _manager.BookService.GetOneBookById(id, trackChanges: false);

        if (book is null)
            return NotFound();

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
    public IActionResult UpdateOneBook([FromRoute(Name = "id")] int id, [FromBody] Book book)
    {
        if (id != book.Id)
            return BadRequest();

        _manager.BookService.UpdateOneBook(id, book, trackChanges: true);
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

        if (entity == null)
            return NotFound();

        bookPatch.ApplyTo(entity);
        _manager.BookService.UpdateOneBook(id, entity, trackChanges: true);
        return NoContent();
    }
}
