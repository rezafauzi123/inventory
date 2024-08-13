using InventoryManagement.Domain.Entities;

namespace InventoryManagement.Core.Abstractions;

/// <summary>
/// Default implementation is AsNoTracking true.
/// </summary>
public interface ICategoryService : IEntityService<Category>
{
    /// <summary>
    /// Get Category by Name.
    /// </summary>
    /// <param name="name">A Name as string</param>
    /// <param name="cancellationToken">See <see cref="CancellationToken"/></param>
    /// <returns>See <see cref="Category">Category</see></returns>
    Task<Category?> GetByNameAsync(string name, CancellationToken cancellationToken = default);

    /// <summary>
    /// Check is name exist
    /// </summary>
    /// <param name="name">A Name as string</param>
    /// <param name="cancellationToken">See <see cref="CancellationToken"/></param>
    /// <returns>bool</returns>
    Task<bool> IsCategoryExistAsync(string name, CancellationToken cancellationToken = default);
}