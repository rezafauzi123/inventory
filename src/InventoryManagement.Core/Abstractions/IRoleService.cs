using InventoryManagement.Domain.Entities;

namespace InventoryManagement.Core.Abstractions;

/// <summary>
/// Represents a service for managing roles.
/// </summary>
public interface IRoleService : IEntityService<Role>
{
    /// <summary>
    /// Retrieves a role by its code asynchronously.
    /// </summary>
    /// <param name="code">The code of the role to retrieve.</param>
    /// <param name="cancellationToken">The cancellation token to cancel operation if needed. (optional)</param>
    /// <returns>A task that represents the asynchronous operation. The task result contains the role with the specified code, or null if not found.</returns>
    Task<Role?> GetByCodeAsync(string code, CancellationToken cancellationToken = default);
}