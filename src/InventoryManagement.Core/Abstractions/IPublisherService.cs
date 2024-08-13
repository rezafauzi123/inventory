using InventoryManagement.Domain.Entities;

namespace InventoryManagement.Core.Abstractions;

/// <summary>
/// Default implementation is AsNoTracking true.
/// </summary>
public interface IPublisherService : IEntityService<Publisher>
{
    /// <summary>
    /// Get Publisher by Name.
    /// </summary>
    /// <param name="name">A Name as string</param>
    /// <param name="cancellationToken">See <see cref="CancellationToken"/></param>
    /// <returns>See <see cref="Publisher">Publisher</see></returns>
    Task<Publisher?> GetByNameAsync(string name, CancellationToken cancellationToken = default);

    /// <summary>
    /// Check is name exist
    /// </summary>
    /// <param name="name">A Name as string</param>
    /// <param name="cancellationToken">See <see cref="CancellationToken"/></param>
    /// <returns>bool</returns>
    Task<bool> IsPublisherExistAsync(string name, CancellationToken cancellationToken = default);
}