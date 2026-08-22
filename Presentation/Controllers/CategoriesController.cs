using Entities.DataTransferObjects;
using Microsoft.AspNetCore.Mvc;
using Presentation.ActionFilters;
using Services.Contracts;

namespace Presentation.Controllers;

[ServiceFilter(typeof(LogFilterAttribute))]
[ApiController]
[Route("api/[controller]")]
public class CategoriesController : ControllerBase
{
    private readonly IServiceManager _manager;

    public CategoriesController(IServiceManager manager)
    {
        _manager = manager;
    }

    [HttpHead]
    [HttpGet(Name = "GetAllCategoriesAsync")]
    public async Task<IActionResult> GetAllCategoriesAsync()
    {
        var categories = await _manager.CategoryService.GetAllCategoriesAsync(false);

        return Ok(categories);
    }

    [HttpHead]
    [HttpGet("{id:int}", Name = "GetOneCategoryAsync")]
    public async Task<IActionResult> GetOneCategoryAsync([FromRoute(Name = "id")] int id)
    {
        var categoryDto = await _manager.CategoryService.GetOneCategoryByIdAsync(id, false);

        return Ok(categoryDto);
    }

    [ServiceFilter(typeof(ValidationFilterAttribute))]
    [HttpPost(Name = "CreateOneCategory")]
    public async Task<IActionResult> CreateOneCategoryAsync(
        [FromBody] CategoryDtoForInsertion categoryDto
    )
    {
        var category = await _manager.CategoryService.CreateOneCategoryAsync(categoryDto);

        return StatusCode(201, category);
    }

    [ServiceFilter(typeof(ValidationFilterAttribute))]
    [HttpPut("{id:int}")]
    public async Task<IActionResult> UpdateOneCategory(
        [FromRoute(Name = "id")] int id,
        [FromBody] CategoryDtoForUpdate categoryDto
    )
    {
        if (id != categoryDto.CategoryId)
            return BadRequest();

        await _manager.CategoryService.UpdateOneCategoryAsync(id, categoryDto, false);
        return NoContent();
    }

    [HttpDelete("{id:int}", Name = "DeleteOneCategoryAsync")]
    public async Task<IActionResult> DeleteOneCategoryAsync([FromRoute(Name = "id")] int id)
    {
        await _manager.CategoryService.DeleteOneCategoryAsync(id, true);
        return NoContent();
    }
}
