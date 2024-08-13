using InventoryManagement.Domain.Entities;

namespace InventoryManagement.Core.Abstractions;

/// <summary>
/// Default implementation is AsNoTracking true.
/// </summary>
public interface ITransactionHistoryService : IEntityService<TransactionHistory>
{
    /// <summary>
    /// Get BookCategory by Name.
    /// </summary>
    /// <param name="cancellationToken">See <see cref="CancellationToken"/></param>
    Task<TransactionHistory?> CreateAsync(TransactionHistory entity, CancellationToken cancellationToken = default);
}