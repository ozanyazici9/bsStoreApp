using System.Collections.Immutable;
using Entities.Models;
using Entities.RequestFeatures;
using Microsoft.EntityFrameworkCore;
using Repositories.Contracts;
using Repositories.EFCore;
using Repositories.EFCore.Extensions;

namespace Repositories;

public sealed class CategoryRepository : RepositoryBase<Category>, ICategoryRepository
{
    public CategoryRepository(RepositoryContext repositoryContext)
        : base(repositoryContext) { }

    public void CreateOneCategory(Category category) => Create(category);

    public void DeleteOneCategory(Category category) => Delete(category);

    public async Task<IEnumerable<Category>> GetAllCategoriesAsync(
        CategoryParameters categoryParameters,
        bool trackChanges
    )
    {
        return await FindAll(trackChanges)
            .SearchCategory(categoryParameters.SearchTerm)
            .Sort(categoryParameters.OrderBy)
            .ToListAsync();
    }

    public async Task<Category> GetOneCategoryByIdAsync(int id, bool trackChanges)
    {
        return await FindByCondition(c => c.Id.Equals(id), trackChanges).SingleOrDefaultAsync();
    }

    public void UpdateOneCategory(Category category) => Update(category);
}
