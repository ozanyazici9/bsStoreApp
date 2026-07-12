using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Azure;
using bsStoreApp.Models;
using bsStoreApp.Repositories;
using Microsoft.AspNetCore.JsonPatch;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;

namespace bsStoreApp.Controllers;

[ApiController]
[Route("api/[controller]")]
public class BooksController : ControllerBase
{
    private readonly RepositoryContext _context;

    public BooksController(RepositoryContext context)
    {
        _context = context;
    }

    [HttpGet]
    public async Task<IActionResult> GetAllBooks()
    {
        try
        {
            var books = await _context.Books.ToListAsync();
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
            var book = await _context.Books.FindAsync(id);
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

            await _context.Books.AddAsync(book);
            await _context.SaveChangesAsync();

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

            var entity = await _context.Books.FindAsync(id);

            if (entity == null)
                return NotFound();

            entity.Title = book.Title;
            entity.Price = book.Price;

            await _context.SaveChangesAsync();
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
            var book = await _context.Books.FindAsync(id);

            if (book == null)
                return NotFound(
                    new { statusCode = 404, message = $"Book with id:{id} could not found." }
                );

            _context.Books.Remove(book);
            await _context.SaveChangesAsync();
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
            var entity = await _context.Books.FindAsync(id);
            if (entity == null)
                return NotFound();

            bookPatch.ApplyTo(entity);
            await _context.SaveChangesAsync();
            return NoContent();
        }
        catch (Exception ex)
        {
            throw new Exception(ex.Message);
        }
    }
}
