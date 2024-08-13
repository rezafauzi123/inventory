using InventoryManagement.Core.Abstractions;
using InventoryManagement.Domain.Entities;
using InventoryManagement.Shared.Abstractions.Databases;
using InventoryManagement.Shared.Abstractions.Encryption;
using Microsoft.EntityFrameworkCore;
using System.Linq.Expressions;

namespace InventoryManagement.Infrastructure.Services;

public class PublisherService : IPublisherService
{
    private readonly IDbContext _dbContext;
    private readonly ISalter _salter;

    public PublisherService(IDbContext dbContext, ISalter salter)
    {
        _dbContext = dbContext;
        _salter = salter;
    }

    public IQueryable<Publisher> GetBaseQuery()
        => _dbContext.Set<Publisher>()
            .AsQueryable();

    public Task<Publisher?> GetByIdAsync(Guid id, CancellationToken cancellationToken = default)
        => GetBaseQuery()
            .Where(e => e.Id == id)
            .FirstOrDefaultAsync(cancellationToken);

    public async Task<Publisher?> CreateAsync(Publisher entity, CancellationToken cancellationToken = default)
    {
        await _dbContext.InsertAsync(entity, cancellationToken);
        await _dbContext.SaveChangesAsync(cancellationToken);
        return entity;
    }

    public async Task DeleteAsync(Guid id, CancellationToken cancellationToken = default)
    {
        var Publisher = await GetByIdAsync(id, cancellationToken);
        if (Publisher is null)
            throw new Exception("Data not found");

        _dbContext.AttachEntity(Publisher);

        Publisher.SetToDeleted();

        await _dbContext.SaveChangesAsync(cancellationToken);
    }

    public Task<Publisher?> GetByExpressionAsync(
        Expression<Func<Publisher, bool>> predicate,
        Expression<Func<Publisher, Publisher>> projection,
        CancellationToken cancellationToken = default)
        => GetBaseQuery()
            .Where(predicate)
            .Select(projection)
            .FirstOrDefaultAsync(cancellationToken);

    public Task<Publisher?> GetByNameAsync(string name, CancellationToken cancellationToken = default)
    {
        var s = name.ToUpper();

        return GetBaseQuery()
            .Where(e => e.Name.ToUpper() == s)
            .FirstOrDefaultAsync(cancellationToken);
    }

    public Task<bool> IsPublisherExistAsync(string name, CancellationToken cancellationToken = default)
    {
        name = name.ToUpper();

        return GetBaseQuery().Where(e => e.Name.ToUpper() == name)
            .AnyAsync(cancellationToken);
    }
}