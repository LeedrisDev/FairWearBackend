using BackOffice.Models;

namespace BackOffice.Business.Interfaces;

/// <summary>Represents the business logic interface for managing BrandModel entities.</summary>
public interface IBrandBusiness
{
    /// <summary>Asynchronously retrieves a collection of BrandModel entities.</summary>
    /// <param name="includeTables">A comma-separated list of related tables to include in the query.</param>
    /// <returns>A processing status response with a collection of BrandModel entities.</returns>
    Task<ProcessingStatusResponse<IEnumerable<BrandModel>>> GetAsync(string includeTables = "");

    /// <summary>Asynchronously inserts a BrandModel entity.</summary>
    /// <param name="entity">The BrandModel entity to insert.</param>
    /// <returns>A processing status response with the inserted BrandModel entity.</returns>
    Task<ProcessingStatusResponse<BrandModel>> InsertAsync(BrandModel entity);

    /// <summary>Asynchronously updates a BrandModel entity.</summary>
    /// <param name="entity">The BrandModel entity to update.</param>
    /// <returns>A processing status response with the updated BrandModel entity.</returns>
    Task<ProcessingStatusResponse<BrandModel>> UpdateAsync(BrandModel entity);

    /// <summary>Asynchronously deletes a BrandModel entity by its unique identifier.</summary>
    /// <param name="id">The unique identifier of the BrandModel entity to delete.</param>
    /// <returns>A processing status response with the deleted BrandModel entity.</returns>
    Task<ProcessingStatusResponse<BrandModel>> DeleteAsync(long id);

    /// <summary>Asynchronously finds a BrandModel entity by its unique identifier.</summary>
    /// <param name="id">The unique identifier of the BrandModel entity to find.</param>
    /// <returns>A processing status response with the found BrandModel entity.</returns>
    Task<ProcessingStatusResponse<BrandModel>> FindByIdAsync(long id);
}