using FairWearGateway.API.DataAccess.BrandData;
using FairWearGateway.API.Models;
using FairWearGateway.API.Models.Response;

namespace FairWearGateway.API.Business.BrandBusiness;


/// <summary>Class that handles the business logic for the Brand model.</summary>
public class BrandBusiness : IBrandBusiness
{
    private readonly IBrandData _brandData;

    /// <summary>Initializes a new instance of the <see cref="BrandBusiness"/> class.</summary>
    public BrandBusiness(IBrandData brandData)
    {
        _brandData = brandData;
    }
    
    /// <inheritdoc/>
    public async Task<ProcessingStatusResponse<IEnumerable<BrandResponse>>> GetAllBrandsAsync()
    {
        return await _brandData.GetAllBrandsAsync();
    }

    /// <inheritdoc/>
    public async Task<ProcessingStatusResponse<BrandResponse>> GetBrandByIdAsync(int brandId)
    {
        return await _brandData.GetBrandByIdAsync(brandId);
    }
}