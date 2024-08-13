using InventoryManagement.Domain.Entities;

namespace InventoryManagement.Core.Abstractions;

/// <summary>
/// Default implementation is AsNoTracking true.
/// </summary>
public interface IInventoryService : IEntityService<Inventory>
{
    Task<Inventory?> GetByBookIdAsync(Guid bookId, CancellationToken cancellationToken = default);
}