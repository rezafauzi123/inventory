using InventoryManagement.Shared.Abstractions.Entities;
using System.Linq.Expressions;

namespace InventoryManagement.Core.Abstractions;

/// <summary>
/// Represents a service for managing entity operations.
/// </summary>
/// <typeparam name="T">The type of entity.</typeparam>
public interface IEntityService<T> where T : BaseEntity
{
    /// <summary>
    /// Retrieves the base query for the specified entity type.
    /// </summary>
    /// <typeparam name="T">The type of entity.</typeparam>
    /// <returns>The base query for the specified entity type.</returns>
    IQueryable<T> GetBaseQuery();

    /// <summary>
    /// Retrieves an object with the specified ID asynchronously.
    /// </summary>
    /// <typeparam name="T">The type of object to retrieve.</typeparam>
    /// <param name="id">The ID of the object to retrieve.</param>
    /// <param name="cancellationToken">A cancellation token that can be used to cancel the operation.</param>
    /// <returns>A task representing the asynchronous operation. The task result contains the retrieved object, or null if no object is found.</returns>
    Task<T?> GetByIdAsync(Guid id, CancellationToken cancellationToken = default);

    /// <summary>
    /// Creates a new entity asynchronously.
    /// </summary>
    /// <param name="entity">The entity to create.</param>
    /// <param name="cancellationToken">The cancellation token.</param>
    /// <returns>A task representing the asynchronous operation. The task result contains the created entity if successful, otherwise null.</returns>
    Task<T?> CreateAsync(T entity, CancellationToken cancellationToken = default);

    /// <summary>
    /// Deletes an item asynchronously.
    /// </summary>
    /// <param name="id">The ID of the item to delete.</param>
    /// <param name="cancellationToken">The cancellation token.</param>
    /// <returns>A task representing the asynchronous operation.</returns>
    Task DeleteAsync(Guid id, CancellationToken cancellationToken = default);

    /// <summary>
    /// Retrieves a single entity from the database using the specified predicate expression
    /// and returns the result of applying the projection expression on the retrieved entity.
    /// </summary>
    /// <typeparam name="TEntity">The type of the entity to retrieve.</typeparam>
    /// <typeparam name="TResult">The type of the result after applying the projection.</typeparam>
    /// <param name="predicate">
    /// A predicate expression used to filter the entities in the database and identify
    /// the entity to retrieve.
    /// </param>
    /// <param name="projection">
    /// A projection expression applied on the retrieved entity to transform it to the desired result.
    /// </param>
    /// <param name="cancellationToken">
    /// Optional. A cancellation token that can be used to cancel the retrieval operation.
    /// </param>
    /// <returns>
    /// A task containing the retrieved entity if found, or null if no entity is found that satisfies
    /// the specified predicate expression.
    /// </returns>
    Task<T?> GetByExpressionAsync(Expression<Func<T, bool>> predicate,
        Expression<Func<T, T>> projection,
        CancellationToken cancellationToken = default);
}