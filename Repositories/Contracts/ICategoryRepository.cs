using Entities.Models;
using Entities.RequestFeatures;

namespace Repositories.Contracts;

public interface ICategoryRepository : IRepositoryBase<Category>
{
    Task<IEnumerable<Category>> GetAllCategoriesAsync(CategoryParameters categoryParameters ,bool trackChanges);
    Task<Category> GetOneCategoryByIdAsync(int id, bool trackChanges);
    void CreateOneCategory(Category category);
    void DeleteOneCategory(Category category);
    void UpdateOneCategory(Category category);
}
