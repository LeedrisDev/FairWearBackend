using BackOffice.Models;

namespace BackOffice.DataAccess.Repositories.Interfaces;

/// <summary>
/// Generic repository interface for data access operations on entities of type <typeparamref name="TModel"/>.
/// </summary>
/// <typeparam name="TModel">The type of entity managed by the repository.</typeparam>
public interface IRepository<TModel>
{
    /// <summary>Asynchronously retrieves a collection of entities of type <typeparamref name="TModel"/>.</summary>
    /// <param name="includeTables">A comma-separated list of related tables to include in the query.</param>
    /// <returns>
    /// An asynchronous task that returns a <see cref="ProcessingStatusResponse{TModel}"/> with a collection of entities.
    /// </returns>
    Task<ProcessingStatusResponse<IEnumerable<TModel>>> GetAsync(string includeTables = "");

    /// <summary>Asynchronously inserts an entity of type <typeparamref name="TModel"/>.</summary>
    /// <param name="entity">The entity to insert.</param>
    /// <returns>
    /// An asynchronous task that returns a <see cref="ProcessingStatusResponse{TModel}"/> with the inserted entity.
    /// </returns>
    Task<ProcessingStatusResponse<TModel>> InsertAsync(TModel entity);

    /// <summary>Asynchronously updates an entity of type <typeparamref name="TModel"/>.</summary>
    /// <param name="entity">The entity to update.</param>
    /// <returns>An asynchronous task that returns a processing status response with the updated entity.</returns>
    Task<ProcessingStatusResponse<TModel>> UpdateAsync(TModel entity);

    /// <summary>Asynchronously deletes an entity by its unique identifier.</summary>
    /// <param name="id">The unique identifier of the entity to delete.</param>
    /// <returns>
    /// An asynchronous task that returns a <see cref="ProcessingStatusResponse{TModel}"/> with the deleted entity.
    /// </returns>
    Task<ProcessingStatusResponse<TModel>> DeleteAsync(long id);

    /// <summary>Asynchronously finds an entity by its unique identifier.</summary>
    /// <param name="id">The unique identifier of the entity to find.</param>
    /// <returns>
    /// An asynchronous task that returns a <see cref="ProcessingStatusResponse{TModel}"/> with the found entity.
    /// </returns>
    Task<ProcessingStatusResponse<TModel>> FindByIdAsync(long id);
}

