using InventoryManagement.Core.Abstractions;
using InventoryManagement.Domain.Entities;
using InventoryManagement.Shared.Abstractions.Databases;
using InventoryManagement.Shared.Abstractions.Encryption;
using Microsoft.EntityFrameworkCore;
using System.Linq.Expressions;

namespace InventoryManagement.Infrastructure.Services;

public class AuthorService : IAuthorService
{
    private readonly IDbContext _dbContext;
    private readonly ISalter _salter;

    public AuthorService(IDbContext dbContext, ISalter salter)
    {
        _dbContext = dbContext;
        _salter = salter;
    }

    public IQueryable<Author> GetBaseQuery()
        => _dbContext.Set<Author>()
            .AsQueryable();

    public Task<Author?> GetByIdAsync(Guid id, CancellationToken cancellationToken = default)
        => GetBaseQuery()
            .Where(e => e.Id == id)
            .FirstOrDefaultAsync(cancellationToken);

    public async Task<Author?> CreateAsync(Author entity, CancellationToken cancellationToken = default)
    {
        await _dbContext.InsertAsync(entity, cancellationToken);
        await _dbContext.SaveChangesAsync(cancellationToken);
        return entity;
    }

    public async Task DeleteAsync(Guid id, CancellationToken cancellationToken = default)
    {
        var Author = await GetByIdAsync(id, cancellationToken);
        if (Author is null)
            throw new Exception("Data not found");

        _dbContext.AttachEntity(Author);

        Author.SetToDeleted();

        await _dbContext.SaveChangesAsync(cancellationToken);
    }

    public Task<Author?> GetByExpressionAsync(
        Expression<Func<Author, bool>> predicate,
        Expression<Func<Author, Author>> projection,
        CancellationToken cancellationToken = default)
        => GetBaseQuery()
            .Where(predicate)
            .Select(projection)
            .FirstOrDefaultAsync(cancellationToken);

    public Task<Author?> GetByNameAsync(string name, CancellationToken cancellationToken = default)
    {
        var s = name.ToUpper();

        return GetBaseQuery()
            .Where(e => e.Name.ToUpper() == s)
            .FirstOrDefaultAsync(cancellationToken);
    }

    public Task<bool> IsAuthorExistAsync(string name, CancellationToken cancellationToken = default)
    {
        name = name.ToUpper();

        return GetBaseQuery().Where(e => e.Name.ToUpper() == name)
            .AnyAsync(cancellationToken);
    }
}