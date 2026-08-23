using Entities.DataTransferObjects;
using Entities.Models;
using Entities.RequestFeatures;

namespace Services.Contracts;

public interface ICategoryService
{
    Task<IEnumerable<CategoryDto>> GetAllCategoriesAsync(CategoryParameters categoryParameters ,bool trackChanges);
    Task<CategoryDto> GetOneCategoryByIdAsync(int id, bool trackChanges);
    Task<CategoryDto> CreateOneCategoryAsync(CategoryDtoForInsertion categoryDto);
    Task UpdateOneCategoryAsync(int id, CategoryDtoForUpdate categoryDto, bool trackChanges);
    Task DeleteOneCategoryAsync(int id, bool trackChanges);
}
