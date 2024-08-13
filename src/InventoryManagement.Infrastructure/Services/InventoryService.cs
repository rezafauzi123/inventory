using InventoryManagement.Core.Abstractions;
using InventoryManagement.Domain.Entities;
using InventoryManagement.Shared.Abstractions.Databases;
using InventoryManagement.Shared.Abstractions.Encryption;
using Microsoft.EntityFrameworkCore;
using System.Linq.Expressions;

namespace InventoryManagement.Infrastructure.Services;

public class InventoryService : IInventoryService
{
    private readonly IDbContext _dbContext;
    private readonly ISalter _salter;

    public InventoryService(IDbContext dbContext, ISalter salter)
    {
        _dbContext = dbContext;
        _salter = salter;
    }

    public IQueryable<Inventory> GetBaseQuery()
        => _dbContext.Set<Inventory>()
            .AsQueryable();

    public Task<Inventory?> GetByIdAsync(Guid id, CancellationToken cancellationToken = default)
        => GetBaseQuery()
            .Where(e => e.Id == id)
            .FirstOrDefaultAsync(cancellationToken);

    public async Task<Inventory?> CreateAsync(Inventory entity, CancellationToken cancellationToken = default)
    {
        await _dbContext.InsertAsync(entity, cancellationToken);
        await _dbContext.SaveChangesAsync(cancellationToken);
        return entity;
    }

    public async Task<Inventory?> UpdateAsync(Inventory entity, CancellationToken cancellationToken = default)
    {
        await _dbContext.SaveChangesAsync(cancellationToken);
        return entity;
    }

    public async Task DeleteAsync(Guid id, CancellationToken cancellationToken = default)
    {
        var Inventory = await GetByIdAsync(id, cancellationToken);
        if (Inventory is null)
            throw new Exception("Data not found");

        _dbContext.AttachEntity(Inventory);

        Inventory.SetToDeleted();

        await _dbContext.SaveChangesAsync(cancellationToken);
    }

    public Task<Inventory?> GetByExpressionAsync(
        Expression<Func<Inventory, bool>> predicate,
        Expression<Func<Inventory, Inventory>> projection,
        CancellationToken cancellationToken = default)
        => GetBaseQuery()
            .Where(predicate)
            .Select(projection)
            .FirstOrDefaultAsync(cancellationToken);

    public Task<Inventory?> GetByBookIdAsync(Guid bookId, CancellationToken cancellationToken = default)
    {
        return GetBaseQuery()
            .Where(e => e.BookId == bookId)
            .FirstOrDefaultAsync(cancellationToken);
    }
}