using AutoMapper;
using Entities.DataTransferObjects;
using Entities.Exceptions;
using Entities.Models;
using Entities.RequestFeatures;
using Repositories.Contracts;
using Services.Contracts;

namespace Services;

public class CategoryManager : ICategoryService
{
    private readonly IMapper _mapper;
    private readonly IRepositoryManager _manager;

    public CategoryManager(IRepositoryManager manager, IMapper mapper)
    {
        _manager = manager;
        _mapper = mapper;
    }

    public async Task<CategoryDto> CreateOneCategoryAsync(CategoryDtoForInsertion categoryDto)
    {
        var entity = _mapper.Map<Category>(categoryDto);
        _manager.Category.CreateOneCategory(entity);
        await _manager.SaveAsync();

        return _mapper.Map<CategoryDto>(entity);
    }

    public async Task DeleteOneCategoryAsync(int id, bool trackChanges)
    {
        var entity = await GetOneCategoryAndCheckExists(id, trackChanges);
        
        _manager.Category.DeleteOneCategory(entity);
        await _manager.SaveAsync();
    }

    public async Task<IEnumerable<CategoryDto>> GetAllCategoriesAsync(CategoryParameters categoryParameters ,bool trackChanges)
    {
        var entity = await _manager.Category.GetAllCategoriesAsync(categoryParameters ,trackChanges);

        return _mapper.Map<IEnumerable<CategoryDto>>(entity);
    }

    public async Task<CategoryDto> GetOneCategoryByIdAsync(int id, bool trackChanges)
    {
        var entity = await GetOneCategoryAndCheckExists(id, trackChanges);

        return _mapper.Map<CategoryDto>(entity);
    }

    public async Task UpdateOneCategoryAsync(int id, CategoryDtoForUpdate categoryDto, bool trackChanges)
    {
        await GetOneCategoryAndCheckExists(id, trackChanges);

        var entity = _mapper.Map<Category>(categoryDto);
        _manager.Category.UpdateOneCategory(entity);
        await _manager.SaveAsync();
    }

    private async Task<Category> GetOneCategoryAndCheckExists(int id, bool trackChanges)
    {
        var entity = await _manager.Category.GetOneCategoryByIdAsync(id, trackChanges);

        if (entity is null)
            throw new CategoryNotFoundException(id);

        return entity;
    }
}
