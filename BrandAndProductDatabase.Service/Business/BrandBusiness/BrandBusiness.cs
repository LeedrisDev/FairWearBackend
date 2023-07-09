using System.Net;
using BrandAndProductDatabase.Service.DataAccess.BrandData;
using BrandAndProductDatabase.Service.DataAccess.Filters;
using BrandAndProductDatabase.Service.DataAccess.IRepositories;
using BrandAndProductDatabase.Service.Models;
using BrandAndProductDatabase.Service.Models.Dto;

namespace BrandAndProductDatabase.Service.Business.BrandBusiness;

/// <summary>Business logic for brands database actions </summary>
public class BrandBusiness : IBrandBusiness
{
    private readonly IBrandData _brandData;
    private readonly IBrandRepository _brandRepository;
    private readonly IFilterFactory<IFilter> _filterFactory;

    /// <summary>
    /// Constructor for BrandBusiness.
    /// </summary>
    /// <param name="brandRepository"></param>
    /// <param name="brandData"></param>
    public BrandBusiness(IBrandRepository brandRepository, IBrandData brandData, IFilterFactory<IFilter> filterFactory)
    {
        _brandRepository = brandRepository;
        _brandData = brandData;
        _filterFactory = filterFactory;
    }

    /// <inheritdoc/>
    public async Task<ProcessingStatusResponse<IEnumerable<BrandDto>>> GetAllBrandsAsync(
        Dictionary<string, string> filters)
    {
        var filter = _filterFactory.CreateFilter(filters);

        return await _brandRepository.GetAllAsync(filter);
    }

    /// <inheritdoc/>
    public async Task<ProcessingStatusResponse<BrandDto>> GetBrandByIdAsync(int id)
    {
        return await _brandRepository.GetByIdAsync(id);
    }

    /// <inheritdoc/>
    public async Task<ProcessingStatusResponse<BrandDto>> GetBrandByNameAsync(string name)
    {
        var brandName = TreatBrandName(name);
        var processingStatusResponse = await _brandRepository.GetBrandByNameAsync(brandName);
        if (processingStatusResponse.Status == HttpStatusCode.OK)
            return processingStatusResponse;

        var brandDataResponse = _brandData.GetBrandByName(brandName);
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
    
    private static string TreatBrandName(string brandName)
    {
        var treatedName = brandName
            .ToLower()
            .Replace(" ", "-")
            .Replace("'", "");

        return treatedName;
    }
}