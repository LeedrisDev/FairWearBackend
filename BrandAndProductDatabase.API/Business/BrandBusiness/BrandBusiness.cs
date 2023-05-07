using AutoMapper;
using BrandAndProductDatabase.API.DataAccess.IRepositories;
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
    public async Task<IEnumerable<BrandDto>> GetAllBrandsAsync()
    {
        var brands = await _brandRepository.GetAllAsync();
        return _mapper.Map<IEnumerable<BrandDto>>(brands.Object);
    }

    /// <inheritdoc/>
    public async Task<BrandDto> GetBrandByIdAsync(int id)
    {
        var brand = await _brandRepository.GetByIdAsync(id);
        return _mapper.Map<BrandDto>(brand.Object);
    }

    /// <inheritdoc/>
    public async Task<BrandDto> CreateBrandAsync(BrandDto brandDto)
    {
        var brand = await _brandRepository.AddAsync(brandDto);
        return _mapper.Map<BrandDto>(brand.Object);
    }

    /// <inheritdoc/>
    public async Task<BrandDto> UpdateBrandAsync(BrandDto brandDto)
    {
        var brand = await _brandRepository.GetByIdAsync(brandDto.Id);
        if (brand == null)
        {
            // Handle the case where the brand doesn't exist
            return null;
        }

        brand = await _brandRepository.UpdateAsync(brandDto);
        return _mapper.Map<BrandDto>(brand.Object);
    }

    /// <inheritdoc/>
    public async Task DeleteBrandAsync(int id)
    {
        throw new NotImplementedException();
    }
}