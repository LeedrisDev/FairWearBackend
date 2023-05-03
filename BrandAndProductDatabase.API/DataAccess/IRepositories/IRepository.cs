using BrandAndProductDatabase.API.Models;

namespace BrandAndProductDatabase.API.DataAccess.IRepositories;

/// <summary>
/// Interface for a generic repository that provides common data access operations for any entity type.
/// </summary>
/// <typeparam name="TModel">The type of the entity for which the repository will be used.</typeparam>

public interface IRepository<TModel> 
    where TModel: class, IObjectWithId
{
    /// <summary>Gets all entities from the data store.</summary>
    /// <returns>A <see cref="Task"/> representing the asynchronous operation.</returns>
    Task<ProcessingStatusResponse<IEnumerable<TModel>>> GetAllAsync();

    /// <summary>Gets a single entity from the data store by its ID.</summary>
    /// <param name="id">The ID of the entity to retrieve.</param>
    /// <returns>A <see cref="Task"/> representing the asynchronous operation.</returns>
    Task<ProcessingStatusResponse<TModel>> GetByIdAsync(int id);

    /// <summary>Adds a new entity to the data store.</summary>
    /// <param name="entity">The entity to add.</param>
    /// <returns>A <see cref="Task"/> representing the asynchronous operation.</returns>
    Task<ProcessingStatusResponse<TModel>> AddAsync(TModel entity);

    /// <summary>Updates an existing entity in the data store.</summary>
    /// <param name="entity">The entity to update.</param>
    /// <returns>A <see cref="Task"/> representing the asynchronous operation.</returns>
    Task<ProcessingStatusResponse<TModel>> UpdateAsync(TModel entity);

    /// <summary>Deletes an entity from the data store by its ID.</summary>
    /// <param name="id">The ID of the entity to delete.</param>
    /// <returns>A <see cref="Task"/> representing the asynchronous operation.</returns>
    Task<ProcessingStatusResponse<TModel>> DeleteAsync(int id);
}