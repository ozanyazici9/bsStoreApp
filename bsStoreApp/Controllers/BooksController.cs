using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Azure;
using Entities.Models;
using Microsoft.AspNetCore.JsonPatch;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using Repositories.Contracts;
using Repositories.EFCore;

namespace bsStoreApp.Controllers;

[ApiController]
[Route("api/[controller]")]
public class BooksController : ControllerBase
{
    private readonly IRepositoryManager _manager;

    public BooksController(IRepositoryManager manager)
    {
        _manager = manager;
    }

    [HttpGet]
    public async Task<IActionResult> GetAllBooks()
    {
        try
        {
            var books = _manager.Book.GetAllBooks(trackChanges: false);
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
            var book = _manager.Book.GetOneBookById(id, trackChanges: false);
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

            _manager.Book.CreateOneBook(book);
            _manager.Save();

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
            if (book == null)
                return BadRequest();

            if (id != book.Id)
                return BadRequest();

            var entity = _manager.Book.GetOneBookById(id, trackChanges: true);

            entity.Title = book.Title;
            entity.Price = book.Price;

            _manager.Save();
            return Ok(entity);
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
            var book = _manager.Book.GetOneBookById(id, trackChanges: false);

            if (book == null)
                return NotFound(
                    new { statusCode = 404, message = $"Book with id:{id} could not found." }
                );

            _manager.Book.DeleteOneBook(book);
            _manager.Save();
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
            var entity = _manager.Book.GetOneBookById(id, trackChanges: true);
            if (entity == null)
                return NotFound();

            bookPatch.ApplyTo(entity);
            _manager.Book.UpdateOneBook(entity);
            return NoContent();
        }
        catch (Exception ex)
        {
            throw new Exception(ex.Message);
        }
    }
}
