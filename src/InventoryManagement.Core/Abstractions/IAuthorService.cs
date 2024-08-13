using InventoryManagement.Domain.Entities;

namespace InventoryManagement.Core.Abstractions;

/// <summary>
/// Default implementation is AsNoTracking true.
/// </summary>
public interface IAuthorService : IEntityService<Author>
{
    /// <summary>
    /// Get Author by Name.
    /// </summary>
    /// <param name="name">A Name as string</param>
    /// <param name="cancellationToken">See <see cref="CancellationToken"/></param>
    /// <returns>See <see cref="Author">Author</see></returns>
    Task<Author?> GetByNameAsync(string name, CancellationToken cancellationToken = default);

    /// <summary>
    /// Check is name exist
    /// </summary>
    /// <param name="name">A Name as string</param>
    /// <param name="cancellationToken">See <see cref="CancellationToken"/></param>
    /// <returns>bool</returns>
    Task<bool> IsAuthorExistAsync(string name, CancellationToken cancellationToken = default);
}