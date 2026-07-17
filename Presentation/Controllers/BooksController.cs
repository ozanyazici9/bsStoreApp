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
    public async Task<IActionResult> GetAllBooks()
    {
        try
        {
            var books = _manager.BookService.GetAllBooks(trackChanges: false);
            return Ok(books);
        }
        catch (Exception ex)
        {
            return BadRequest(ex.Message);
        }
    }

    [HttpGet("{id:int}")]
    public async Task<IActionResult> GetOneBook([FromRoute(Name = "id")] int id)
    {
        try
        {
            var book = _manager.BookService.GetOneBookById(id, trackChanges: false);
            return Ok(book);
        }
        catch (Exception ex)
        {
            return BadRequest(ex.Message);
        }
    }

    [HttpPost]
    public async Task<IActionResult> CreateOneBook([FromBody] Book book)
    {
        try
        {
            if (book == null)
                return BadRequest();

            _manager.BookService.CreateOneBook(book);

            return StatusCode(201, book);
        }
        catch (Exception ex)
        {
            return BadRequest(ex.Message);
        }
    }

    [HttpPut("{id:int}")]
    public async Task<IActionResult> UpdateOneBook(
        [FromRoute(Name = "id")] int id,
        [FromBody] Book book
    )
    {
        try
        {
            if (id != book.Id)
                return BadRequest();

            _manager.BookService.UpdateOneBook(id, book, trackChanges: true);
            return NoContent();
        }
        catch (Exception ex)
        {
            throw new Exception(ex.Message);
        }
    }

    [HttpDelete("{id:int}")]
    public async Task<IActionResult> DeleteAllBooks([FromRoute(Name = "id")] int id)
    {
        try
        {
            _manager.BookService.DeleteOneBook(id, trackChanges: true);
            return NoContent();
        }
        catch (Exception ex)
        {
            throw new Exception(ex.Message);
        }
    }

    [HttpPatch("{id:int}")]
    public async Task<IActionResult> PartiallyUpdateOneBook(
        [FromRoute(Name = "id")] int id,
        [FromBody] JsonPatchDocument<Book> bookPatch
    )
    {
        try
        {
            var entity = _manager.BookService.GetOneBookById(id, trackChanges: true);

            if (entity == null)
                return NotFound();

            bookPatch.ApplyTo(entity);
            _manager.BookService.UpdateOneBook(id, entity, trackChanges: true);
            return NoContent();
        }
        catch (Exception ex)
        {
            throw new Exception(ex.Message);
        }
    }
}
