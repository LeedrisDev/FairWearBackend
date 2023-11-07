using BackOffice.Models;

namespace BackOffice.DataAccess.Repositories.Interfaces;

/// <summary>Represents a repository interface for managing brand entities.</summary>
public interface IBrandRepository : IRepository<BrandModel>
{
    /// <summary>Asynchronously finds a <see cref="BrandModel"/> entity by its name.</summary>
    /// <param name="name">The name of the <see cref="BrandModel"/> entity to find.</param>
    /// <returns>A processing status response with the found <see cref="BrandModel"/> entity.</returns>
    Task<ProcessingStatusResponse<BrandModel>> FindByNameAsync(string name);

}
