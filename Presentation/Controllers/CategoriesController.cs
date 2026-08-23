using Entities.DataTransferObjects;
using Entities.RequestFeatures;
using Microsoft.AspNetCore.Authorization;
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

    [Authorize]
    [HttpHead]
    [HttpGet(Name = "GetAllCategoriesAsync")]
    public async Task<IActionResult> GetAllCategoriesAsync(
        [FromQuery] CategoryParameters categoryParameters
    )
    {
        var categories = await _manager.CategoryService.GetAllCategoriesAsync(
            categoryParameters,
            false
        );

        return Ok(categories);
    }

    [Authorize]
    [HttpHead("{id:int}")]
    [HttpGet("{id:int}", Name = "GetOneCategoryAsync")]
    public async Task<IActionResult> GetOneCategoryAsync([FromRoute(Name = "id")] int id)
    {
        var categoryDto = await _manager.CategoryService.GetOneCategoryByIdAsync(id, false);

        return Ok(categoryDto);
    }

    [ServiceFilter(typeof(ValidationFilterAttribute))]
    [Authorize(Roles = "Admin, Editor")]
    [HttpPost(Name = "CreateOneCategory")]
    public async Task<IActionResult> CreateOneCategoryAsync(
        [FromBody] CategoryDtoForInsertion categoryDto
    )
    {
        var category = await _manager.CategoryService.CreateOneCategoryAsync(categoryDto);

        return StatusCode(201, category);
    }

    [ServiceFilter(typeof(ValidationFilterAttribute))]
    [Authorize(Roles = "Admin, Editor")]
    [HttpPut("{id:int}", Name = "UpdateOneCategory")]
    public async Task<IActionResult> UpdateOneCategory(
        [FromRoute(Name = "id")] int id,
        [FromBody] CategoryDtoForUpdate categoryDto
    )
    {
        if (id != categoryDto.Id)
            return BadRequest();

        await _manager.CategoryService.UpdateOneCategoryAsync(id, categoryDto, false);
        return NoContent();
    }

    [Authorize(Roles = "Admin")]
    [HttpDelete("{id:int}", Name = "DeleteOneCategoryAsync")]
    public async Task<IActionResult> DeleteOneCategoryAsync([FromRoute(Name = "id")] int id)
    {
        await _manager.CategoryService.DeleteOneCategoryAsync(id, true);
        return NoContent();
    }

    [Authorize]
    [HttpOptions]
    public IActionResult GetCategoryOptions()
    {
        Response.Headers.Add("Allow", "GET, PUT, POST,DELETE, OPTIONS, HEAD");
        return Ok();
    }
}
