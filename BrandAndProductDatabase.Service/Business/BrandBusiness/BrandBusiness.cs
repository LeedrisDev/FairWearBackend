using System.Net;
using BrandAndProductDatabase.Service.DataAccess.BrandData;
using BrandAndProductDatabase.Service.DataAccess.IRepositories;
using BrandAndProductDatabase.Service.Models;
using BrandAndProductDatabase.Service.Models.Dto;

namespace BrandAndProductDatabase.Service.Business.BrandBusiness;

/// <summary>Business logic for brands database actions </summary>
public class BrandBusiness : IBrandBusiness
{
    private readonly IBrandData _brandData;
    private readonly IBrandRepository _brandRepository;

    /// <summary>
    /// Constructor for BrandBusiness.
    /// </summary>
    /// <param name="brandRepository"></param>
    /// <param name="brandData"></param>
    public BrandBusiness(IBrandRepository brandRepository, IBrandData brandData)
    {
        _brandRepository = brandRepository;
        _brandData = brandData;
    }

    /// <inheritdoc/>
    public async Task<ProcessingStatusResponse<IEnumerable<BrandDto>>> GetAllBrandsAsync()
    {
        return await _brandRepository.GetAllAsync();
    }

    /// <inheritdoc/>
    public async Task<ProcessingStatusResponse<BrandDto>> GetBrandByIdAsync(int id)
    {
        return await _brandRepository.GetByIdAsync(id);
    }

    /// <inheritdoc/>
    public async Task<ProcessingStatusResponse<BrandDto>> GetBrandByNameAsync(string name)
    {
        var processingStatusResponse = await _brandRepository.GetBrandByNameAsync(name);
        if (processingStatusResponse.Status == HttpStatusCode.OK)
            return processingStatusResponse;

        var brandDataResponse = await _brandData.GetBrandByNameAsync(name);
        if (brandDataResponse.Status != HttpStatusCode.OK)
            return brandDataResponse;

        var repositoryResponse = await _brandRepository.AddAsync(brandDataResponse.Object);
        return repositoryResponse;
    }

    /// <inheritdoc/>
    public async Task<ProcessingStatusResponse<BrandDto>> CreateBrandAsync(BrandDto brandDto)
    {
        return await _brandRepository.AddAsync(brandDto);
    }

    /// <inheritdoc/>
    public async Task<ProcessingStatusResponse<BrandDto>> UpdateBrandAsync(BrandDto brandDto)
    {
        return await _brandRepository.UpdateAsync(brandDto);
    }

    /// <inheritdoc/>
    public async Task<ProcessingStatusResponse<BrandDto>> DeleteBrandAsync(int id)
    {
        return await _brandRepository.DeleteAsync(id);
    }
}