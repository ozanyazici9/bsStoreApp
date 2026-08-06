using Asp.Versioning;
using Microsoft.AspNetCore.Mvc;
using Services.Contracts;

namespace Presentation.Controllers;

[ApiVersion("2.0")]
[Route("api/books")]
[ApiController]
public class BooksV2Controller : ControllerBase
{
     private readonly IServiceManager _manager;

     public BooksV2Controller(IServiceManager manager)
     {
         _manager = manager;
     }

     [HttpGet]
     public async Task<IActionResult> GetAllBooksAsync()
     {
         var books = await _manager.BookService.GetAllBooksAsync(trackChanges: false);
         return Ok(books);
     }
}
