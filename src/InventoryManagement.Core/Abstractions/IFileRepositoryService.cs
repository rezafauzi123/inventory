using InventoryManagement.Domain.Entities;

namespace InventoryManagement.Core.Abstractions;

/// <summary>
/// Represents a file repository service.
/// </summary>
public interface IFileRepositoryService : IEntityService<FileRepository>
{
    /// <summary>
    /// Retrieves the total used storage asynchronously.
    /// </summary>
    /// <param name="cancellationToken">A token to cancel the operation.</param>
    /// <returns>The total used storage in bytes.</returns>
    Task<long> TotalUsedStorageAsync(CancellationToken cancellationToken = default);

    /// <summary>
    /// Retrieves a file from the repository by its unique file name.
    /// </summary>
    /// <param name="fileName">The unique file name to search for.</param>
    /// <param name="cancellationToken">The cancellation token to cancel the async operation (optional).</param>
    /// <returns>A task representing the asynchronous operation. The task result is the <see cref="FileRepository"/> object if found, or null if not found.</returns>
    Task<FileRepository?> GetByUniqueFileNameAsync(string fileName, CancellationToken cancellationToken = default);
}