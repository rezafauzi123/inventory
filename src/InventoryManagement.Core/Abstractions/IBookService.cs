using InventoryManagement.Domain.Entities;

namespace InventoryManagement.Core.Abstractions;

/// <summary>
/// Default implementation is AsNoTracking true.
/// </summary>
public interface IBookService : IEntityService<Book>
{
    /// <summary>
    /// Get Book by Title.
    /// </summary>
    /// <param name="title">A Title as string</param>
    /// <param name="cancellationToken">See <see cref="CancellationToken"/></param>
    /// <returns>See <see cref="Book">Book</see></returns>
    Task<Book?> GetByTitleAsync(string title, CancellationToken cancellationToken = default);

    /// <summary>
    /// Check is title exist
    /// </summary>
    /// <param name="title">A Title as string</param>
    /// <param name="cancellationToken">See <see cref="CancellationToken"/></param>
    /// <returns>bool</returns>
    Task<bool> IsBookExistAsync(string title, CancellationToken cancellationToken = default);
}