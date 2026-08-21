using Entities.DataTransferObjects;
using Entities.Models;

namespace Services.Contracts;

public interface ICategoryService
{
    Task<IEnumerable<CategoryDto>> GetAllCategoriesAsync(bool trackChanges);
    Task<CategoryDto> GetOneCategoryByIdAsync(int id, bool trackChanges);
    Task<CategoryDto> CreateOneCategoryAsync(CategoryDtoForInsertion categoryDto);
    Task UpdateOneCategoryAsync(int id, CategoryDtoForUpdate categoryDto, bool trackChanges);
    Task DeleteOneCategoryAsync(int id, bool trackChanges);
}
