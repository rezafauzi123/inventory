using InventoryManagement.Core.Abstractions;
using InventoryManagement.Domain.Entities;
using InventoryManagement.Shared.Abstractions.Databases;
using InventoryManagement.Shared.Abstractions.Encryption;
using Microsoft.EntityFrameworkCore;
using System.Linq.Expressions;

namespace InventoryManagement.Infrastructure.Services;

public class CategoryService : ICategoryService
{
    private readonly IDbContext _dbContext;
    private readonly ISalter _salter;

    public CategoryService(IDbContext dbContext, ISalter salter)
    {
        _dbContext = dbContext;
        _salter = salter;
    }

    public IQueryable<Category> GetBaseQuery()
        => _dbContext.Set<Category>()
            .AsQueryable();

    public Task<Category?> GetByIdAsync(Guid id, CancellationToken cancellationToken = default)
        => GetBaseQuery()
            .Where(e => e.Id == id)
            .FirstOrDefaultAsync(cancellationToken);

    public async Task<Category?> CreateAsync(Category entity, CancellationToken cancellationToken = default)
    {
        await _dbContext.InsertAsync(entity, cancellationToken);
        await _dbContext.SaveChangesAsync(cancellationToken);
        return entity;
    }

    public async Task DeleteAsync(Guid id, CancellationToken cancellationToken = default)
    {
        var Category = await GetByIdAsync(id, cancellationToken);
        if (Category is null)
            throw new Exception("Data not found");

        _dbContext.AttachEntity(Category);

        Category.SetToDeleted();

        await _dbContext.SaveChangesAsync(cancellationToken);
    }

    public Task<Category?> GetByExpressionAsync(
        Expression<Func<Category, bool>> predicate,
        Expression<Func<Category, Category>> projection,
        CancellationToken cancellationToken = default)
        => GetBaseQuery()
            .Where(predicate)
            .Select(projection)
            .FirstOrDefaultAsync(cancellationToken);

    public Task<Category?> GetByNameAsync(string name, CancellationToken cancellationToken = default)
    {
        var s = name.ToUpper();

        return GetBaseQuery()
            .Where(e => e.Name.ToUpper() == s)
            .FirstOrDefaultAsync(cancellationToken);
    }

    public Task<bool> IsCategoryExistAsync(string name, CancellationToken cancellationToken = default)
    {
        name = name.ToUpper();

        return GetBaseQuery().Where(e => e.Name.ToUpper() == name)
            .AnyAsync(cancellationToken);
    }
}