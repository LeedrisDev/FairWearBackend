using AutoMapper;
using BrandAndProductDatabase.API.DataAccess.IRepositories;
using BrandAndProductDatabase.API.Models;
using BrandAndProductDatabase.API.Models.Dto;

namespace BrandAndProductDatabase.API.Business.BrandBusiness;

/// <summary>Business logic for brands database actions </summary>
public class BrandBusiness : IBrandBusiness
{
    private readonly IBrandRepository _brandRepository;
    private readonly IMapper _mapper;

    /// <summary>
    /// Constructor for BrandBusiness.
    /// </summary>
    /// <param name="brandRepository"></param>
    /// <param name="mapper"></param>
    public BrandBusiness(IBrandRepository brandRepository, IMapper mapper)
    {
        _brandRepository = brandRepository;
        _mapper = mapper;
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