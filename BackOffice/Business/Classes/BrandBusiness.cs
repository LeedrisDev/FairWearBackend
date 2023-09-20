using BackOffice.Business.Interfaces;
using BackOffice.DataAccess.Repositories.Interfaces;
using BackOffice.Models;

namespace BackOffice.Business.Classes;

/// <summary>Represents the business logic for managing <see cref="BrandModel"/> entities.</summary>
public class BrandBusiness : IBrandBusiness
{
    private readonly IBrandRepository _brandRepository;
    
    /// <summary>Initializes a new instance of the <see cref="BrandBusiness"/> class.</summary>
    /// <param name="brandRepository">The repository for data access operations related to brands.</param>
    public BrandBusiness(IBrandRepository brandRepository)
    {
        _brandRepository = brandRepository;
    }
    
    /// <inheritdoc/>
    public async Task<ProcessingStatusResponse<IEnumerable<BrandModel>>> GetAsync(string includeTables = "")
    {
        var response = await _brandRepository.GetAsync(includeTables);
        var orderedBrands = response.Entity.OrderBy(e => e.Name);
        response.Entity = orderedBrands;
        return response;
    }

    /// <inheritdoc/>
    public async Task<ProcessingStatusResponse<BrandModel>> InsertAsync(BrandModel entity)
    {
        return await _brandRepository.InsertAsync(entity);
    }

    /// <inheritdoc/>
    public async Task<ProcessingStatusResponse<BrandModel>> UpdateAsync(BrandModel entity)
    {
        return await _brandRepository.UpdateAsync(entity);
    }

    /// <inheritdoc/>
    public async Task<ProcessingStatusResponse<BrandModel>> DeleteAsync(long id)
    {
        return await _brandRepository.DeleteAsync(id);
    }

    /// <inheritdoc/>
    public async Task<ProcessingStatusResponse<BrandModel>> FindByIdAsync(long id)
    {
        return await _brandRepository.FindByIdAsync(id);
    }

    /// <inheritdoc/>
    public async Task<ProcessingStatusResponse<BrandModel>> FindByNameAsync(string name)
    {
        return await _brandRepository.FindByNameAsync(name);
    }
}