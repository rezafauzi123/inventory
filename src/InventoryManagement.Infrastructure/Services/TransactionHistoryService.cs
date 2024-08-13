using InventoryManagement.Core.Abstractions;
using InventoryManagement.Domain.Entities;
using InventoryManagement.Shared.Abstractions.Databases;
using InventoryManagement.Shared.Abstractions.Encryption;
using Microsoft.EntityFrameworkCore;
using System.Linq.Expressions;

namespace InventoryManagement.Infrastructure.Services;

public class TransactionHistoryService : ITransactionHistoryService
{
    private readonly IDbContext _dbContext;
    private readonly ISalter _salter;

    public TransactionHistoryService(IDbContext dbContext, ISalter salter)
    {
        _dbContext = dbContext;
        _salter = salter;
    }

    public IQueryable<TransactionHistory> GetBaseQuery()
        => _dbContext.Set<TransactionHistory>()
            .AsQueryable();

    public Task<TransactionHistory?> GetByIdAsync(Guid id, CancellationToken cancellationToken = default)
        => GetBaseQuery()
            .Where(e => e.Id == id)
            .FirstOrDefaultAsync(cancellationToken);

    public async Task<TransactionHistory?> CreateAsync(TransactionHistory entity, CancellationToken cancellationToken = default)
    {
        await _dbContext.InsertAsync(entity, cancellationToken);
        await _dbContext.SaveChangesAsync(cancellationToken);
        return entity;
    }

    public async Task DeleteAsync(Guid id, CancellationToken cancellationToken = default)
    {
        var TransactionHistory = await GetByIdAsync(id, cancellationToken);
        if (TransactionHistory is null)
            throw new Exception("Data not found");

        _dbContext.AttachEntity(TransactionHistory);

        TransactionHistory.SetToDeleted();

        await _dbContext.SaveChangesAsync(cancellationToken);
    }

    public Task<TransactionHistory?> GetByExpressionAsync(
        Expression<Func<TransactionHistory, bool>> predicate,
        Expression<Func<TransactionHistory, TransactionHistory>> projection,
        CancellationToken cancellationToken = default)
        => GetBaseQuery()
            .Where(predicate)
            .Select(projection)
            .FirstOrDefaultAsync(cancellationToken);
}